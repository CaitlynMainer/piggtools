using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace ParagonForge.PiggInterface {
  /// <summary>
  /// Correlates a PiggLeaf object to a physical file or files.
  /// </summary>
  public class PiggLeafInfo {
    #region Constructors
    /// <summary>
    /// Creates a new PiggLeafInfo object.
    /// </summary>
    public PiggLeafInfo() { }

    /// <summary>
    /// Creates a new PiggLeafInfo object.
    /// </summary>
    /// <param name="Base">Base PiggFile object that contains the file
    /// described by this info object.</param>
    public PiggLeafInfo(PiggFile Base) {
      this.File = Base;
    }

    /// <summary>
    /// Creates a new PiggLeafInfo object.
    /// </summary>
    /// <param name="Base">Base PiggFile object that contains the file
    /// described by this info object.</param>
    /// <param name="Entry">PiggFileDirectoryEntry object that contains
    /// descriptive information for this info object.</param>
    internal PiggLeafInfo(PiggFile Base, PiggFileDirectoryEntry Entry) {
      this.File = Base;
      this.UncompressedSize = Entry.UncompressedSize;
      this.CompressedSize = Entry.CompressedSize;
      this.Offset = Entry.Offset;
      this.SecondaryIndex = Entry.SecondaryIndex;
      this.Timestamp = Entry.Timestamp;
      this.MD5 = Entry.MD5;
    }
    #endregion

    #region Member Functions
    /// <summary>
    /// The full path of the Pigg file that references this info object.
    /// </summary>
    /// <returns>The full path of the Pigg file that references this info
    /// object.</returns>
    public override string ToString() {
      return this.File.FullPath;
    }

    /// <summary>
    /// Extracts an embedded file using the information contained within the
    /// PiggLeafInfo structure.
    /// </summary>
    /// <param name="output">Stream to which file information is saved.
    /// </param>
    /// <returns>The total number of bytes written to the stream.</returns>
    public int Extract(Stream output) {
      PiggStream p_stream = new PiggStream(this);
      int block_size = 0x1000;
      byte[] bytes = new byte[block_size];
      int total_bytes = 0;
      bool end_of_stream = false;
      while (end_of_stream == false) {
        int bytes_read = p_stream.Read(bytes, 0, block_size);
        total_bytes += bytes_read;
        if (bytes_read > 0) output.Write(bytes, 0, bytes_read);
        else end_of_stream = true;
      }
      return total_bytes;
    }
    #endregion

    #region Properties
    private PiggFile m_file;
    private int m_uncompressed_size;
    private int m_compressed_size;
    private long m_offset;
    private int m_secondary_index;
    private DateTime m_timestamp;
    private byte[] m_md5;

    /// <summary>
    /// PiggFile object to which this LeafInfo structure refers.
    /// </summary>
    public PiggFile File {
      get { return m_file; }
      set { m_file = value; }
    }
    /// <summary>
    /// Size of the uncompressed file.
    /// </summary>
    public int UncompressedSize {
      get { return m_uncompressed_size; }
      set { m_uncompressed_size = value; }
    }
    /// <summary>
    /// Size of the compressed file within the Pigg file.
    /// </summary>
    public int CompressedSize {
      get { return m_compressed_size; }
      set { m_compressed_size = value; }
    }
    /// <summary>
    /// Offset of the file within the Pigg file.
    /// </summary>
    public long Offset {
      get { return m_offset; }
      set { m_offset = value; }
    }
    /// <summary>
    /// Index of secondary data structure.
    /// </summary>
    public int SecondaryIndex {
      get { return m_secondary_index; }
      set { m_secondary_index = value; }
    }
    /// <summary>
    /// Date and time file was created.
    /// </summary>
    public DateTime Timestamp {
      get { return m_timestamp; }
      set { m_timestamp = value; }
    }
    /// <summary>
    /// MD5 checksum of file.
    /// </summary>
    public byte[] MD5 {
      get { return m_md5; }
      set { m_md5 = value; }
    }
    #endregion
  }

  /// <summary>
  /// Encapsulates the functionality of a pigg leaf node, which represents
  /// a stored and compressed file within a pig file.
  /// </summary>
  public class PiggLeaf {

    #region PiggLeaf Constructors

    /// <summary>
    /// Creates a default PiggLeaf object.  Properties must be set manually
    /// after creating the object.
    /// </summary>
    public PiggLeaf() {
      Initialize(null, null);
    }

    /// <summary>
    /// Creates a root PiggLeaf object with the specified name.
    /// </summary>
    /// <param name="Name">Name of the PiggLeaf object.</param>
    public PiggLeaf(string Name) {
      Initialize(Name, null);
    }

    /// <summary>
    /// Creates a PiggLeaf that is a subnode of a PiggNode object.
    /// </summary>
    /// <param name="Name">Name of the PiggLeaf object.</param>
    /// <param name="Parent">PiggNode that is the parent node of this PiggLeaf
    /// object.</param>
    public PiggLeaf(string Name, PiggNode Parent) {
      Initialize(Name, Parent);
    }

    /// <summary>
    /// Initializes a directory node with the given parameters.
    /// </summary>
    /// <param name="Name">Name of the directory node.</param>
    /// <param name="Parent">Parent node of the directory.  If the node is a
    /// root node, Parent returns null.</param>
    /// <remarks>By default, Initialize sets all members to some reasonable
    /// default value, such as zero.  For the DateTime timestamp, it is set
    /// to the minimum DateTime available (midnight Jan. 1, 1 AD).  Non-
    /// default values should be set after the Initialize function is called.
    /// </remarks>
    private void Initialize(string Name, PiggNode Parent) {
      this.Name = Name;
      this.Parent = Parent;
      Parent.AddLeaf(this);
      m_pigg_references = new List<PiggLeafInfo>();
    }

    #endregion

    #region PiggLeaf Member Functions

    /// <summary>
    /// Compares this instance with another object.
    /// </summary>
    /// <param name="OtherObject"></param>
    /// <returns></returns>
    public int CompareTo(object OtherObject) {
      if (OtherObject is PiggLeaf) {
        return this.Name.ToLower().CompareTo(OtherObject.ToString().ToLower());
      }
      else {
        throw new ArgumentException("OtherObject is not a PiggLeaf.");
      }
    }

    /// <summary>
    /// Return the name of the directory node.
    /// </summary>
    /// <returns>The name of the directory node.</returns>
    public override string ToString() {
      if (this.Name == null) { return ""; }
      else { return this.Name; }
    }

    /// <summary>
    /// Add a leaf as a child to the PiggNode tree.
    /// </summary>
    /// <param name="Parent">Root of the PiggNode tree.</param>
    /// <param name="FullPath">Full path name of the PiggLeaf object.</param>
    /// <returns></returns>
    public static PiggLeaf AddLeaf(PiggNode Parent, string FullPath) {
      string path = System.IO.Path.GetDirectoryName(FullPath);
      string leaf = System.IO.Path.GetFileName(FullPath);

      // If a null parent was passed in, create a default root node.
      if (Parent == null) Parent = new PiggNode();
      return new PiggLeaf(leaf, Parent.AddPath(path));
    }

    #endregion

    #region PiggLeaf Properties

    private string m_name;
    private PiggNode m_parent;
    private List<PiggLeafInfo> m_pigg_references;

    /// <summary>
    /// The name of the file
    /// </summary>
    public string Name {
      get { return m_name; }
      set { m_name = value; }
    }

    /// <summary>
    /// Parent directory of this Pigg File.
    /// </summary>
    public PiggNode Parent {
      get { return m_parent; }
      set { m_parent = value; }
    }

    /// <summary>
    /// An array of Pigg leaf info objects that contain information required
    /// to access this leaf within the physical Pigg file.  Normally, this
    /// should always contain a single file, but in theory, it could contain
    /// multiple references if the file is contained in more than one Pigg
    /// file.
    /// </summary>
    public List<PiggLeafInfo> PiggReferences {
      get { return m_pigg_references; }
    }

    /// <summary>
    /// Indicates whether or not 
    /// </summary>
    public bool IsTexture {
      get {
        return Path.GetExtension(this.Name).ToLower() == ".texture";
      }
    }

    #endregion
  }
}
