using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ParagonForge.PiggInterface {
  #region Enumerations
  #endregion

  /// <summary>
  /// Encapsulates the functionality of a Pigg directory in the hierarchical
  /// naming structure of Pigg files.
  /// </summary>
  public class PiggNode {
    #region PiggNode Constructors

    /// <summary>
    /// Initializes a new PiggDirectory object with a blank name and no parent
    /// directory.
    /// </summary>
    /// <remarks>Creates a root directory, though the parent can be set later
    /// using the Parent property.</remarks>
    public PiggNode() {
      Initialize("", null, 0);
    }

    /// <summary>
    /// Creates and initializes a new PiggDirectory object with the given name
    /// and no parent directory.
    /// </summary>
    /// <remarks>Creates a root directory, though the parent can be set later
    /// using the Parent property.</remarks>
    /// <param name="Name">Name of the directory object.</param>
    public PiggNode(string Name) {
      Initialize(Name, null, 0);
    }

    /// <summary>
    /// Creates and initializes a new PiggDirectory object with the given name
    /// and parent directory.
    /// </summary>
    /// <param name="Name">Name of the directory object.</param>
    /// <param name="Parent">Parent directory of the directory object.</param>
    public PiggNode(string Name, PiggNode Parent) {
      Initialize(Name, Parent, Parent.Level + 1);
    }

    /// <summary>
    /// Initializes a new PiggDirectory object with the given name and parent
    /// directory.
    /// </summary>
    /// <param name="Name">Name of PiggNode object.</param>
    /// <param name="Parent">Parent of the PiggNode object.</param>
    /// <param name="Level">Depth of node in PiggNode tree.</param>
    private void Initialize(string Name, PiggNode Parent, int Level) {
      this.Name = Name;
      this.Parent = Parent;
      this.Level = Level;
      this.LeafMap = new Dictionary<string, PiggLeaf>();
      this.SubnodeMap = new Dictionary<string, PiggNode>();
      this.FilerefMap = new Dictionary<string, PiggFile>();
    }
    #endregion

    #region PiggNode Member Functions
    /// <summary>
    /// Provides a human-readable path of this PiggNode object.
    /// </summary>
    /// <returns>The full path of this PiggNode object.</returns>
    public override string ToString() {
      return this.FullPath + " (" + this.Level + ")";
    }

    /// <summary>
    /// Provides a human-readable tree graph of this node and all of its
    /// subnodes.
    /// </summary>
    /// <returns>The entire tree structure of this node.</returns>
    public string Dump() {
      string result = "";
      for (int i = 0; i < Level; i++) result += "  ";
      result += this.Name + "*";
      Array.ForEach<PiggNode>(this.Subnodes, delegate(PiggNode item) {
        result += "\r\n";
        result += item.Dump();
      });
      return result;
    }

    /// <summary>
    /// Adds a PiggLeaf object to the tree.
    /// </summary>
    /// <param name="Leaf">Leaf to add to the PiggNode tree.</param>
    /// <remarks>If you try to add two files with the same name, only the
    /// second one will be set and the first one tossed.  Name collisions
    /// might be an error, but I'm not sure, so for now, I'll give it the
    /// benefit of a doubt and assume that they're legal.</remarks>
    /// <returns>The PiggLeaf object added.</returns>
    public PiggLeaf AddLeaf(PiggLeaf Leaf) {
      Leaf.Parent = this;
      this.LeafMap[Leaf.Name] = Leaf;
      return this.LeafMap[Leaf.Name];
    }

    /// <summary>
    /// Adds a PiggLeaf object to the tree at the specified path.
    /// </summary>
    /// <param name="Leaf">Leaf to add to the PiggNode tree.</param>
    /// <param name="Path">Path to the PiggNode object to which the PiggLeaf
    /// should be added.</param>
    /// <returns>The PiggLeaf object added.</returns>
    public PiggLeaf AddLeaf(PiggLeaf Leaf, string Path) {
      PiggNode parent = this.AddPath(Path);

      return parent.AddLeaf(Leaf);
    }

    /// <summary>
    /// Adds a Pigg node as a subnode of this node.
    /// </summary>
    /// <param name="Node">Pigg node to add as a subnode to this node.  If a
    /// node already exists with the name of the node, the existing node is
    /// returned instead.</param>
    /// <returns>The Pigg node added.</returns>
    public PiggNode AddNode(PiggNode Node) {
      if (this.SubnodeMap.ContainsKey(Node.Name))
        return this.SubnodeMap[Node.Name];
      this.SubnodeMap[Node.Name] = Node;
      Node.Parent = this;
      return this.SubnodeMap[Node.Name];
    }

    /// <summary>
    /// Adds a Pigg node as a subnode of this node.
    /// </summary>
    /// <param name="Name">Name of the Pigg node to add as a subnode to this
    /// node.  If a node already exists with the name of the node, the
    /// existing node is returned.</param>
    /// <returns>The Pigg node added.</returns>
    public PiggNode AddNode(string Name) {
      if (this.SubnodeMap.ContainsKey(Name))
        return this.SubnodeMap[Name];
      this.SubnodeMap[Name] = new PiggNode(Name, this);
      return this.SubnodeMap[Name];
    }

    /// <summary>
    /// Creates a new subnode of this node.
    /// </summary>
    /// <param name="Name">The name of the new subnode.  If the subnode
    /// already exists, it is not overwritten.</param>
    /// <returns>The new subnode, or if the subnode already existed, the
    /// existing subnode.</returns>
    public PiggNode Subnode(string Name) {
      if (this.SubnodeMap.ContainsKey(Name)) {
        return this.SubnodeMap[Name];
      }
      else {
        PiggNode new_node = new PiggNode(Name, this);
        this.SubnodeMap.Add(Name, new_node);
        return new_node;
      }
    }

    /// <summary>
    /// Creates a PiggNode path structure, ensuring that all elements along
    /// the path exist.
    /// </summary>
    /// <param name="Path">Path structure to create.</param>
    /// <returns></returns>
    public PiggNode AddPath(string Path) {
      string path = System.IO.Path.GetDirectoryName(Path);
      string leaf = System.IO.Path.GetFileName(Path);

      PiggNode parent = path == "" ? this : this.AddPath(path);
      if (!parent.SubnodeMap.ContainsKey(leaf))
        parent.SubnodeMap[leaf] = new PiggNode(leaf, parent);
      return parent.SubnodeMap[leaf];
    }

    /// <summary>
    /// Adds to the amount of storage this node is holding.
    /// </summary>
    /// <param name="Bytes">Number of bytes to add to storage.</param>
    internal void AddCapacity(long Bytes) {
      this.Capacity += Bytes;
      PiggNode walker = this;
      while (walker != null) {
        walker.TotalCapacity += Bytes;
        walker.TotalLeafCount++;
        walker = walker.Parent;
      }
    }

    /// <summary>
    /// Extract a node and all of its subnodes and children.
    /// </summary>
    /// <param name="RootPath">Path to which to extract node.</param>
    /// <param name="TextureType">How textures are to be extracted.</param>
    /// <param name="NodeType">Whether to recreate the full root path.</param>
    /// <param name="OnlyTextures">Indicates whether only textures or to be
    /// extracted or all files.</param>
    /// <param name="Callback">Callback function to update the caller that a
    /// leaf is about to be extracted.</param>
    public void Extract(string RootPath, TextureExtractType TextureType,
      NodeExtractType NodeType, bool OnlyTextures, ExtractProgress Callback) {
      // Create the path to extract the nodes.
      string dir_path = RootPath;
      switch (NodeType) {
        case NodeExtractType.Relative:
          dir_path += Path.DirectorySeparatorChar + this.Name;
          break;
        case NodeExtractType.FullPath:
          dir_path += Path.DirectorySeparatorChar + this.FullPath;
          break;
        default:
          throw new NotSupportedException("NodeExtractType not supported.");
      }
      DirectoryInfo new_dir = Directory.CreateDirectory(dir_path);

      // Extract all child nodes
      foreach (PiggNode node in this.Subnodes) {
        node.Extract(dir_path, TextureType, NodeExtractType.Relative,
          OnlyTextures, Callback);
      }

      // Extract all files
      foreach (PiggLeaf leaf in this.Leafs) {
        if (Callback != null) Callback(leaf);
        int last = leaf.PiggReferences.Count - 1;
        PiggLeafInfo leaf_info = leaf.PiggReferences[last];

        if (Path.GetExtension(leaf.Name).ToLower() != ".texture" ||
          TextureType == TextureExtractType.Texture) {
          // Check to see if we only want textures.
          if (TextureType == TextureExtractType.Texture || !OnlyTextures) {
            // Extract the file as-is.
            FileStream fs = new FileStream(dir_path +
              Path.DirectorySeparatorChar + leaf.Name,
              FileMode.Create, FileAccess.Write, FileShare.Write);
            if (fs != null) {
              leaf_info.Extract(fs);
            }
            fs.Close();
          }
        }
        else {
          PiggTexture tex = new PiggTexture(leaf_info);
          string ext = "";
          switch (TextureType) {
            case TextureExtractType.Bmp:
              ext += ".bmp";
              break;
            case TextureExtractType.Gif:
              ext += ".gif";
              break;
            case TextureExtractType.Jpeg:
              ext += ".jpg";
              break;
            case TextureExtractType.Original:
              ext += Path.GetExtension(tex.Filename).ToLower();
              break;
            case TextureExtractType.Png:
              ext += ".png";
              break;
            case TextureExtractType.Tiff:
              ext += ".tif";
              break;
            default:
              throw new NotSupportedException();
          }
          FileStream fs = new FileStream(dir_path +
            Path.DirectorySeparatorChar +
            Path.GetFileNameWithoutExtension(leaf.Name) + ext,
            FileMode.Create, FileAccess.Write, FileShare.Write);
          tex.Extract(fs, TextureType);
          fs.Close();
        }
      }
    }

    #endregion

    #region PiggNode Properties

    /// <summary>
    /// Called during a node extraction to let the caller know which leaf is
    /// being updated.
    /// </summary>
    /// <param name="Target">The leaf that is about to be extracted.</param>
    public delegate void ExtractProgress(PiggLeaf Target);

    private string m_name;
    private PiggNode m_parent;
    private Dictionary<string, PiggLeaf> m_leafs;
    private Dictionary<string, PiggNode> m_nodes;
    private Dictionary<string, PiggFile> m_piggfile_refs;
    private int m_level;
    private long m_capacity;
    private long m_total_capacity;
    private long m_total_leaf_count;

    /// <summary>
    /// The name of the pigg directory
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
    /// Level of node within tree structure.
    /// </summary>
    public int Level {
      get { return m_level; }
      protected set { m_level = value; }
    }
    /// <summary>
    /// Returns true if this node is a root node, false if it is a subnode.
    /// </summary>
    public bool IsRoot {
      get { return this.Parent == null ? true : false; }
    }
    /// <summary>
    /// Root PiggNode for this PiggNode tree structure.
    /// </summary>
    public PiggNode Root {
      get {
        PiggNode result = this;
        while (result.Parent != null) result = result.Parent;
        return result;
      }
    }
    /// <summary>
    /// Full path of the node, using the directory separator character
    /// (/ on Unix/Max OS machines, \ on Windows) as separators.
    /// </summary>
    public string FullPath {
      get {
        string result = "";
        PiggNode node = this;
        while (node != null) {
          if (node.Name != "") {
            result = (result == "") ?
              node.Name : (node.Name + Path.DirectorySeparatorChar + result);
          }
          node = node.Parent;
        }
        return result;
      }
    }
    /// <summary>
    /// An array of pigg files contained in this directory node.
    /// </summary>
    public PiggLeaf[] Leafs {
      get {
        PiggLeaf[] results = new PiggLeaf[m_leafs.Count];
        m_leafs.Values.CopyTo(results, 0);
        return results;
      }
    }
    /// <summary>
    /// The back-end dictionary of leaf items.
    /// </summary>
    protected Dictionary<string, PiggLeaf> LeafMap {
      get { return m_leafs; }
      set { m_leafs = value; }
    }
    /// <summary>
    /// An array of pigg subnodes contained in this directory node.
    /// </summary>
    public PiggNode[] Subnodes {
      get {
        PiggNode[] results = new PiggNode[this.SubnodeMap.Count];
        this.SubnodeMap.Values.CopyTo(results, 0);
        return results;
      }
    }
    /// <summary>
    /// The back-end dictionary of subnode items.
    /// </summary>
    protected Dictionary<string, PiggNode> SubnodeMap {
      get { return m_nodes; }
      set { m_nodes = value; }
    }
    /// <summary>
    /// Array of strings of all subnode elements.
    /// </summary>
    public string[] NodeNames {
      get {
        PiggNode[] subnodes = this.Subnodes;
        string[] results = new string[subnodes.Length];
        int i = 0;
        Array.ForEach<PiggNode>(subnodes, delegate(PiggNode item) {
          results[i++] = item.Name;
        });
        return results;
      }
    }
    /// <summary>
    /// An array of pigg file references contained in this directory node.
    /// </summary>
    public PiggFile[] PiggFileRefs {
      get {
        PiggFile[] results = new PiggFile[this.FilerefMap.Count];
        this.FilerefMap.Values.CopyTo(results, 0);
        return results;
      }
    }
    /// <summary>
    /// The back-end dictionary of PiggFile ref items.
    /// </summary>
    protected Dictionary<string, PiggFile> FilerefMap {
      get { return m_piggfile_refs; }
      set { m_piggfile_refs = value; }
    }

    /// <summary>
    /// Number of bytes used by this node's immediate children.
    /// </summary>
    public long Capacity {
      get { return m_capacity; }
      internal set { m_capacity = value; }
    }

    /// <summary>
    /// Number of bytes used by all of this node's descendants.
    /// </summary>
    public long TotalCapacity {
      get { return m_total_capacity; }
      internal set { m_total_capacity = value; }
    }

    /// <summary>
    /// Number of leafs in all of this node's descendants.
    /// </summary>
    public long TotalLeafCount {
      get { return m_total_leaf_count; }
      internal set { m_total_leaf_count = value; }
    }

    #endregion
  }
}
