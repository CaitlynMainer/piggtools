namespace ParagonForge.PiggViewerPro
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
          this.toolContainer = new System.Windows.Forms.ToolStripContainer();
          this.staStatus = new System.Windows.Forms.StatusStrip();
          this.splHSplitter = new System.Windows.Forms.SplitContainer();
          this.splVSplitter = new System.Windows.Forms.SplitContainer();
          this.tvwFilelistNormal = new System.Windows.Forms.TreeView();
          this.imlImages = new System.Windows.Forms.ImageList(this.components);
          this.panPicturePanel = new System.Windows.Forms.Panel();
          this.imgViewer = new ParagonTools.Controls.ImageViewer();
          this.picImage = new System.Windows.Forms.PictureBox();
          this.txtProperties = new System.Windows.Forms.TextBox();
          this.mnuMain = new System.Windows.Forms.MenuStrip();
          this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuFileOpendirectory = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuFileOpenfile = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.exportAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.exportAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
          this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuView = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuViewReferences = new System.Windows.Forms.ToolStripMenuItem();
          this.fitImagesToWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.fitToWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.sizeWindowToImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.fixedZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
          this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
          this.previewOriginalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuViewProperties = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuToolsOptions = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuTest = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuTest1 = new System.Windows.Forms.ToolStripMenuItem();
          this.toolStrip1 = new System.Windows.Forms.ToolStrip();
          this.dlgDirectory = new System.Windows.Forms.FolderBrowserDialog();
          this.mnuContext = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.mnuContextNodeExtract = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuContextNodeSeparator = new System.Windows.Forms.ToolStripSeparator();
          this.mnuContextNodeExtractOriginal = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuContextNodeExtractPng = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuContextNodeExtractJpg = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuContextNodeExtractBmp = new System.Windows.Forms.ToolStripMenuItem();
          this.mnuContextNodeExtractWav = new System.Windows.Forms.ToolStripMenuItem();
          this.dlgSave = new System.Windows.Forms.SaveFileDialog();
          this.toolContainer.BottomToolStripPanel.SuspendLayout();
          this.toolContainer.ContentPanel.SuspendLayout();
          this.toolContainer.TopToolStripPanel.SuspendLayout();
          this.toolContainer.SuspendLayout();
          this.splHSplitter.Panel1.SuspendLayout();
          this.splHSplitter.Panel2.SuspendLayout();
          this.splHSplitter.SuspendLayout();
          this.splVSplitter.Panel1.SuspendLayout();
          this.splVSplitter.Panel2.SuspendLayout();
          this.splVSplitter.SuspendLayout();
          this.panPicturePanel.SuspendLayout();
          ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
          this.mnuMain.SuspendLayout();
          this.mnuContext.SuspendLayout();
          this.SuspendLayout();
          // 
          // toolContainer
          // 
          // 
          // toolContainer.BottomToolStripPanel
          // 
          this.toolContainer.BottomToolStripPanel.Controls.Add(this.staStatus);
          // 
          // toolContainer.ContentPanel
          // 
          this.toolContainer.ContentPanel.Controls.Add(this.splHSplitter);
          this.toolContainer.ContentPanel.Size = new System.Drawing.Size(640, 409);
          this.toolContainer.Dock = System.Windows.Forms.DockStyle.Fill;
          this.toolContainer.Location = new System.Drawing.Point(0, 0);
          this.toolContainer.Name = "toolContainer";
          this.toolContainer.Size = new System.Drawing.Size(640, 480);
          this.toolContainer.TabIndex = 0;
          this.toolContainer.Text = "toolStripContainer1";
          // 
          // toolContainer.TopToolStripPanel
          // 
          this.toolContainer.TopToolStripPanel.Controls.Add(this.mnuMain);
          this.toolContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
          // 
          // staStatus
          // 
          this.staStatus.Dock = System.Windows.Forms.DockStyle.None;
          this.staStatus.Location = new System.Drawing.Point(0, 0);
          this.staStatus.Name = "staStatus";
          this.staStatus.Size = new System.Drawing.Size(640, 22);
          this.staStatus.TabIndex = 0;
          // 
          // splHSplitter
          // 
          this.splHSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splHSplitter.Location = new System.Drawing.Point(0, 0);
          this.splHSplitter.Name = "splHSplitter";
          this.splHSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
          // 
          // splHSplitter.Panel1
          // 
          this.splHSplitter.Panel1.Controls.Add(this.splVSplitter);
          // 
          // splHSplitter.Panel2
          // 
          this.splHSplitter.Panel2.Controls.Add(this.txtProperties);
          this.splHSplitter.Size = new System.Drawing.Size(640, 409);
          this.splHSplitter.SplitterDistance = 319;
          this.splHSplitter.TabIndex = 0;
          // 
          // splVSplitter
          // 
          this.splVSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
          this.splVSplitter.Location = new System.Drawing.Point(0, 0);
          this.splVSplitter.Name = "splVSplitter";
          // 
          // splVSplitter.Panel1
          // 
          this.splVSplitter.Panel1.Controls.Add(this.tvwFilelistNormal);
          // 
          // splVSplitter.Panel2
          // 
          this.splVSplitter.Panel2.Controls.Add(this.panPicturePanel);
          this.splVSplitter.Size = new System.Drawing.Size(640, 319);
          this.splVSplitter.SplitterDistance = 211;
          this.splVSplitter.TabIndex = 0;
          // 
          // tvwFilelistNormal
          // 
          this.tvwFilelistNormal.Dock = System.Windows.Forms.DockStyle.Fill;
          this.tvwFilelistNormal.ImageIndex = 0;
          this.tvwFilelistNormal.ImageList = this.imlImages;
          this.tvwFilelistNormal.Location = new System.Drawing.Point(0, 0);
          this.tvwFilelistNormal.Name = "tvwFilelistNormal";
          this.tvwFilelistNormal.SelectedImageIndex = 0;
          this.tvwFilelistNormal.ShowNodeToolTips = true;
          this.tvwFilelistNormal.Size = new System.Drawing.Size(211, 319);
          this.tvwFilelistNormal.TabIndex = 1;
          this.tvwFilelistNormal.AfterCollapse += new System.Windows.Forms.TreeViewEventHandler(this.tvwFilelistNormal_AfterCollapse);
          this.tvwFilelistNormal.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvwFilelistNormal_AfterSelect);
          this.tvwFilelistNormal.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvwFilelistNormal_NodeMouseClick);
          this.tvwFilelistNormal.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.tvwFilelistNormal_AfterExpand);
          // 
          // imlImages
          // 
          this.imlImages.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlImages.ImageStream")));
          this.imlImages.TransparentColor = System.Drawing.Color.Transparent;
          this.imlImages.Images.SetKeyName(0, "coh.png");
          this.imlImages.Images.SetKeyName(1, "folderclosed.png");
          this.imlImages.Images.SetKeyName(2, "folderopen.png");
          this.imlImages.Images.SetKeyName(3, "generic.png");
          this.imlImages.Images.SetKeyName(4, "image.png");
          this.imlImages.Images.SetKeyName(5, "binary.png");
          this.imlImages.Images.SetKeyName(6, "geometry.png");
          this.imlImages.Images.SetKeyName(7, "text.png");
          this.imlImages.Images.SetKeyName(8, "config.png");
          this.imlImages.Images.SetKeyName(9, "audio.png");
          // 
          // panPicturePanel
          // 
          this.panPicturePanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
          this.panPicturePanel.Controls.Add(this.imgViewer);
          this.panPicturePanel.Controls.Add(this.picImage);
          this.panPicturePanel.Dock = System.Windows.Forms.DockStyle.Fill;
          this.panPicturePanel.Location = new System.Drawing.Point(0, 0);
          this.panPicturePanel.Name = "panPicturePanel";
          this.panPicturePanel.Size = new System.Drawing.Size(425, 319);
          this.panPicturePanel.TabIndex = 0;
          // 
          // imgViewer
          // 
          this.imgViewer.AutoScroll = true;
          this.imgViewer.Dock = System.Windows.Forms.DockStyle.Fill;
          this.imgViewer.Location = new System.Drawing.Point(0, 0);
          this.imgViewer.Name = "imgViewer";
          this.imgViewer.Picture = null;
          this.imgViewer.Size = new System.Drawing.Size(421, 315);
          this.imgViewer.TabIndex = 2;
          this.imgViewer.ZoomFactor = 1;
          this.imgViewer.ZoomSetting = ParagonTools.Controls.ImageViewer.Zoom.Manual;
          // 
          // picImage
          // 
          this.picImage.Location = new System.Drawing.Point(66, 77);
          this.picImage.Name = "picImage";
          this.picImage.Size = new System.Drawing.Size(279, 246);
          this.picImage.TabIndex = 1;
          this.picImage.TabStop = false;
          // 
          // txtProperties
          // 
          this.txtProperties.Dock = System.Windows.Forms.DockStyle.Fill;
          this.txtProperties.Location = new System.Drawing.Point(0, 0);
          this.txtProperties.Multiline = true;
          this.txtProperties.Name = "txtProperties";
          this.txtProperties.ReadOnly = true;
          this.txtProperties.Size = new System.Drawing.Size(640, 86);
          this.txtProperties.TabIndex = 0;
          // 
          // mnuMain
          // 
          this.mnuMain.Dock = System.Windows.Forms.DockStyle.None;
          this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuView,
            this.mnuTools,
            this.mnuTest});
          this.mnuMain.Location = new System.Drawing.Point(0, 0);
          this.mnuMain.Name = "mnuMain";
          this.mnuMain.Size = new System.Drawing.Size(640, 24);
          this.mnuMain.TabIndex = 0;
          this.mnuMain.Text = "menuStrip1";
          // 
          // mnuFile
          // 
          this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileOpendirectory,
            this.mnuFileOpenfile,
            this.mnuSeparator1,
            this.exportToolStripMenuItem,
            this.exportAsToolStripMenuItem,
            this.exportAllToolStripMenuItem,
            this.toolStripSeparator1,
            this.mnuFileExit});
          this.mnuFile.Name = "mnuFile";
          this.mnuFile.Size = new System.Drawing.Size(35, 20);
          this.mnuFile.Text = "&File";
          // 
          // mnuFileOpendirectory
          // 
          this.mnuFileOpendirectory.Name = "mnuFileOpendirectory";
          this.mnuFileOpendirectory.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
          this.mnuFileOpendirectory.Size = new System.Drawing.Size(198, 22);
          this.mnuFileOpendirectory.Text = "&Open Directory";
          this.mnuFileOpendirectory.Click += new System.EventHandler(this.mnuFileOpendirectory_Click);
          // 
          // mnuFileOpenfile
          // 
          this.mnuFileOpenfile.Name = "mnuFileOpenfile";
          this.mnuFileOpenfile.Size = new System.Drawing.Size(198, 22);
          this.mnuFileOpenfile.Text = "Open &File";
          // 
          // mnuSeparator1
          // 
          this.mnuSeparator1.Name = "mnuSeparator1";
          this.mnuSeparator1.Size = new System.Drawing.Size(195, 6);
          // 
          // exportToolStripMenuItem
          // 
          this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
          this.exportToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
          this.exportToolStripMenuItem.Text = "&Export";
          // 
          // exportAsToolStripMenuItem
          // 
          this.exportAsToolStripMenuItem.Name = "exportAsToolStripMenuItem";
          this.exportAsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
          this.exportAsToolStripMenuItem.Text = "Export &As...";
          // 
          // exportAllToolStripMenuItem
          // 
          this.exportAllToolStripMenuItem.Name = "exportAllToolStripMenuItem";
          this.exportAllToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
          this.exportAllToolStripMenuItem.Text = "Export &All";
          // 
          // toolStripSeparator1
          // 
          this.toolStripSeparator1.Name = "toolStripSeparator1";
          this.toolStripSeparator1.Size = new System.Drawing.Size(195, 6);
          // 
          // mnuFileExit
          // 
          this.mnuFileExit.Name = "mnuFileExit";
          this.mnuFileExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
          this.mnuFileExit.Size = new System.Drawing.Size(198, 22);
          this.mnuFileExit.Text = "E&xit";
          this.mnuFileExit.Click += new System.EventHandler(this.mnuFileExit_Click);
          // 
          // mnuView
          // 
          this.mnuView.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuViewReferences,
            this.fitImagesToWindowToolStripMenuItem,
            this.previewOriginalToolStripMenuItem,
            this.mnuViewProperties});
          this.mnuView.Name = "mnuView";
          this.mnuView.Size = new System.Drawing.Size(41, 20);
          this.mnuView.Text = "&View";
          // 
          // mnuViewReferences
          // 
          this.mnuViewReferences.Name = "mnuViewReferences";
          this.mnuViewReferences.Size = new System.Drawing.Size(162, 22);
          this.mnuViewReferences.Text = "&References";
          // 
          // fitImagesToWindowToolStripMenuItem
          // 
          this.fitImagesToWindowToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fitToWindowToolStripMenuItem,
            this.sizeWindowToImageToolStripMenuItem,
            this.fixedZoomToolStripMenuItem});
          this.fitImagesToWindowToolStripMenuItem.Name = "fitImagesToWindowToolStripMenuItem";
          this.fitImagesToWindowToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
          this.fitImagesToWindowToolStripMenuItem.Text = "Image Size";
          // 
          // fitToWindowToolStripMenuItem
          // 
          this.fitToWindowToolStripMenuItem.Name = "fitToWindowToolStripMenuItem";
          this.fitToWindowToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
          this.fitToWindowToolStripMenuItem.Text = "Fit to Window";
          // 
          // sizeWindowToImageToolStripMenuItem
          // 
          this.sizeWindowToImageToolStripMenuItem.Name = "sizeWindowToImageToolStripMenuItem";
          this.sizeWindowToImageToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
          this.sizeWindowToImageToolStripMenuItem.Text = "Size Window to Image";
          // 
          // fixedZoomToolStripMenuItem
          // 
          this.fixedZoomToolStripMenuItem.Checked = true;
          this.fixedZoomToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
          this.fixedZoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6,
            this.toolStripMenuItem7,
            this.toolStripSeparator2,
            this.toolStripTextBox1});
          this.fixedZoomToolStripMenuItem.Name = "fixedZoomToolStripMenuItem";
          this.fixedZoomToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
          this.fixedZoomToolStripMenuItem.Text = "Fixed Zoom";
          // 
          // toolStripMenuItem3
          // 
          this.toolStripMenuItem3.Name = "toolStripMenuItem3";
          this.toolStripMenuItem3.Size = new System.Drawing.Size(160, 22);
          this.toolStripMenuItem3.Text = "50%";
          // 
          // toolStripMenuItem4
          // 
          this.toolStripMenuItem4.Name = "toolStripMenuItem4";
          this.toolStripMenuItem4.Size = new System.Drawing.Size(160, 22);
          this.toolStripMenuItem4.Text = "75%";
          // 
          // toolStripMenuItem5
          // 
          this.toolStripMenuItem5.Checked = true;
          this.toolStripMenuItem5.CheckState = System.Windows.Forms.CheckState.Checked;
          this.toolStripMenuItem5.Name = "toolStripMenuItem5";
          this.toolStripMenuItem5.Size = new System.Drawing.Size(160, 22);
          this.toolStripMenuItem5.Text = "100%";
          // 
          // toolStripMenuItem6
          // 
          this.toolStripMenuItem6.Name = "toolStripMenuItem6";
          this.toolStripMenuItem6.Size = new System.Drawing.Size(160, 22);
          this.toolStripMenuItem6.Text = "150%";
          // 
          // toolStripMenuItem7
          // 
          this.toolStripMenuItem7.Name = "toolStripMenuItem7";
          this.toolStripMenuItem7.Size = new System.Drawing.Size(160, 22);
          this.toolStripMenuItem7.Text = "200%";
          // 
          // toolStripSeparator2
          // 
          this.toolStripSeparator2.Name = "toolStripSeparator2";
          this.toolStripSeparator2.Size = new System.Drawing.Size(157, 6);
          // 
          // toolStripTextBox1
          // 
          this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
          this.toolStripTextBox1.Name = "toolStripTextBox1";
          this.toolStripTextBox1.Size = new System.Drawing.Size(100, 21);
          // 
          // previewOriginalToolStripMenuItem
          // 
          this.previewOriginalToolStripMenuItem.Checked = true;
          this.previewOriginalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
          this.previewOriginalToolStripMenuItem.Name = "previewOriginalToolStripMenuItem";
          this.previewOriginalToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
          this.previewOriginalToolStripMenuItem.Text = "Preview Original";
          // 
          // mnuViewProperties
          // 
          this.mnuViewProperties.Name = "mnuViewProperties";
          this.mnuViewProperties.Size = new System.Drawing.Size(162, 22);
          this.mnuViewProperties.Text = "Properties";
          this.mnuViewProperties.Click += new System.EventHandler(this.mnuViewProperties_Click);
          // 
          // mnuTools
          // 
          this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuToolsOptions});
          this.mnuTools.Name = "mnuTools";
          this.mnuTools.Size = new System.Drawing.Size(44, 20);
          this.mnuTools.Text = "&Tools";
          // 
          // mnuToolsOptions
          // 
          this.mnuToolsOptions.Name = "mnuToolsOptions";
          this.mnuToolsOptions.Size = new System.Drawing.Size(134, 22);
          this.mnuToolsOptions.Text = "&Options...";
          this.mnuToolsOptions.Click += new System.EventHandler(this.mnuToolsOptions_Click);
          // 
          // mnuTest
          // 
          this.mnuTest.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuTest1});
          this.mnuTest.Name = "mnuTest";
          this.mnuTest.Size = new System.Drawing.Size(40, 20);
          this.mnuTest.Text = "T&est";
          // 
          // mnuTest1
          // 
          this.mnuTest1.Name = "mnuTest1";
          this.mnuTest1.Size = new System.Drawing.Size(115, 22);
          this.mnuTest1.Text = "Test &1";
          this.mnuTest1.Click += new System.EventHandler(this.mnuTest1_Click_1);
          // 
          // toolStrip1
          // 
          this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
          this.toolStrip1.Location = new System.Drawing.Point(3, 24);
          this.toolStrip1.Name = "toolStrip1";
          this.toolStrip1.Size = new System.Drawing.Size(109, 25);
          this.toolStrip1.TabIndex = 1;
          // 
          // dlgDirectory
          // 
          this.dlgDirectory.ShowNewFolderButton = false;
          // 
          // mnuContext
          // 
          this.mnuContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuContextNodeExtract,
            this.mnuContextNodeSeparator,
            this.mnuContextNodeExtractOriginal,
            this.mnuContextNodeExtractPng,
            this.mnuContextNodeExtractJpg,
            this.mnuContextNodeExtractBmp,
            this.mnuContextNodeExtractWav});
          this.mnuContext.Name = "NodeMenu";
          this.mnuContext.Size = new System.Drawing.Size(219, 142);
          // 
          // mnuContextNodeExtract
          // 
          this.mnuContextNodeExtract.Name = "mnuContextNodeExtract";
          this.mnuContextNodeExtract.Size = new System.Drawing.Size(218, 22);
          this.mnuContextNodeExtract.Text = "&Extract File...";
          this.mnuContextNodeExtract.Click += new System.EventHandler(this.mnuNodeExtract_Click);
          // 
          // mnuContextNodeSeparator
          // 
          this.mnuContextNodeSeparator.Name = "mnuContextNodeSeparator";
          this.mnuContextNodeSeparator.Size = new System.Drawing.Size(215, 6);
          // 
          // mnuContextNodeExtractOriginal
          // 
          this.mnuContextNodeExtractOriginal.Name = "mnuContextNodeExtractOriginal";
          this.mnuContextNodeExtractOriginal.Size = new System.Drawing.Size(218, 22);
          this.mnuContextNodeExtractOriginal.Text = "Extract as &original format...";
          this.mnuContextNodeExtractOriginal.Click += new System.EventHandler(this.mnuContextNodeExtractOriginal_Click);
          // 
          // mnuContextNodeExtractPng
          // 
          this.mnuContextNodeExtractPng.Name = "mnuContextNodeExtractPng";
          this.mnuContextNodeExtractPng.Size = new System.Drawing.Size(218, 22);
          this.mnuContextNodeExtractPng.Text = "Extract as &PNG...";
          this.mnuContextNodeExtractPng.Click += new System.EventHandler(this.mnuContextNodeExtractPng_Click);
          // 
          // mnuContextNodeExtractJpg
          // 
          this.mnuContextNodeExtractJpg.Name = "mnuContextNodeExtractJpg";
          this.mnuContextNodeExtractJpg.Size = new System.Drawing.Size(218, 22);
          this.mnuContextNodeExtractJpg.Text = "Extract as &JPG...";
          // 
          // mnuContextNodeExtractBmp
          // 
          this.mnuContextNodeExtractBmp.Name = "mnuContextNodeExtractBmp";
          this.mnuContextNodeExtractBmp.Size = new System.Drawing.Size(218, 22);
          this.mnuContextNodeExtractBmp.Text = "Extract as &BMP...";
          // 
          // mnuContextNodeExtractWav
          // 
          this.mnuContextNodeExtractWav.Name = "mnuContextNodeExtractWav";
          this.mnuContextNodeExtractWav.Size = new System.Drawing.Size(218, 22);
          this.mnuContextNodeExtractWav.Text = "Extract as &WAV...";
          // 
          // MainForm
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = global::ParagonForge.PiggViewerPro.Properties.Settings.Default.MainSize;
          this.Controls.Add(this.toolContainer);
          this.DataBindings.Add(new System.Windows.Forms.Binding("ClientSize", global::ParagonForge.PiggViewerPro.Properties.Settings.Default, "MainSize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::ParagonForge.PiggViewerPro.Properties.Settings.Default, "MainLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.DataBindings.Add(new System.Windows.Forms.Binding("WindowState", global::ParagonForge.PiggViewerPro.Properties.Settings.Default, "MainWindowState", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.Location = global::ParagonForge.PiggViewerPro.Properties.Settings.Default.MainLocation;
          this.MainMenuStrip = this.mnuMain;
          this.Name = "MainForm";
          this.Text = "Pigg Viewer Pro";
          this.WindowState = global::ParagonForge.PiggViewerPro.Properties.Settings.Default.MainWindowState;
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_Closing);
          this.toolContainer.BottomToolStripPanel.ResumeLayout(false);
          this.toolContainer.BottomToolStripPanel.PerformLayout();
          this.toolContainer.ContentPanel.ResumeLayout(false);
          this.toolContainer.TopToolStripPanel.ResumeLayout(false);
          this.toolContainer.TopToolStripPanel.PerformLayout();
          this.toolContainer.ResumeLayout(false);
          this.toolContainer.PerformLayout();
          this.splHSplitter.Panel1.ResumeLayout(false);
          this.splHSplitter.Panel2.ResumeLayout(false);
          this.splHSplitter.Panel2.PerformLayout();
          this.splHSplitter.ResumeLayout(false);
          this.splVSplitter.Panel1.ResumeLayout(false);
          this.splVSplitter.Panel2.ResumeLayout(false);
          this.splVSplitter.ResumeLayout(false);
          this.panPicturePanel.ResumeLayout(false);
          ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
          this.mnuMain.ResumeLayout(false);
          this.mnuMain.PerformLayout();
          this.mnuContext.ResumeLayout(false);
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolContainer;
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpendirectory;
        private System.Windows.Forms.ToolStripMenuItem mnuFileOpenfile;
        private System.Windows.Forms.ToolStripSeparator mnuSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuView;
        private System.Windows.Forms.ToolStripMenuItem mnuViewReferences;
      private System.Windows.Forms.SplitContainer splHSplitter;
      private System.Windows.Forms.SplitContainer splVSplitter;
      private System.Windows.Forms.TreeView tvwFilelistNormal;
      private System.Windows.Forms.StatusStrip staStatus;
      private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem exportAllToolStripMenuItem;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
      private System.Windows.Forms.ToolStripMenuItem fitImagesToWindowToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem fitToWindowToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem sizeWindowToImageToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem mnuTools;
      private System.Windows.Forms.ToolStripMenuItem mnuToolsOptions;
      private System.Windows.Forms.ToolStripMenuItem exportAsToolStripMenuItem;
      private System.Windows.Forms.FolderBrowserDialog dlgDirectory;
      private System.Windows.Forms.ImageList imlImages;
      private System.Windows.Forms.ToolStripMenuItem fixedZoomToolStripMenuItem;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
      private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
      private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
      private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
      private System.Windows.Forms.Panel panPicturePanel;
      private System.Windows.Forms.PictureBox picImage;
      private System.Windows.Forms.ToolStrip toolStrip1;
      private System.Windows.Forms.ToolStripMenuItem previewOriginalToolStripMenuItem;
      private ParagonTools.Controls.ImageViewer imgViewer;
      private System.Windows.Forms.ContextMenuStrip mnuContext;
      private System.Windows.Forms.ToolStripMenuItem mnuContextNodeExtract;
      private System.Windows.Forms.TextBox txtProperties;
      private System.Windows.Forms.ToolStripMenuItem mnuViewProperties;
      private System.Windows.Forms.ToolStripSeparator mnuContextNodeSeparator;
      private System.Windows.Forms.ToolStripMenuItem mnuContextNodeExtractOriginal;
      private System.Windows.Forms.ToolStripMenuItem mnuContextNodeExtractPng;
      private System.Windows.Forms.ToolStripMenuItem mnuContextNodeExtractJpg;
      private System.Windows.Forms.ToolStripMenuItem mnuContextNodeExtractBmp;
      private System.Windows.Forms.ToolStripMenuItem mnuContextNodeExtractWav;
      private System.Windows.Forms.SaveFileDialog dlgSave;
      private System.Windows.Forms.ToolStripMenuItem mnuTest;
      private System.Windows.Forms.ToolStripMenuItem mnuTest1;
    }
}

