using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using ParagonForge.PiggInterface;

namespace ParagonForge.PiggViewerPro
{
  public partial class MainForm : Form {
    #region MainForm Constructors

    public MainForm() {
      InitializeComponent();
      Initialize();
    }

    private void Initialize() {
      mnuViewReferences.Checked = true;
      splVSplitter.Panel2Collapsed = false;
      this.RootNode = new PiggNode();
      this.PiggFiles = new List<PiggFile>();
      this.Settings = new Properties.Settings();

      // Set the images on the context menu
      mnuContextNodeExtractBmp.Image = imlImages.Images[4];
      mnuContextNodeExtractJpg.Image = imlImages.Images[4];
      mnuContextNodeExtractPng.Image = imlImages.Images[4];
      mnuContextNodeExtractWav.Image = imlImages.Images[9];
    }

    protected override void OnLoad(EventArgs e) {
      SyncProperties();
      base.OnLoad(e);
    }

    #endregion

    #region MainForm Member Functions

    private void LoadFileList(string RootNodeName) {
      tvwFilelistNormal.Nodes.Clear();
      TreeNode root = new TreeNode(RootNodeName, 0, 0);
      root.Tag = new DirectoryNode(this.RootNode);
      tvwFilelistNormal.Nodes.Add(root);
      root.Expand();
      LoadNode(root);
    }

    private void SyncProperties() {
      if (Properties.Settings.Default.ViewProperties) {
        mnuViewProperties.Checked = true;
        txtProperties.Visible = true;
        splHSplitter.Panel2Collapsed = false;
      }
      else {
        mnuViewProperties.Checked = false;
        txtProperties.Visible = false;
        splHSplitter.Panel2Collapsed = true;
      }
    }

    private void LoadNode(TreeNode Subnode) {
      DirectoryNode d_node = (DirectoryNode)Subnode.Tag;
      if (d_node.Node == null || d_node.IsLoaded) { return; }
      foreach (PiggNode entry in d_node.Node.Subnodes) {
        TreeNode new_node = new TreeNode(entry.Name);
        new_node.Tag = new DirectoryNode(entry);
        new_node.ImageIndex = 1;
        new_node.SelectedImageIndex = 1;
        Subnode.Nodes.Add(new_node);
      }
      foreach (PiggLeaf entry in d_node.Node.Leafs) {
        TreeNode new_node = new TreeNode(entry.Name);
        new_node.Tag = new DirectoryNode(entry);
        int index = entry.Name.LastIndexOf('.');
        string extension = "";
        if (index >= 0) { extension = entry.Name.Substring(index); }
        switch (extension) {
          case ".texture":
          case ".jpg":
          case ".bmp":
          case ".png":
            new_node.ImageIndex = 4;
            new_node.SelectedImageIndex = 4;
            break;
          case ".bin":
            new_node.ImageIndex = 5;
            new_node.SelectedImageIndex = 5;
            break;
          case ".geo":
            new_node.ImageIndex = 6;
            new_node.SelectedImageIndex = 6;
            break;
          case ".txt":
            new_node.ImageIndex = 7;
            new_node.SelectedImageIndex = 7;
            break;
          case ".cfg":
            new_node.ImageIndex = 8;
            new_node.SelectedImageIndex = 8;
            break;
          case ".ogg":
          case ".mp3":
          case ".wav":
            new_node.ImageIndex = 9;
            new_node.SelectedImageIndex = 9;
            break;
          default:
            new_node.ImageIndex = 3;
            new_node.SelectedImageIndex = 3;
            break;
        }
        Subnode.Nodes.Add(new_node);
      }
      d_node.IsLoaded = true;
    }

    private long m_extract_leaf_total;
    private ToolStripProgressBar m_extract_progress;
    private ToolStripLabel m_extract_label;
    private void UpdateExtractProgress(PiggLeaf leaf) {
      m_extract_progress.Value++;
      m_extract_label.Text = string.Format("Extracting: {0}%", m_extract_progress.Value * 100 / m_extract_leaf_total);
      staStatus.Refresh();
    }

    #endregion

    #region MainForm Events

    private void mnuFileExit_Click(object sender, EventArgs e)
    {
      Application.Exit();
    }

    private void mnuFileOpendirectory_Click(object sender, EventArgs e) {
      string pigg_dir =
        Registry.GetValue(@"HKEY_CURRENT_USER\Software\Cryptic\Coh",
        "Installation Directory", "").ToString();
      if (pigg_dir != "") {
        pigg_dir += @"\piggs";
        if (Directory.Exists(pigg_dir)) {
          dlgDirectory.Description = "Select a directory that contains the " +
            "pigg files you wish to view.";
          dlgDirectory.ShowNewFolderButton = false;
          dlgDirectory.SelectedPath = pigg_dir;
        }
      }
      if (dlgDirectory.ShowDialog() == DialogResult.OK) {
        // Retrieve the pigg file names in the selected directory.
        pigg_dir = dlgDirectory.SelectedPath;
        if (!Directory.Exists(pigg_dir)) { return; }
        //this.RootNode.Name = pigg_dir;
        string[] pigg_files = Directory.GetFiles(pigg_dir, "*.pigg");

        // Add the status bar items.
        ToolStripProgressBar progress_bar = new ToolStripProgressBar();
        progress_bar.Maximum = pigg_files.Length;
        staStatus.Items.Add(progress_bar);
        ToolStripLabel label = new ToolStripLabel();
        staStatus.Items.Add(label);

        // Add each pigg file to the pigg file collection.
        for (int i = 0; i < pigg_files.Length; i++) {
          progress_bar.Value = i;
          if (File.Exists(pigg_files[i])) {
            label.Text = "Loading " + pigg_files[i];
            staStatus.Refresh();
            this.PiggFiles.Add(new PiggFile(pigg_files[i], this.RootNode));
          }
        }

        // Build the tree view and clean up.
        label.Text = "Building file tree...";
        LoadFileList(pigg_dir);
        staStatus.Items.Remove(label);
        staStatus.Items.Remove(progress_bar);
      }
    }

    private void tvwFilelistNormal_AfterExpand(object sender, TreeViewEventArgs e) {
      if (((DirectoryNode)e.Node.Tag).Node != null) {
        foreach (TreeNode t_node in e.Node.Nodes) {
          LoadNode(t_node);
        }
      }
    }

    private void tvwFilelistNormal_AfterCollapse(object sender, TreeViewEventArgs e) {
    }
    
    private void tvwFilelistNormal_AfterSelect(object sender, TreeViewEventArgs e) {
      DirectoryNode d_node = (DirectoryNode)e.Node.Tag;
      if (d_node.Leaf != null) {
        PiggLeafInfo leaf_info =
          d_node.Leaf.PiggReferences[d_node.Leaf.PiggReferences.Count - 1];
        int index = d_node.Leaf.Name.LastIndexOf('.');
        string extension = "";
        if (index >= 0) { extension = d_node.Leaf.Name.Substring(index); }
        switch (extension) {
          case ".texture":
            PiggTexture tex = new PiggTexture(leaf_info);
            imgViewer.Picture = tex.Image;
            picImage.Image = tex.Image;
            picImage.Height = tex.Image.Height;
            picImage.Width = tex.Image.Width;

            break;
        }

        // Fill in properties
        txtProperties.Clear();
        string properties = "";
        properties += string.Format("Source: {0}\r\n",
          leaf_info.File.FullPath);
        properties += string.Format("File size: {0}\r\n",
          Utility.FriendlyBytes(leaf_info.UncompressedSize));
        properties += string.Format("Timestamp: {0}\r\n",
          leaf_info.Timestamp);
        properties += string.Format("Filename: {0}\r\n",
          leaf_info.File.FullPath);
        properties += string.Format("File start: 0x{0:x8}\r\n",
          leaf_info.Offset);
        properties += string.Format("Compressend length: 0x{0:x8}\r\n",
          leaf_info.CompressedSize);
        properties += string.Format("Uncompressed length: 0x{0:x8}\r\n",
          leaf_info.UncompressedSize);
        txtProperties.Text = properties;
      }
      else {
        txtProperties.Clear();
        string properties = "";
        properties += string.Format("Full Path: {0}\r\n", d_node.Node.FullPath);
        properties += string.Format("Size of child leafs: {0:x16} ({1})\r\n",
          d_node.Node.Capacity, Utility.FriendlyBytes(d_node.Node.Capacity));
        properties += string.Format("Total Size: {0:x16} ({1})\r\n",
          d_node.Node.TotalCapacity, Utility.FriendlyBytes(d_node.Node.TotalCapacity));
        foreach (PiggFile reference in d_node.Node.PiggFileRefs) {
          properties += string.Format("Reference: {0}", reference.FullPath);
        }
        txtProperties.Text = properties;
      }
    }

    private void tvwFilelistNormal_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
      // Check for right-click
      if (e.Button == MouseButtons.Right) {
        tvwFilelistNormal.SelectedNode = e.Node;
        DirectoryNode d_node = (DirectoryNode)e.Node.Tag;
        if (d_node.Leaf != null) {
          FileInfo fi = new FileInfo(d_node.Leaf.Name);
          string ext = fi.Extension;
          mnuContextNodeSeparator.Visible = false;
          mnuContextNodeExtractOriginal.Visible = false;
          mnuContextNodeExtractBmp.Visible = false;
          mnuContextNodeExtractJpg.Visible = false;
          mnuContextNodeExtractPng.Visible = false;
          mnuContextNodeExtractWav.Visible = false;
          switch (ext.ToLower()) {
            case ".texture":
              int last = d_node.Leaf.PiggReferences.Count - 1;
              PiggLeafInfo info = d_node.Leaf.PiggReferences[last];
              PiggTexture tex = new PiggTexture(info);
              ext = Path.GetExtension(tex.Filename).ToLower();
              if (ext[0] == '.') ext = ext.Substring(1);

              mnuContextNodeExtract.Image = imlImages.Images[5];
              mnuContextNodeExtractOriginal.Image = imlImages.Images[4];
              mnuContextNodeExtractOriginal.Text =
                string.Format("Extract as original format ({0})...", ext);
              mnuContextNodeSeparator.Visible = true;
              mnuContextNodeExtractOriginal.Visible = true;
              mnuContextNodeExtractBmp.Visible = true;
              mnuContextNodeExtractJpg.Visible = true;
              mnuContextNodeExtractPng.Visible = true;
              break;
            case ".bmp":
            case ".jpg":
            case ".png":
              mnuContextNodeExtract.Image = imlImages.Images[4];
              break;
            case ".bin":
              mnuContextNodeExtract.Image = imlImages.Images[5];
              break;
            case ".geo":
              mnuContextNodeExtract.Image = imlImages.Images[6];
              break;
            case ".txt":
              mnuContextNodeExtract.Image = imlImages.Images[7];
              break;
            case ".cfg":
              mnuContextNodeExtract.Image = imlImages.Images[8];
              break;
            case ".ogg":
            case ".mp3":
            case ".wav":
              mnuContextNodeExtract.Image = imlImages.Images[9];
              //mnuContextNodeExtractOriginal.Image = imlImages.Images[9];
              //mnuContextNodeSeparator.Visible = true;
              //mnuContextNodeExtractOriginal.Visible = true;
              //mnuContextNodeExtractWav.Visible = true;
              break;
            default:
              mnuContextNodeExtract.Image = imlImages.Images[3];
              break;
          }
        }
        mnuContext.Show(tvwFilelistNormal, e.Location);
      }
    }

    private void mnuNodeExtract_Click(object sender, EventArgs e) {
      TreeNode n = tvwFilelistNormal.SelectedNode;
      if (n != null) {
        DirectoryNode d_node = (DirectoryNode)n.Tag;
        if(d_node.Leaf != null) {
          string filetype = Path.GetExtension(d_node.Leaf.Name).ToLower();
          string filter = "";
          filter += Utility.ExtToDescriptive(filetype) + " files " +
            "(*" + filetype + ")|*" + filetype;
          if (filter != "") filter += "|";
          filter += "All files (*.*)|*.*";
          dlgSave.Filter = filter;
          dlgSave.FilterIndex = 1;
          dlgSave.FileName = d_node.Leaf.Name;
          dlgSave.OverwritePrompt = true;

          DialogResult dlg_result = dlgSave.ShowDialog();
          if (dlg_result == DialogResult.OK) {
            FileStream fs = new FileStream(dlgSave.FileName, FileMode.Create,
              FileAccess.Write, FileShare.Write);
            int last = d_node.Leaf.PiggReferences.Count - 1;
            d_node.Leaf.PiggReferences[last].Extract(fs);
            fs.Close();
          }
        }
        else if (d_node.Node != null) {  // A directory was clicked
          dlgExtractOptions options = new dlgExtractOptions();
          if (d_node.Node.TotalCapacity > 0x10000000) {  // 200 MB
            options.SetWarningSize(d_node.Node.TotalCapacity);
          }
          if (options.ShowDialog() == DialogResult.OK) {
            dlgDirectory.Description = "Select a directory where files in " +
              "this directory are to be extracted.";
            dlgDirectory.RootFolder = Environment.SpecialFolder.MyDocuments;
            dlgDirectory.ShowNewFolderButton = true;
            if (dlgDirectory.ShowDialog(this) == DialogResult.OK) {
              // Set up the progress bar
              m_extract_progress = new ToolStripProgressBar();
              m_extract_progress.Maximum = (int)d_node.Node.TotalLeafCount;
              staStatus.Items.Add(m_extract_progress);
              m_extract_label = new ToolStripLabel();
              staStatus.Items.Add(m_extract_label);
              m_extract_leaf_total = d_node.Node.TotalLeafCount;

              d_node.Node.Extract(dlgDirectory.SelectedPath,
                options.TextureType, NodeExtractType.Relative,
                options.TexturesOnly, this.UpdateExtractProgress);

              staStatus.Items.Remove(m_extract_progress);
              staStatus.Items.Remove(m_extract_label);
            }
          }
        }
      }
    }

    private void mnuContextNodeExtractPng_Click(object sender, EventArgs e) {
      TreeNode n = tvwFilelistNormal.SelectedNode;
      if (n != null) {
        DirectoryNode d_node = (DirectoryNode)n.Tag;
        if (d_node.Leaf != null) {
          string filter = "PNG files (*.png)|*.png|";
          filter += "All files (*.*)|*.*";
          dlgSave.Filter = filter;
          dlgSave.FilterIndex = 1;
          dlgSave.FileName = Path.GetFileNameWithoutExtension(d_node.Leaf.Name) + ".png";
          dlgSave.OverwritePrompt = true;
          if (dlgSave.ShowDialog() == DialogResult.OK) {
            FileStream fs = new FileStream(dlgSave.FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            int last = d_node.Leaf.PiggReferences.Count - 1;
            PiggTexture tex = new PiggTexture(d_node.Leaf.PiggReferences[last]);
            tex.Extract(fs, TextureExtractType.Png);
            fs.Close();
          }
        }
      }
    }

    private void mnuContextNodeExtractOriginal_Click(object sender, EventArgs e) {
      TreeNode n = tvwFilelistNormal.SelectedNode;
      if (n != null) {
        DirectoryNode d_node = (DirectoryNode)n.Tag;
        if (d_node.Leaf != null) {
          PiggLeaf leaf = d_node.Leaf;
          PiggLeafInfo leaf_info = leaf.PiggReferences[leaf.PiggReferences.Count - 1];
          PiggTexture tex = new PiggTexture(leaf_info);
          string ext = Path.GetExtension(tex.Filename).ToLower();

          string filter = Utility.ExtToDescriptive(ext) +
            " files (*" + ext + ")|*" + ext + "|";
          filter += "All files (*.*)|*.*";
          dlgSave.Filter = filter;
          dlgSave.FilterIndex = 1;
          dlgSave.FileName = Path.GetFileName(tex.Filename);
          dlgSave.OverwritePrompt = true;
          if (dlgSave.ShowDialog() == DialogResult.OK) {
            FileStream fs = new FileStream(dlgSave.FileName, FileMode.Create, FileAccess.Write, FileShare.Write);
            tex.Extract(fs, TextureExtractType.Original);
            fs.Close();
          }
        }
      }
    }

    private void mnuToolsOptions_Click(object sender, EventArgs e) {
      Options dlg_options = new Options(m_settings);
      dlg_options.Show();
      if (dlg_options.DialogResult == DialogResult.OK) {
        m_settings = dlg_options.Settings;
      }
    }

    private void mnuViewProperties_Click(object sender, EventArgs e) {
      Properties.Settings.Default.ViewProperties = !Properties.Settings.Default.ViewProperties;
      SyncProperties();
    }

    private void MainForm_Closing(object sender, EventArgs e) {
      Properties.Settings.Default.Save();
    }

    #if DEBUG
    private void mnuTest1_Click(object sender, EventArgs e) {
    }

    private void mnuTest2_Click(object sender, EventArgs e) {
    }

    private void mnuTest3_Click(object sender, EventArgs e) {
    }
    #endif

    #endregion

    #region MainProperties

    private PiggNode m_root;
    private List<PiggFile> m_pigg_files;
    private Properties.Settings m_settings;

    public PiggNode RootNode {
      get { return m_root; }
      set { m_root = value; }
    }

    public List<PiggFile> PiggFiles {
      get { return m_pigg_files; }
      set { m_pigg_files = value; }
    }

    internal Properties.Settings Settings {
      get { return m_settings; }
      set { m_settings = value; }
    }

    #endregion

    private void mnuTest1_Click_1(object sender, EventArgs e) {
      string root = @"E:\Docs";
      string path1 = root + "/foo/monkey/bar";
      Directory.CreateDirectory(path1);
    }

  }

  public class DirectoryNode {

    #region DirectoryNode Constructors

    private void Initialize() {
      m_loaded = false;
      m_leaf = null;
      m_node = null;
    }

    public DirectoryNode() {
      Initialize();
    }

    public DirectoryNode(PiggLeaf Leaf) {
      Initialize();
      this.Leaf = Leaf;
    }

    public DirectoryNode(PiggNode Node) {
      Initialize();
      this.Node = Node;
    }

    #endregion

    #region DirectoryNode Properties

    bool m_loaded;
    PiggLeaf m_leaf;
    PiggNode m_node;

    public bool IsLoaded {
      get { return m_loaded; }
      set { m_loaded = value; }
    }

    public PiggLeaf Leaf {
      get { return m_leaf; }
      set {
        m_leaf = value;
        m_node = null;
      }
    }

    public PiggNode Node {
      get { return m_node; }
      set {
        m_leaf = null;
        m_node = value;
      }
    }

    #endregion
  }
}