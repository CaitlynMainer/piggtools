using System;
using System.Collections.Generic;
using System.Text;

namespace ParagonForge.PiggInterface {
  /// <summary>
  /// Indicates how to extract a texture.  TextureExtractTypes can be combined
  /// to generate multiple files of different types.  For example,
  /// TextureExtractType.Png | TextureExtractType.Texture will extract both a
  /// texture file and a corresponding PNG file.
  /// </summary>
  [FlagsAttribute]
  public enum TextureExtractType : uint {
    /// <summary>Extract texture file as a texture.</summary>
    Texture = 0x00000001,
    /// <summary>Extract texture file as original image type.</summary>
    Original = 0x00000002,
    /// <summary>Extract texture file as PNG image.</summary>
    Png = 0x00000004,
    /// <summary>Extract texture file as JPG image.</summary>
    Jpeg = 0x00000008,
    /// <summary>Extract texture file as BMP image.</summary>
    Bmp = 0x00000010,
    /// <summary>Extract texture file as TIFF image.</summary>
    Tiff = 0x00000020,
    /// <summary>Extract texture file as GIF image.</summary>
    Gif = 0x00000040,
  }

  /// <summary>
  /// Indicates whether or not to extract a node using the full path of the
  /// node from the root or just the node.
  /// </summary>
  public enum NodeExtractType {
    /// <summary>Recreate the full node path.</summary>
    FullPath,
    /// <summary>Only create the relative path.</summary>
    Relative,
  }
}
