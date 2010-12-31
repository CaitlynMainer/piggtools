namespace ParagonForge.PiggViewerPro {
  partial class Options {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.lstOptionGroups = new System.Windows.Forms.ListBox();
      this.cmdOK = new System.Windows.Forms.Button();
      this.cmdApply = new System.Windows.Forms.Button();
      this.cmdCancel = new System.Windows.Forms.Button();
      this.panViewer = new System.Windows.Forms.Panel();
      this.panExtract = new System.Windows.Forms.Panel();
      this.grpExtract01 = new System.Windows.Forms.GroupBox();
      this.optAsk = new System.Windows.Forms.RadioButton();
      this.optDirectory = new System.Windows.Forms.RadioButton();
      this.txtExtractDirectory = new System.Windows.Forms.TextBox();
      this.cmdExtractBrowse = new System.Windows.Forms.Button();
      this.optExtractDirect = new System.Windows.Forms.RadioButton();
      this.optExtractNested = new System.Windows.Forms.RadioButton();
      this.radToWindow = new System.Windows.Forms.RadioButton();
      this.radToFullSize = new System.Windows.Forms.RadioButton();
      this.lblZoomImage = new System.Windows.Forms.Label();
      this.dlgBrowse = new System.Windows.Forms.FolderBrowserDialog();
      this.grpExtract2 = new System.Windows.Forms.GroupBox();
      this.grpExtract3 = new System.Windows.Forms.GroupBox();
      this.cboEnableShellExtension = new System.Windows.Forms.CheckBox();
      this.lblExtract1 = new System.Windows.Forms.Label();
      this.optExtractAsTexture = new System.Windows.Forms.RadioButton();
      this.optExtractAsOriginal = new System.Windows.Forms.RadioButton();
      this.optExtractAsFiletype = new System.Windows.Forms.RadioButton();
      this.lstExtractFiletypes = new System.Windows.Forms.ComboBox();
      this.cboSavePathInImage = new System.Windows.Forms.CheckBox();
      this.cboConvertDDS = new System.Windows.Forms.CheckBox();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.panViewer.SuspendLayout();
      this.panExtract.SuspendLayout();
      this.grpExtract01.SuspendLayout();
      this.grpExtract2.SuspendLayout();
      this.grpExtract3.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.lstOptionGroups);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.cmdOK);
      this.splitContainer1.Panel2.Controls.Add(this.cmdApply);
      this.splitContainer1.Panel2.Controls.Add(this.cmdCancel);
      this.splitContainer1.Panel2.Controls.Add(this.panViewer);
      this.splitContainer1.Size = new System.Drawing.Size(640, 480);
      this.splitContainer1.SplitterDistance = 166;
      this.splitContainer1.TabIndex = 0;
      // 
      // lstOptionGroups
      // 
      this.lstOptionGroups.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lstOptionGroups.FormattingEnabled = true;
      this.lstOptionGroups.IntegralHeight = false;
      this.lstOptionGroups.Items.AddRange(new object[] {
            "Viewer",
            "Extract"});
      this.lstOptionGroups.Location = new System.Drawing.Point(0, 0);
      this.lstOptionGroups.Name = "lstOptionGroups";
      this.lstOptionGroups.Size = new System.Drawing.Size(166, 480);
      this.lstOptionGroups.TabIndex = 0;
      // 
      // cmdOK
      // 
      this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.cmdOK.Location = new System.Drawing.Point(221, 454);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new System.Drawing.Size(75, 23);
      this.cmdOK.TabIndex = 5;
      this.cmdOK.Text = "OK";
      this.cmdOK.UseVisualStyleBackColor = true;
      this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
      // 
      // cmdApply
      // 
      this.cmdApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdApply.Location = new System.Drawing.Point(302, 454);
      this.cmdApply.Name = "cmdApply";
      this.cmdApply.Size = new System.Drawing.Size(75, 23);
      this.cmdApply.TabIndex = 4;
      this.cmdApply.Text = "Apply";
      this.cmdApply.UseVisualStyleBackColor = true;
      this.cmdApply.Click += new System.EventHandler(this.cmdApply_Click);
      // 
      // cmdCancel
      // 
      this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdCancel.Location = new System.Drawing.Point(392, 454);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(75, 23);
      this.cmdCancel.TabIndex = 3;
      this.cmdCancel.Text = "&Cancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
      // 
      // panViewer
      // 
      this.panViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panViewer.Controls.Add(this.panExtract);
      this.panViewer.Controls.Add(this.radToWindow);
      this.panViewer.Controls.Add(this.radToFullSize);
      this.panViewer.Controls.Add(this.lblZoomImage);
      this.panViewer.Location = new System.Drawing.Point(2, 3);
      this.panViewer.Name = "panViewer";
      this.panViewer.Size = new System.Drawing.Size(465, 445);
      this.panViewer.TabIndex = 2;
      // 
      // panExtract
      // 
      this.panExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panExtract.Controls.Add(this.grpExtract3);
      this.panExtract.Controls.Add(this.grpExtract2);
      this.panExtract.Controls.Add(this.grpExtract01);
      this.panExtract.Location = new System.Drawing.Point(0, 0);
      this.panExtract.Name = "panExtract";
      this.panExtract.Size = new System.Drawing.Size(467, 445);
      this.panExtract.TabIndex = 3;
      // 
      // grpExtract01
      // 
      this.grpExtract01.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.grpExtract01.Controls.Add(this.optAsk);
      this.grpExtract01.Controls.Add(this.optDirectory);
      this.grpExtract01.Controls.Add(this.txtExtractDirectory);
      this.grpExtract01.Controls.Add(this.cmdExtractBrowse);
      this.grpExtract01.Location = new System.Drawing.Point(0, 3);
      this.grpExtract01.Name = "grpExtract01";
      this.grpExtract01.Size = new System.Drawing.Size(466, 72);
      this.grpExtract01.TabIndex = 8;
      this.grpExtract01.TabStop = false;
      this.grpExtract01.Text = "Default extract directory";
      // 
      // optAsk
      // 
      this.optAsk.AutoSize = true;
      this.optAsk.Location = new System.Drawing.Point(6, 19);
      this.optAsk.Name = "optAsk";
      this.optAsk.Size = new System.Drawing.Size(92, 17);
      this.optAsk.TabIndex = 1;
      this.optAsk.TabStop = true;
      this.optAsk.Text = "Ask each time";
      this.optAsk.UseVisualStyleBackColor = true;
      // 
      // optDirectory
      // 
      this.optDirectory.AutoSize = true;
      this.optDirectory.Location = new System.Drawing.Point(6, 42);
      this.optDirectory.Name = "optDirectory";
      this.optDirectory.Size = new System.Drawing.Size(69, 17);
      this.optDirectory.TabIndex = 2;
      this.optDirectory.TabStop = true;
      this.optDirectory.Text = "Location:";
      this.optDirectory.UseVisualStyleBackColor = true;
      // 
      // txtExtractDirectory
      // 
      this.txtExtractDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtExtractDirectory.Location = new System.Drawing.Point(81, 41);
      this.txtExtractDirectory.Name = "txtExtractDirectory";
      this.txtExtractDirectory.Size = new System.Drawing.Size(347, 20);
      this.txtExtractDirectory.TabIndex = 3;
      this.txtExtractDirectory.TextChanged += new System.EventHandler(this.txtExtractDirectory_TextChanged);
      // 
      // cmdExtractBrowse
      // 
      this.cmdExtractBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdExtractBrowse.Location = new System.Drawing.Point(434, 41);
      this.cmdExtractBrowse.Name = "cmdExtractBrowse";
      this.cmdExtractBrowse.Size = new System.Drawing.Size(26, 20);
      this.cmdExtractBrowse.TabIndex = 4;
      this.cmdExtractBrowse.Text = "...";
      this.cmdExtractBrowse.UseVisualStyleBackColor = true;
      this.cmdExtractBrowse.Click += new System.EventHandler(this.cmdExtractBrowse_Click);
      // 
      // optExtractDirect
      // 
      this.optExtractDirect.AutoSize = true;
      this.optExtractDirect.Location = new System.Drawing.Point(6, 42);
      this.optExtractDirect.Name = "optExtractDirect";
      this.optExtractDirect.Size = new System.Drawing.Size(187, 17);
      this.optExtractDirect.TabIndex = 7;
      this.optExtractDirect.TabStop = true;
      this.optExtractDirect.Text = "Extract directly to chosen directory";
      this.optExtractDirect.UseVisualStyleBackColor = true;
      // 
      // optExtractNested
      // 
      this.optExtractNested.AutoSize = true;
      this.optExtractNested.Location = new System.Drawing.Point(6, 19);
      this.optExtractNested.Name = "optExtractNested";
      this.optExtractNested.Size = new System.Drawing.Size(172, 17);
      this.optExtractNested.TabIndex = 6;
      this.optExtractNested.TabStop = true;
      this.optExtractNested.Text = "Create entire directory structure";
      this.optExtractNested.UseVisualStyleBackColor = true;
      // 
      // radToWindow
      // 
      this.radToWindow.AutoSize = true;
      this.radToWindow.Location = new System.Drawing.Point(6, 50);
      this.radToWindow.Name = "radToWindow";
      this.radToWindow.Size = new System.Drawing.Size(77, 17);
      this.radToWindow.TabIndex = 2;
      this.radToWindow.TabStop = true;
      this.radToWindow.Text = "To window";
      this.radToWindow.UseVisualStyleBackColor = true;
      // 
      // radToFullSize
      // 
      this.radToFullSize.AutoSize = true;
      this.radToFullSize.Location = new System.Drawing.Point(6, 26);
      this.radToFullSize.Name = "radToFullSize";
      this.radToFullSize.Size = new System.Drawing.Size(75, 17);
      this.radToFullSize.TabIndex = 1;
      this.radToFullSize.TabStop = true;
      this.radToFullSize.Text = "To full size";
      this.radToFullSize.UseVisualStyleBackColor = true;
      // 
      // lblZoomImage
      // 
      this.lblZoomImage.AutoSize = true;
      this.lblZoomImage.Location = new System.Drawing.Point(3, 9);
      this.lblZoomImage.Name = "lblZoomImage";
      this.lblZoomImage.Size = new System.Drawing.Size(69, 13);
      this.lblZoomImage.TabIndex = 0;
      this.lblZoomImage.Text = "Zoom Image:";
      // 
      // grpExtract2
      // 
      this.grpExtract2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.grpExtract2.Controls.Add(this.optExtractNested);
      this.grpExtract2.Controls.Add(this.optExtractDirect);
      this.grpExtract2.Location = new System.Drawing.Point(0, 81);
      this.grpExtract2.Name = "grpExtract2";
      this.grpExtract2.Size = new System.Drawing.Size(466, 72);
      this.grpExtract2.TabIndex = 9;
      this.grpExtract2.TabStop = false;
      this.grpExtract2.Text = "Extract to behavior";
      // 
      // grpExtract3
      // 
      this.grpExtract3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.grpExtract3.Controls.Add(this.cboConvertDDS);
      this.grpExtract3.Controls.Add(this.cboSavePathInImage);
      this.grpExtract3.Controls.Add(this.lstExtractFiletypes);
      this.grpExtract3.Controls.Add(this.optExtractAsFiletype);
      this.grpExtract3.Controls.Add(this.optExtractAsOriginal);
      this.grpExtract3.Controls.Add(this.optExtractAsTexture);
      this.grpExtract3.Controls.Add(this.lblExtract1);
      this.grpExtract3.Controls.Add(this.cboEnableShellExtension);
      this.grpExtract3.Location = new System.Drawing.Point(0, 159);
      this.grpExtract3.Name = "grpExtract3";
      this.grpExtract3.Size = new System.Drawing.Size(465, 220);
      this.grpExtract3.TabIndex = 10;
      this.grpExtract3.TabStop = false;
      this.grpExtract3.Text = "Image handling";
      // 
      // cboEnableShellExtension
      // 
      this.cboEnableShellExtension.AutoSize = true;
      this.cboEnableShellExtension.Location = new System.Drawing.Point(6, 19);
      this.cboEnableShellExtension.Name = "cboEnableShellExtension";
      this.cboEnableShellExtension.Size = new System.Drawing.Size(169, 17);
      this.cboEnableShellExtension.TabIndex = 0;
      this.cboEnableShellExtension.Text = "Enable .texture shell extension";
      this.cboEnableShellExtension.UseVisualStyleBackColor = true;
      // 
      // lblExtract1
      // 
      this.lblExtract1.AutoSize = true;
      this.lblExtract1.Location = new System.Drawing.Point(6, 58);
      this.lblExtract1.Name = "lblExtract1";
      this.lblExtract1.Size = new System.Drawing.Size(129, 13);
      this.lblExtract1.TabIndex = 1;
      this.lblExtract1.Text = "Extract as image behavior";
      // 
      // optExtractAsTexture
      // 
      this.optExtractAsTexture.AutoSize = true;
      this.optExtractAsTexture.Location = new System.Drawing.Point(6, 75);
      this.optExtractAsTexture.Name = "optExtractAsTexture";
      this.optExtractAsTexture.Size = new System.Drawing.Size(244, 17);
      this.optExtractAsTexture.TabIndex = 2;
      this.optExtractAsTexture.TabStop = true;
      this.optExtractAsTexture.Text = "Do not extract images (leave in .texture format)";
      this.optExtractAsTexture.UseVisualStyleBackColor = true;
      // 
      // optExtractAsOriginal
      // 
      this.optExtractAsOriginal.AutoSize = true;
      this.optExtractAsOriginal.Location = new System.Drawing.Point(6, 99);
      this.optExtractAsOriginal.Name = "optExtractAsOriginal";
      this.optExtractAsOriginal.Size = new System.Drawing.Size(171, 17);
      this.optExtractAsOriginal.TabIndex = 3;
      this.optExtractAsOriginal.TabStop = true;
      this.optExtractAsOriginal.Text = "Extract as original image format";
      this.optExtractAsOriginal.UseVisualStyleBackColor = true;
      // 
      // optExtractAsFiletype
      // 
      this.optExtractAsFiletype.AutoSize = true;
      this.optExtractAsFiletype.Location = new System.Drawing.Point(6, 123);
      this.optExtractAsFiletype.Name = "optExtractAsFiletype";
      this.optExtractAsFiletype.Size = new System.Drawing.Size(124, 17);
      this.optExtractAsFiletype.TabIndex = 4;
      this.optExtractAsFiletype.TabStop = true;
      this.optExtractAsFiletype.Text = "Extract all images as:";
      this.optExtractAsFiletype.UseVisualStyleBackColor = true;
      // 
      // lstExtractFiletypes
      // 
      this.lstExtractFiletypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lstExtractFiletypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.lstExtractFiletypes.FormattingEnabled = true;
      this.lstExtractFiletypes.Items.AddRange(new object[] {
            "Portable Network Graphics (.png)",
            "JPEG compressed (.jpg)",
            "Windows Bitmap (.bmp)",
            "DirectDraw Surface (.dds)"});
      this.lstExtractFiletypes.Location = new System.Drawing.Point(136, 122);
      this.lstExtractFiletypes.Name = "lstExtractFiletypes";
      this.lstExtractFiletypes.Size = new System.Drawing.Size(324, 21);
      this.lstExtractFiletypes.TabIndex = 5;
      // 
      // cboSavePathInImage
      // 
      this.cboSavePathInImage.AutoSize = true;
      this.cboSavePathInImage.Location = new System.Drawing.Point(6, 197);
      this.cboSavePathInImage.Name = "cboSavePathInImage";
      this.cboSavePathInImage.Size = new System.Drawing.Size(201, 17);
      this.cboSavePathInImage.TabIndex = 6;
      this.cboSavePathInImage.Text = "Save source path in image if possible";
      this.cboSavePathInImage.UseVisualStyleBackColor = true;
      // 
      // cboConvertDDS
      // 
      this.cboConvertDDS.AutoSize = true;
      this.cboConvertDDS.Location = new System.Drawing.Point(6, 174);
      this.cboConvertDDS.Name = "cboConvertDDS";
      this.cboConvertDDS.Size = new System.Drawing.Size(148, 17);
      this.cboConvertDDS.TabIndex = 7;
      this.cboConvertDDS.Text = "Convert DDS files to PNG";
      this.cboConvertDDS.UseVisualStyleBackColor = true;
      // 
      // Options
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(640, 480);
      this.ControlBox = false;
      this.Controls.Add(this.splitContainer1);
      this.Name = "Options";
      this.Text = "Options";
      this.Load += new System.EventHandler(this.Options_Load);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.panViewer.ResumeLayout(false);
      this.panViewer.PerformLayout();
      this.panExtract.ResumeLayout(false);
      this.grpExtract01.ResumeLayout(false);
      this.grpExtract01.PerformLayout();
      this.grpExtract2.ResumeLayout(false);
      this.grpExtract2.PerformLayout();
      this.grpExtract3.ResumeLayout(false);
      this.grpExtract3.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.ListBox lstOptionGroups;
    private System.Windows.Forms.Panel panViewer;
    private System.Windows.Forms.RadioButton radToWindow;
    private System.Windows.Forms.RadioButton radToFullSize;
    private System.Windows.Forms.Label lblZoomImage;
    private System.Windows.Forms.Button cmdOK;
    private System.Windows.Forms.Button cmdApply;
    private System.Windows.Forms.Button cmdCancel;
    private System.Windows.Forms.Panel panExtract;
    private System.Windows.Forms.RadioButton optAsk;
    private System.Windows.Forms.Button cmdExtractBrowse;
    private System.Windows.Forms.TextBox txtExtractDirectory;
    private System.Windows.Forms.RadioButton optDirectory;
    private System.Windows.Forms.FolderBrowserDialog dlgBrowse;
    private System.Windows.Forms.RadioButton optExtractNested;
    private System.Windows.Forms.RadioButton optExtractDirect;
    private System.Windows.Forms.GroupBox grpExtract01;
    private System.Windows.Forms.GroupBox grpExtract2;
    private System.Windows.Forms.GroupBox grpExtract3;
    private System.Windows.Forms.ComboBox lstExtractFiletypes;
    private System.Windows.Forms.RadioButton optExtractAsFiletype;
    private System.Windows.Forms.RadioButton optExtractAsOriginal;
    private System.Windows.Forms.RadioButton optExtractAsTexture;
    private System.Windows.Forms.Label lblExtract1;
    private System.Windows.Forms.CheckBox cboEnableShellExtension;
    private System.Windows.Forms.CheckBox cboSavePathInImage;
    private System.Windows.Forms.CheckBox cboConvertDDS;
  }
}