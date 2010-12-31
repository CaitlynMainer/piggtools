using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace ParagonTools.Controls {
  public partial class ImageViewer : UserControl {
    public enum Zoom {
      FitToWindow,
      Manual
    };

    public enum MouseState {
      None,
      LeftButton,
      RightButton,
      Unknown
    };

    #region Private Class Members
    private Image m_image;
    private bool m_image_loaded;
    private List<Zoom> m_zoom_setting_history;
    private List<double> m_zoom_factor_history;
    private Point m_scroll_base;
    private ToolTip m_tooltip;

    #endregion

    #region Constructors
    public ImageViewer() {
      InitializeComponent();
      Initialize();
    }
    #endregion

    #region Private Class Methods
    private bool Initialize() {
      m_image = null;
      m_image_loaded = false;
      m_zoom_setting_history = new List<Zoom>();
      m_zoom_factor_history = new List<Double>();
      m_scroll_base = new Point(0, 0);
      m_tooltip = new ToolTip();

      // Set default zoom settings: 100%
      ZoomSetting = Zoom.Manual;
      ZoomFactor = 1.0;

      return true;
    }

    private bool SizeImage() {
      if (!m_image_loaded) { return false; }

      // Calculate image size
      switch (m_zoom_setting_history[m_zoom_setting_history.Count - 1]) {
      case Zoom.Manual:
        double width = m_image.Width *
          m_zoom_factor_history[m_zoom_factor_history.Count - 1];
        double height = m_image.Height *
          m_zoom_factor_history[m_zoom_factor_history.Count - 1];
        picImage.SizeMode = PictureBoxSizeMode.StretchImage;
        picImage.Height = (int)Math.Round(height, 0);
        picImage.Width = (int)Math.Round(width, 0);
        break;
      case Zoom.FitToWindow:
        double aspect_ratio_image = m_image.Width / m_image.Height;
        double aspect_ratio_view = this.Width / this.Height;
        if (aspect_ratio_image > aspect_ratio_view) {
          picImage.Width = this.Width;
          picImage.Height = (int)Math.Round(this.Width / aspect_ratio_image);
        }
        else {
          picImage.Height = this.Height;
          picImage.Width = (int)Math.Round(this.Height * aspect_ratio_image);
        }
        picImage.SizeMode = PictureBoxSizeMode.StretchImage;
        break;
      }

      // Check to see if we need to center the image
      if (picImage.Width < this.Width) {
        picImage.Left = picImage.Left = (this.Width - picImage.Width) / 2;
      }
      else {
        picImage.Left = 0;
      }

      if (picImage.Height < this.Height) {
        picImage.Top = picImage.Top = (this.Height - picImage.Height) / 2;
      }
      else {
        picImage.Top = 0;
      }

      return true;
    }

    private bool SetImage() {
      picImage.Image = m_image;
      SizeImage();
      picImage.Visible = true;
      picImage.Cursor = Cursors.SizeAll;
      return true;
    }
    #endregion

    #region Public Class Methods
    //public void 
    #endregion

    #region Class Properties
    /// <summary>
    /// Image that will be displayed
    /// </summary>
    public Image Picture {
      get {
         return m_image_loaded ? m_image : null;
      }
      set {
        if (value != null) {
          m_image = new Bitmap(value);
          m_image_loaded = true;
          SetImage();
        }
        else {
          m_image = null;
          m_image_loaded = false;
        }
      }
    }

    /// <summary>
    /// Zoom setting of zoom factor
    /// </summary>
    public Zoom ZoomSetting {
      get {
        return m_zoom_setting_history[m_zoom_setting_history.Count - 1];
      }
      set {
        if (m_zoom_setting_history.Count == 0 ||
          m_zoom_setting_history[m_zoom_setting_history.Count - 1] != value) {
          m_zoom_setting_history.Add(value);
          SizeImage();
        }
      }
    }

    /// <summary>
    /// Zoom factor of image in control (1.0 = 100%)
    /// </summary>
    public double ZoomFactor {
      get {
        return m_zoom_factor_history[m_zoom_factor_history.Count - 1];
      }
      set {
        if (m_zoom_factor_history.Count == 0 ||
          m_zoom_factor_history[m_zoom_factor_history.Count - 1] != value) {
          m_zoom_factor_history.Add(value);
          SizeImage();
        }
      }
    }
    #endregion

    #region Class Events
    private void picImage_MouseDown(object sender, MouseEventArgs e) {
      if (e.Button == MouseButtons.Left) {
        picImage.Capture = true;
        m_scroll_base = e.Location;
      }
    }

    private void picImage_MouseUp(object sender, MouseEventArgs e) {
      if (e.Button == MouseButtons.Left) {
        picImage.Capture = false;
      }
    }

    private void picImage_MouseMove(object sender, MouseEventArgs e) {
      switch (e.Button) {
      case MouseButtons.Left:
        if (picImage.Capture == true) {
          Point scroll_offset = new Point(
            m_scroll_base.X - e.Location.X - this.AutoScrollPosition.X,
            m_scroll_base.Y - e.Location.Y - this.AutoScrollPosition.Y);
          this.AutoScrollPosition = scroll_offset;
        }
        break;
      }
    }

    private void picImage_MouseWheel(object sender, MouseEventArgs e) {
      MessageBox.Show("Mouse wheel moved!");
      if (e.Delta > 0) {
        double new_zoom = ZoomFactor + e.Delta * 0.2;
        if (new_zoom > 0.0 && new_zoom < 20.0) {
          ZoomFactor = new_zoom;
        }
      }
    }
    #endregion

    private void ImageViewer_Resize(object sender, EventArgs e) {
      SizeImage();
    }

  }
}