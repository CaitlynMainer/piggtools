namespace ParagonTools.Controls {
  partial class ImageViewer {
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.picImage = new System.Windows.Forms.PictureBox();
      ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
      this.SuspendLayout();
      // 
      // picImage
      // 
      this.picImage.Location = new System.Drawing.Point(0, 0);
      this.picImage.Name = "picImage";
      this.picImage.Size = new System.Drawing.Size(100, 50);
      this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
      this.picImage.TabIndex = 0;
      this.picImage.TabStop = false;
      this.picImage.Visible = false;
      this.picImage.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseWheel);
      this.picImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseMove);
      this.picImage.MouseDown += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseDown);
      this.picImage.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picImage_MouseUp);
      // 
      // ImageViewer
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.Controls.Add(this.picImage);
      this.Name = "ImageViewer";
      this.Size = new System.Drawing.Size(600, 600);
      this.Resize += new System.EventHandler(this.ImageViewer_Resize);
      ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.PictureBox picImage;
  }
}
