using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Configuration;

namespace ParagonForge.PiggViewerPro
{
  public static class Utility
  {
    public static string ExtToDescriptive(string Extension) {
      switch (Extension.ToLower()) {
        case ".dds":
          return "DirectDraw Surface";
        case ".png":
          return "PNG";
        case ".txt":
          return "Text";
        case ".geo":
          return "Geometry";
        case ".texture":
          return "Texture";
        case ".gif":
          return "GIF";
        case ".jpg":
          return "JPEG";
        case ".ogg":
          return "Ogg audio";
        case ".mp3":
          return "Mp3 audio";
        default:
          return Extension[0] == '.' ? Extension.Substring(1) : Extension;
      }
    }

    /// <summary>
    /// Converts a number of bytes from a number into a more human-readable
    /// number with an abbreviation.
    /// </summary>
    /// <param name="RawSize">Number of bytes to convert</param>
    /// <returns>A string with human-readable number.</returns>
    /// <example>FriendlyBytes(16457807268) = "15.33 GB"</example>
    /// <remarks>This uses the computationally correct definition of kilobyte,
    /// megabyte, etc.  1 KB = 1024 bytes, not 1000.  Sorry, hard drive
    /// marketing goobers.  (But not really.)</remarks>
    public static string FriendlyBytes(long RawSize) {
      long order_of_magnitude = (long)Math.Log10(RawSize);
      if (order_of_magnitude < 3) { return RawSize.ToString("N") + " B"; }
      else if (order_of_magnitude < 6) { return (RawSize / Math.Pow(2, 10)).ToString("N2") + " KB"; }
      else if (order_of_magnitude < 9) { return ( RawSize / Math.Pow(2, 20) ).ToString("N2") + " MB"; }
      else if (order_of_magnitude < 12) { return ( RawSize / Math.Pow(2, 30) ).ToString("N2") + " GB"; }
      else { return ( RawSize / Math.Pow(2, 40) ).ToString("N2") + " TB"; }
    }

    /// <summary>
    /// Determine the live CoH installation directory based on its entry in
    /// the registry.
    /// </summary>
    /// <returns>A path name to the live City of Heroes installation.
    /// </returns>
    public static string CoHLiveDirectory() {
      Properties.Settings s = new Properties.Settings();
      string live_directory =
        (string)Microsoft.Win32.Registry.GetValue(s.LiveRegKey,
        "Installation Directory", "");
      if (live_directory != "") {
        if (Directory.Exists(live_directory)) { return live_directory; }
      }
      return "";
    }
  }
}
