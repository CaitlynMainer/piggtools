using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ParagonForge.PiggInterface;

namespace ParagonForge.PiggViewerPro {
  public partial class dlgExtractOptions : Form {
    public dlgExtractOptions() {
      InitializeComponent();
    }

    private void dlgExtractOptions_Load(object sender, EventArgs e) {
      lstExtractImages.SelectedIndex = 0;
    }

    public void ClearWarning() {
      panWarning.Visible = false;
      this.Height = 183;
    }

    public void SetWarningSize(long Size) {
      string capacity = Utility.FriendlyBytes(Size);
      capacity += string.Format(" ({0:0,0} bytes)", Size);
      lblCapacity.Text = capacity;
      this.Height = panWarning.Size.Height + 183;
      panWarning.Visible = true;
    }

    public TextureExtractType TextureType {
      get {
        switch (lstExtractImages.SelectedIndex) {
          case 0: return TextureExtractType.Texture;
          case 1: return TextureExtractType.Original;
          case 2: return TextureExtractType.Png;
          case 3: return TextureExtractType.Jpeg;
          case 4: return TextureExtractType.Bmp;
          default:
            throw new NotSupportedException("Invalid texture type.");
        }
      }
      set {
        switch (value) {
          case TextureExtractType.Texture:
            lstExtractImages.SelectedIndex = 0;
            break;
          case TextureExtractType.Original:
            lstExtractImages.SelectedIndex = 1;
            break;
          case TextureExtractType.Png:
            lstExtractImages.SelectedIndex = 2;
            break;
          case TextureExtractType.Jpeg:
            lstExtractImages.SelectedIndex = 3;
            break;
          case TextureExtractType.Bmp:
            lstExtractImages.SelectedIndex = 4;
            break;
          default:
            throw new NotSupportedException("Invalid texture type.");
        }
      }
    }

    public bool TexturesOnly {
      get { return optTexturesOnly.Checked; }
      set { optTexturesOnly.Checked = value; }
    }
  }
}