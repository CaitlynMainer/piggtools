using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ParagonForge.PiggViewerPro.Properties;

namespace ParagonForge.PiggViewerPro {
  public partial class Options : Form {
    private Settings m_settings;

    public Options() {
      this.Settings = new Settings();
      InitializeComponent();
    }

    internal Options(Settings Settings) {
      this.Settings = Settings;
      InitializeComponent();
    }

    private void cmdCancel_Click(object sender, EventArgs e) {
      this.Close();
    }

    internal Settings Settings {
      get { return m_settings; }
      set { m_settings = value; }
    }

    private void Options_Load(object sender, EventArgs e) {
      if (m_settings.ExtractAsk) {
        optAsk.Checked = true;
      }
      else {
        optDirectory.Checked = true;
        txtExtractDirectory.Text = m_settings.ExtractDirectory;
      }
    }

    /// <summary>
    /// Applies all currently selected options to the master settings object
    /// </summary>
    private void ApplySettings() {
      if (optAsk.Checked) {
        m_settings.ExtractAsk = true;
      }
      else {
        m_settings.ExtractAsk = false;
        m_settings.ExtractDirectory = txtExtractDirectory.Text;
      }
      m_settings.Save();
    }

    private void txtExtractDirectory_TextChanged(object sender, EventArgs e) {
      optDirectory.Checked = true;
    }

    private void cmdExtractBrowse_Click(object sender, EventArgs e) {
      if (System.IO.Directory.Exists(txtExtractDirectory.Text)) {
        dlgBrowse.Description = @"Please select the path you wish to " +
          @"extract files to by default.";
        dlgBrowse.SelectedPath = txtExtractDirectory.Text;
      }
      else {
      }
      dlgBrowse.ShowDialog();
    }

    /// <summary>
    /// Fired when the Apply button is clicked
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cmdApply_Click(object sender, EventArgs e) {
      ApplySettings();
    }

    private void cmdOK_Click(object sender, EventArgs e) {
      ApplySettings();
      this.Close();
    }

  }

  internal class OptionPanels {
    private string m_name;
    private System.Windows.Forms.Panel m_panel;

    public OptionPanels() {
      this.Name = "";
      this.Panel = null;
    }

    public override string ToString() { return Name; }

    public string Name {
      get { return m_name; }
      set { m_name = value; }
    }

    public System.Windows.Forms.Panel Panel {
      get { return m_panel; }
      set { m_panel = value; }
    }

    public bool Visible {
      get {
        if (this.Panel == null) { return false; }
        return this.Panel.Visible;
      }
      set {
        if (this.Panel != null) { this.Panel.Visible = value; }
      }
    }

    public void Show() { this.Visible = true; }
    public void Hide() { this.Visible = false; }
  }
}