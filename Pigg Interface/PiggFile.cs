using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ParagonForge.PiggInterface {
  /// <summary>
  /// "Magic numbers" that, if not encountered, are a sign of a pigg file
  /// error.
  /// </summary>
  public enum PiggFileMarker {
    /// <summary>Marker to ensure file integrity.</summary>
    Header0 = 0x00000123,
    /// <summary>Unknown marker or flags.</summary>
    Header1 = 0x00020002,
    /// <summary>Unknown marker or flags.</summary>
    Header2 = 0x00300010,
    /// <summary>Marker to ensure file integrity.</summary>
    DirectoryEntry = 0x00003456,
    /// <summary>Marker to ensure file integrity.</summary>
    StringTable = 0x00006789,
    /// <summary>Marker to ensure file integrity.</summary>
    SecondaryTable = 0x00009ABC,
  };

  /// <summary>
  /// Encapulates the header at the beginning of all Pigg files.
  /// </summary>
  internal class PiggFileHeader {
    #region Constructors

    /// <summary>
    /// Creates a new PiggHeader object.
    /// </summary>
    public PiggFileHeader() {
      this.Marker = (int)PiggFileMarker.Header0;
      this.Unknown = new int[2] {
        (int)PiggFileMarker.Header1,
        (int)PiggFileMarker.Header2
      };
      this.DirectoryEntries = 0;
    }

    /// <summary>
    /// Creates a new PiggHeader object.
    /// </summary>
    /// <param name="Reader"><typeparamref name="BinaryReader"/> object from
    /// which to read the PiggHeader object.</param>
    public PiggFileHeader(BinaryReader Reader) {
      this.ReadHeader(Reader);
    }

    #endregion

    #region Member Functions

    /// <summary>
    /// Returns a <typeparamref name="String"/> that represents the
    /// current <typeparamref name="PiggFileHeader"/> object.
    /// </summary>
    /// <returns>A <typeparamref name="String"/> that represents the
    /// current PiggFileHeader object.</returns>
    public override string ToString() {
      string result = "[ ";
      result += string.Format("Size: {0:x} ({1:0,0}); ", this.Size, this.Size);
      result += string.Format("Marker: {0:x}; ", this.Marker);
      result += string.Format("Unknown 0: {0:x}; ", this.Unknown[0]);
      result += string.Format("Unknown 1: {0:x}; ", this.Unknown[1]);
      result += string.Format("Directory Entries: {0:x} ({1:0,0}) ",
        this.DirectoryEntries, this.DirectoryEntries);
      result += "]";
      return result;
    }

    /// <summary>
    /// Reads a Pigg file header.
    /// </summary>
    /// <param name="Reader"><typeparamref name="BinaryReader"/> object from
    /// which to read the PiggHeader object.</param>
    public void ReadHeader(BinaryReader Reader) {
      string err_msg = @"Invalid pigg file format at offset {0:x8}.";

      this.Unknown = new int[2]; // Initialize our unknown elements

      // Read and check the header marker.
      int marker = Reader.ReadInt32();
      if (marker != (int)PiggFileMarker.Header0) {
        throw new FormatException(string.Format(err_msg,
          Reader.BaseStream.Position - sizeof(Int32)));
      }
      this.Marker = marker;

      // Read the unknowns.  These might be more markers; they always seem
      // to be set to 0x00020002 and 0x00300010, respectively.  They might
      // also be some sort of flags that the original developer of the Pigg
      // file format intended for some other purpose, maybe a version number.
      // If they're not set to what we expect them to be, strictly speaking,
      // it's not an error.
      for (int i = 0; i < Unknown.Length; i++)
        this.Unknown[i] = Reader.ReadInt32();

      // Read the number of directory entries within this Pigg file.
      this.DirectoryEntries = Reader.ReadInt32();
    }

    #endregion

    #region Properties
    private int m_marker;
    private int[] m_unknown;
    private int m_directory_entries;

    /// <summary>
    /// Size of the physical PiggHeader structure within a Pigg file.
    /// </summary>
    public int Size {
      get { return 0x10; } // 16 bytes
    }
    /// <summary>
    /// Marker to ensure data file integrity.
    /// </summary>
    public int Marker {
      get { return m_marker; }
      protected set { m_marker = value; }
    }
    /// <summary>
    /// Unknown flags or markers.  Currently always 0x000200002 and 00300010.
    /// </summary>
    public int[] Unknown {
      get { return m_unknown; }
      protected set { m_unknown = value; }
    }
    /// <summary>
    /// Number of directory entries the Pigg file contains.
    /// </summary>
    public int DirectoryEntries {
      get { return this.m_directory_entries; }
      protected set { m_directory_entries = value; }
    }
    #endregion
  }

  /// <summary>
  /// Encapsulates the string table within a Pigg file.
  /// </summary>
  internal class PiggFileStringTable {
    #region Constructors
    public PiggFileStringTable() {
    }
    public PiggFileStringTable(BinaryReader Reader) {
      this.Read(Reader);
    }
    #endregion

    #region Member Functions
    /// <summary>
    /// Reads a Pigg file string table.
    /// </summary>
    /// <param name="Reader"><typeparamref name="BinaryReader"/> object from
    /// which to read the PiggHeader object.</param>
    public void Read(BinaryReader Reader) {
      // Scratch any existing string list we might have.
      if (this.Strings != null) { this.Strings.Clear(); }
      this.Strings = new List<string>();

      string err_msg = @"Invalid string table format within Pigg File " +
        "at offset {0:x8}.";

      // Read and check the header marker.
      int marker = Reader.ReadInt32();
      if (marker != (int)PiggFileMarker.StringTable) {
        throw new FormatException(string.Format(err_msg,
          Reader.BaseStream.Position - sizeof(Int32)));
      }
      this.Marker = marker;

      this.Count = Reader.ReadInt32();
      this.Size = Reader.ReadInt32();

      // Read the strings in the string table.  Strings are stored as a 32-bit
      // integer that gives the size of the string, followed by the string
      // itself, zero-terminated.
      int bytes_read = 0;
      for (int i = 0; i < this.Count; i++) {
        int string_size = Reader.ReadInt32();
        if (string_size > 0) {  // Sanity check.  Should always be > 0.
          byte[] string_bytes = Reader.ReadBytes(string_size - 1);
          this.Strings.Add(System.Text.Encoding.ASCII.GetString(string_bytes));
          // Eat the terminating zero.
          if (Reader.ReadByte() != 0x00) {
            throw new FormatException(string.Format(err_msg,
              Reader.BaseStream.Position - sizeof(Int32)));
          }
        }
        // We'll track the number of bytes read in case we want to do any
        // error checking against it in the future.
        bytes_read += string_size;
      }
    }
    #endregion

    #region Properties
    private int m_marker;
    private int m_count;
    private int m_size;
    private List<string> m_strings;

    /// <summary>
    /// Gets or sets a string from the string table by index.
    /// </summary>
    /// <param name="Index">Index of the string.</param>
    /// <returns>The string within the string table.</returns>
    public string this[int Index] {
      get { return m_strings[Index]; }
      set { m_strings[Index] = value; }
    }
    /// <summary>
    /// List of strings in string table.
    /// </summary>
    protected List<string> Strings {
      get { return m_strings; }
      set { m_strings = value; }
    }
    /// <summary>
    /// Marker to ensure data file integrity.
    /// </summary>
    public int Marker {
      get { return m_marker; }
      set { m_marker = value; }
    }
    /// <summary>
    /// Number of strings in string table.
    /// </summary>
    public int Count {
      get { return m_count; }
      set { m_count = value; }
    }
    /// <summary>
    /// Size of the physical PiggHeader structure within a Pigg file.
    /// </summary>
    public int Size {
      get { return m_size; }
      set { m_size = value; }
    }
    #endregion
  }

  /// <summary>
  /// Represents a pigg file directory entry object.  These store information
  /// about individual directory entries.
  /// </summary>
  internal class PiggFileDirectoryEntry {
    #region Constructors

    /// <summary>
    /// Create a directory entry object and read it from the pigg file.
    /// </summary>
    /// <param name="Reader">A binary reader which is used to read the
    /// directory entry object from the pigg file.</param>
    public PiggFileDirectoryEntry(BinaryReader Reader) {
      this.Read(Reader);
    }
    #endregion

    /// <summary>
    /// Reads a directory entry information structure.
    /// </summary>
    /// <param name="Reader"></param>
    public void Read(BinaryReader Reader) {
      string err_msg = @"Invalid directory entry format within Pigg File " +
        "at offset {0:x8}.";

      // Read and validate the first marker
      this.Marker = Reader.ReadInt32();
      if (this.Marker != (int)PiggFileMarker.DirectoryEntry) {
        throw new FormatException(string.Format(err_msg,
          Reader.BaseStream.Position - sizeof(Int32)));
      }

      this.StringIndex = Reader.ReadInt32();
      this.UncompressedSize = Reader.ReadInt32();
      this.TimestampEpoch = Reader.ReadInt32();
      this.Offset = Reader.ReadInt64();
      this.SecondaryIndex = Reader.ReadInt32();
      this.MD5 = Reader.ReadBytes(16);
      CompressedSize = Reader.ReadInt32();
    }

    # region Properties
    private int m_marker;
    private int m_string_index;
    private int m_uncompressed_size;
    private int m_compressed_size;
    private long m_offset;
    private int m_secondary_index;
    private int m_timestamp;
    private byte[] m_md5;

    /// <summary>
    /// A "magic number" used to validate that this is in fact a directory
    /// entry that is being read.
    /// </summary>
    public int Marker {
      get { return m_marker; }
      set { m_marker = value; }
    }
    /// <summary>
    /// Index of string within string table.
    /// </summary>
    public int StringIndex {
      get { return m_string_index; }
      set { m_string_index = value; }
    }
    /// <summary>
    /// Size of uncompressed file.
    /// </summary>
    public int UncompressedSize {
      get { return m_uncompressed_size; }
      set { m_uncompressed_size = value; }
    }
    /// <summary>
    /// Size of compressed file within Pigg file.
    /// </summary>
    public int CompressedSize {
      get { return m_compressed_size; }
      set { m_compressed_size = value; }
    }
    /// <summary>
    /// Offset of file within Pigg file.
    /// </summary>
    public long Offset {
      get { return m_offset; }
      set { m_offset = value; }
    }
    /// <summary>
    /// Index of secondary header within Pigg File.
    /// </summary>
    public int SecondaryIndex {
      get { return m_secondary_index; }
      set { m_secondary_index = value; }
    }
    /// <summary>
    /// Timestamp of file, measured in seconds since midnight January 1, 1970
    /// UTC.
    /// </summary>
    public int TimestampEpoch {
      get { return m_timestamp; }
      set { m_timestamp = value; }
    }
    /// <summary>
    /// Timestamp of file, as a <typeparamref name="DateTime"/> object.
    /// </summary>
    public DateTime Timestamp {
      get {
        long ticks = PiggUtilities.EpochToTicks(this.TimestampEpoch);
        return new DateTime(ticks, DateTimeKind.Utc);
      }
      set {
        long epoch =
          PiggUtilities.TicksToEpoch(value.ToUniversalTime().Ticks);
        if (epoch > int.MaxValue || epoch < int.MinValue)
          throw new OverflowException();
        this.TimestampEpoch = (int)epoch;
      }
    }
    /// <summary>
    /// MD5 checksum of file to verify integrity.
    /// </summary>
    public byte[] MD5 {
      get { return m_md5; }
      set { m_md5 = value; }
    }
    #endregion
  }

  /// <summary>
  /// Represents a pigg file secondary directory entry object.  These store
  /// information about images and don't apply to non-image files.
  /// </summary>
  internal class PiggFileSecondaryEntry {
    public int Size;
    public int ImageWidth;
    public int ImageHeight;
    public int[] Unknown;
    public string Extension;
    public string Filename;

    /// <summary>
    /// Create a new PiggFileSecondaryEntry object.
    /// </summary>
    public PiggFileSecondaryEntry() {
      Unknown = new int[4];
    }

    /// <summary>
    /// Reads a secondary directory entry information structure.
    /// </summary>
    /// <param name="Reader">Binary reader from which the information should
    /// be read.</param>
    public void Read(BinaryReader Reader) {
      // Dummy commands to disable warnings for now.
      Size++; ImageHeight++; ImageWidth++; Extension = ""; Filename = "";
    }
  }

  /// <summary>
  /// Encapsulates the functionality of a physical pigg file (i.e. a file
  /// with the .pigg extension that is a compressed directory of other files).
  /// </summary>
  public class PiggFile {
    string m_fullpath;
    PiggNode m_root_node;

    List<PiggFileDirectoryEntry> m_directory_entries;


    #region Constructors
    /// <summary>
    /// Create a new pigg file object.
    /// </summary>
    public PiggFile() {
      Initialize(null, null);
    }

    /// <summary>
    /// Create a new pigg file object.
    /// </summary>
    /// <param name="Filename">Name of the file used to initialize the pigg
    /// file object.</param>
    public PiggFile(string Filename) {
      Initialize(Filename, null);
    }

    /// <summary>
    /// Create a new pigg file object.
    /// </summary>
    /// <param name="Filename">Name of the file used to initialize the pigg
    /// file object.</param>
    /// <param name="RootNode">Root node to which parsed directory entries
    /// will be added.</param>
    public PiggFile(string Filename, PiggNode RootNode) {
      Initialize(Filename, RootNode);
    }

    /// <summary>
    /// Initialize a pigg file object.
    /// </summary>
    /// <param name="Filename">Name of file used to initialize pigg file
    /// object.</param>
    /// <param name="RootNode">Root node of the Pigg tree.</param>
    private void Initialize(string Filename, PiggNode RootNode) {
      // Assign root node
      if (RootNode == null) { this.RootNode = new PiggNode(); }
      else { this.RootNode = RootNode; }

      FileInfo info = new FileInfo(Filename);
      this.FullPath = info.FullName;
      ReadPiggFile();
    }
    #endregion

    #region Member Functions
    /// <summary>
    /// Reads and populates this object from a pigg data file.
    /// </summary>
    private void ReadPiggFile() {
      if (FullPath == null) { return; }

      FileStream fs = File.Open(FullPath, FileMode.Open, FileAccess.Read);
      BinaryReader reader = new BinaryReader(fs);

      // Read the Pigg file header
      this.Header = new PiggFileHeader(reader);

      // Read the Pigg file string table
      fs.Seek(this.Header.DirectoryEntries * 0x30, SeekOrigin.Current);
      long string_table_offset = fs.Position;
      this.StringTable = new PiggFileStringTable(reader);

      // --- Read the directory entry table -----------------------------------
      fs.Seek(this.Header.Size, SeekOrigin.Begin);
      string err_msg = @"Invalid directory entry format within Pigg File " +
        "at offset {0:x8}.";
      for (int i = 0; i < this.Header.DirectoryEntries; i++) {
        PiggFileDirectoryEntry entry = new PiggFileDirectoryEntry(reader);
        PiggLeaf leaf =
          PiggLeaf.AddLeaf(RootNode, this.StringTable[entry.StringIndex]);
        PiggLeafInfo leaf_info = new PiggLeafInfo(this, entry);
        leaf.Parent.AddCapacity(leaf_info.UncompressedSize);
        leaf.PiggReferences.Add(leaf_info);
      }

      // --- Read the secondary entry table -----------------------------------

      fs.Seek(string_table_offset + 0x0c + this.StringTable.Size, SeekOrigin.Begin);
      if (reader.ReadInt32() != (int)PiggFileMarker.SecondaryTable) {
        throw new FormatException(string.Format(err_msg,
          reader.BaseStream.Position - sizeof(Int32)));
      }
      int secondary_entry_count = reader.ReadInt32();
      int secondary_entry_size = reader.ReadInt32();



      fs.Close();
    }
    #endregion

    #region Properties
    private PiggFileHeader m_header;
    private PiggFileStringTable m_string_table;

    internal PiggFileHeader Header {
      get { return m_header; }
      set { m_header = value; }
    }

    internal PiggFileStringTable StringTable {
      get { return m_string_table; }
      set { m_string_table = value; }
    }

    /// <summary>
    /// The full path of the pigg file.
    /// </summary>
    public string FullPath {
      get { return m_fullpath; }
      set { m_fullpath = value; }
    }
    /// <summary>
    /// The list of DirectoryEntry objects contained within this pigg file.
    /// </summary>
    private List<PiggFileDirectoryEntry> DirectoryEntries {
      get { return m_directory_entries; }
      set { m_directory_entries = value; }
    }

    /// <summary>
    /// A FileInfo structure representing the full path of the pigg file.
    /// </summary>
    /// <remarks>This property can throw an application exception</remarks>
    public FileInfo PiggFileInfo {
      get {
        if (this.FullPath == null) {
          string err_msg = "Pigg file object has not been initialized.";
          throw new ApplicationException(err_msg);
        }
        if (!File.Exists(this.FullPath)) {
          string err_msg = "Invalid Pigg file: ";
          err_msg += this.FullPath + " does not exist or is inaccessible.";
          throw new ArgumentException(err_msg, FullPath);
        }
        else { return new FileInfo(this.FullPath); }
      }
    }

    /// <summary>
    /// Fetches a list of root nodes contained within the pigg file.
    /// </summary>
    public PiggNode RootNode {
      get { return m_root_node; }
      set { m_root_node = value; }
    }
    #endregion
  }

}
