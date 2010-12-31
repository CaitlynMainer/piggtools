namespace ParagonForge.PiggViewerPro {
  partial class dlgExtractOptions {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(dlgExtractOptions));
      this.cmdCancel = new System.Windows.Forms.Button();
      this.cmdOK = new System.Windows.Forms.Button();
      this.panExtractControls = new System.Windows.Forms.Panel();
      this.optSuppressOptions = new System.Windows.Forms.CheckBox();
      this.optSave = new System.Windows.Forms.CheckBox();
      this.optTexturesOnly = new System.Windows.Forms.CheckBox();
      this.lstExtractImages = new System.Windows.Forms.ComboBox();
      this.lblExtractImages = new System.Windows.Forms.Label();
      this.panWarning = new System.Windows.Forms.Panel();
      this.optSuppressWarning = new System.Windows.Forms.CheckBox();
      this.lblCapacity = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.lblWarning = new System.Windows.Forms.Label();
      this.panExtractControls.SuspendLayout();
      this.panWarning.SuspendLayout();
      this.SuspendLayout();
      // 
      // cmdCancel
      // 
      this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cmdCancel.Location = new System.Drawing.Point(312, 117);
      this.cmdCancel.Name = "cmdCancel";
      this.cmdCancel.Size = new System.Drawing.Size(75, 23);
      this.cmdCancel.TabIndex = 2;
      this.cmdCancel.Text = "Cancel";
      this.cmdCancel.UseVisualStyleBackColor = true;
      // 
      // cmdOK
      // 
      this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.cmdOK.Location = new System.Drawing.Point(231, 117);
      this.cmdOK.Name = "cmdOK";
      this.cmdOK.Size = new System.Drawing.Size(75, 23);
      this.cmdOK.TabIndex = 3;
      this.cmdOK.Text = "OK";
      this.cmdOK.UseVisualStyleBackColor = true;
      // 
      // panExtractControls
      // 
      this.panExtractControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.panExtractControls.Controls.Add(this.optSuppressOptions);
      this.panExtractControls.Controls.Add(this.optSave);
      this.panExtractControls.Controls.Add(this.optTexturesOnly);
      this.panExtractControls.Controls.Add(this.lstExtractImages);
      this.panExtractControls.Controls.Add(this.lblExtractImages);
      this.panExtractControls.Location = new System.Drawing.Point(0, 0);
      this.panExtractControls.Name = "panExtractControls";
      this.panExtractControls.Size = new System.Drawing.Size(400, 111);
      this.panExtractControls.TabIndex = 8;
      // 
      // optSuppressOptions
      // 
      this.optSuppressOptions.AutoSize = true;
      this.optSuppressOptions.Enabled = false;
      this.optSuppressOptions.Location = new System.Drawing.Point(15, 88);
      this.optSuppressOptions.Name = "optSuppressOptions";
      this.optSuppressOptions.Size = new System.Drawing.Size(107, 17);
      this.optSuppressOptions.TabIndex = 8;
      this.optSuppressOptions.Text = "Do not &ask again";
      this.optSuppressOptions.UseVisualStyleBackColor = true;
      // 
      // optSave
      // 
      this.optSave.AutoSize = true;
      this.optSave.Enabled = false;
      this.optSave.Location = new System.Drawing.Point(15, 64);
      this.optSave.Name = "optSave";
      this.optSave.Size = new System.Drawing.Size(139, 17);
      this.optSave.TabIndex = 7;
      this.optSave.Text = "Save settings as &default";
      this.optSave.UseVisualStyleBackColor = true;
      // 
      // optTexturesOnly
      // 
      this.optTexturesOnly.AutoSize = true;
      this.optTexturesOnly.Location = new System.Drawing.Point(15, 40);
      this.optTexturesOnly.Name = "optTexturesOnly";
      this.optTexturesOnly.Size = new System.Drawing.Size(124, 17);
      this.optTexturesOnly.TabIndex = 6;
      this.optTexturesOnly.Text = "&Skip non-texture files";
      this.optTexturesOnly.UseVisualStyleBackColor = true;
      // 
      // lstExtractImages
      // 
      this.lstExtractImages.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.lstExtractImages.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.lstExtractImages.FormattingEnabled = true;
      this.lstExtractImages.Items.AddRange(new object[] {
            "Texture files (.texture)",
            "Original format (.png, .jpg, ...)",
            "PNG files (.png)",
            "JPG files (.jpg)",
            "BMP files (.bmp)"});
      this.lstExtractImages.Location = new System.Drawing.Point(131, 13);
      this.lstExtractImages.Name = "lstExtractImages";
      this.lstExtractImages.Size = new System.Drawing.Size(257, 21);
      this.lstExtractImages.TabIndex = 2;
      // 
      // lblExtractImages
      // 
      this.lblExtractImages.AutoSize = true;
      this.lblExtractImages.Location = new System.Drawing.Point(12, 16);
      this.lblExtractImages.Name = "lblExtractImages";
      this.lblExtractImages.Size = new System.Drawing.Size(113, 13);
      this.lblExtractImages.TabIndex = 1;
      this.lblExtractImages.Text = "Extract texture files as:";
      // 
      // panWarning
      // 
      this.panWarning.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panWarning.Controls.Add(this.optSuppressWarning);
      this.panWarning.Controls.Add(this.lblCapacity);
      this.panWarning.Controls.Add(this.label1);
      this.panWarning.Controls.Add(this.lblWarning);
      this.panWarning.Location = new System.Drawing.Point(0, 0);
      this.panWarning.Name = "panWarning";
      this.panWarning.Size = new System.Drawing.Size(400, 109);
      this.panWarning.TabIndex = 9;
      this.panWarning.Visible = false;
      // 
      // optSuppressWarning
      // 
      this.optSuppressWarning.AutoSize = true;
      this.optSuppressWarning.Enabled = false;
      this.optSuppressWarning.Location = new System.Drawing.Point(7, 84);
      this.optSuppressWarning.Name = "optSuppressWarning";
      this.optSuppressWarning.Size = new System.Drawing.Size(174, 17);
      this.optSuppressWarning.TabIndex = 3;
      this.optSuppressWarning.Text = "Do not show this warning again";
      this.optSuppressWarning.UseVisualStyleBackColor = true;
      // 
      // lblCapacity
      // 
      this.lblCapacity.AutoSize = true;
      this.lblCapacity.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblCapacity.Location = new System.Drawing.Point(4, 65);
      this.lblCapacity.Name = "lblCapacity";
      this.lblCapacity.Size = new System.Drawing.Size(99, 15);
      this.lblCapacity.TabIndex = 2;
      this.lblCapacity.Text = "1.21 Jigabytes";
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(4, 24);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(392, 41);
      this.label1.TabIndex = 1;
      this.label1.Text = resources.GetString("label1.Text");
      // 
      // lblWarning
      // 
      this.lblWarning.AutoSize = true;
      this.lblWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.lblWarning.ForeColor = System.Drawing.Color.Red;
      this.lblWarning.Location = new System.Drawing.Point(4, 4);
      this.lblWarning.Name = "lblWarning";
      this.lblWarning.Size = new System.Drawing.Size(112, 16);
      this.lblWarning.TabIndex = 0;
      this.lblWarning.Text = "PLEASE NOTE";
      // 
      // dlgExtractOptions
      // 
      this.AcceptButton = this.cmdOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cmdCancel;
      this.ClientSize = new System.Drawing.Size(400, 152);
      this.ControlBox = false;
      this.Controls.Add(this.panWarning);
      this.Controls.Add(this.panExtractControls);
      this.Controls.Add(this.cmdOK);
      this.Controls.Add(this.cmdCancel);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "dlgExtractOptions";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "Extract Options";
      this.Load += new System.EventHandler(this.dlgExtractOptions_Load);
      this.panExtractControls.ResumeLayout(false);
      this.panExtractControls.PerformLayout();
      this.panWarning.ResumeLayout(false);
      this.panWarning.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Button cmdCancel;
    private System.Windows.Forms.Button cmdOK;
    private System.Windows.Forms.Panel panExtractControls;
    private System.Windows.Forms.ComboBox lstExtractImages;
    private System.Windows.Forms.Label lblExtractImages;
    private System.Windows.Forms.CheckBox optSuppressOptions;
    private System.Windows.Forms.CheckBox optSave;
    private System.Windows.Forms.CheckBox optTexturesOnly;
    private System.Windows.Forms.Panel panWarning;
    private System.Windows.Forms.Label lblWarning;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.CheckBox optSuppressWarning;
    private System.Windows.Forms.Label lblCapacity;
  }
}