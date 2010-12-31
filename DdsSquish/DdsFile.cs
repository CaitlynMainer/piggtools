using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

// If we want to do the alignment as per the (broken) DDS documentation, then we
// uncomment this define.. 
//#define  APPLY_PITCH_ALIGNMENT

namespace ParagonForge.Squish {
  public enum DdsFileFormat {
    /// <summary>Use DXT1 compression</summary>
    DDS_FORMAT_DXT1,
    /// <summary>Use DXT3 compression</summary>
    DDS_FORMAT_DXT3,
    /// <summary>Use DXT5 compression</summary>
    DDS_FORMAT_DXT5,
    DDS_FORMAT_A8R8G8B8,
    DDS_FORMAT_X8R8G8B8,
    DDS_FORMAT_A8B8G8R8,
    DDS_FORMAT_X8B8G8R8,
    DDS_FORMAT_A1R5G5B5,
    DDS_FORMAT_A4R4G4B4,
    DDS_FORMAT_R8G8B8,
    DDS_FORMAT_R5G6B5,

    DDS_FORMAT_INVALID,
  };

  public enum PixelFormatFlags {
    DDS_FOURCC = 0x00000004,
    DDS_RGB = 0x00000040,
    DDS_RGBA = 0x00000041,
  };

  /// <summary>
  /// DDS header flags.  See Microsoft's DDS documentation for full
  /// documentation of DDS file format.
  /// http://msdn.microsoft.com/en-us/library/bb943982(VS.85).aspx
  /// </summary>
  public enum HeaderFlags {
    /// <summary>
    /// Include DDSD_CAPS, DDSD_HEIGHT, DDSD_WIDTH, and DDSD_PIXELFORMAT
    /// fields.  These fields are required in all DDS data structures.
    /// </summary>
    DDS_HEADER_FLAGS_TEXTURE = 0x00001007,
    /// <summary>
    /// Required when pitch is provided for an uncompressed texture.
    /// </summary>
    DDS_HEADER_FLAGS_PITCH = 0x00000008,
    /// <summary>Required in a mipmapped texture.</summary>
    DDS_HEADER_FLAGS_MIPMAP = 0x00020000,
    /// <summary>Required in a depth texture.</summary>
    DDS_HEADER_FLAGS_DEPTH = 0x00800000,
    /// <summary>
    /// Required when pitch is provided for a compressed texture.
    /// </summary>
    DDS_HEADER_FLAGS_LINEARSIZE = 0x00080000,
  };

  public enum SurfaceFlags {
    DDS_SURFACE_FLAGS_TEXTURE = 0x00001000,  // DDSCAPS_TEXTURE
    DDS_SURFACE_FLAGS_MIPMAP = 0x00400008,  // DDSCAPS_COMPLEX | DDSCAPS_MIPMAP
    DDS_SURFACE_FLAGS_CUBEMAP = 0x00000008,  // DDSCAPS_COMPLEX
  };

  public enum CubemapFlags {
    DDS_CUBEMAP_POSITIVEX = 0x00000600, // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEX
    DDS_CUBEMAP_NEGATIVEX = 0x00000a00, // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEX
    DDS_CUBEMAP_POSITIVEY = 0x00001200, // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEY
    DDS_CUBEMAP_NEGATIVEY = 0x00002200, // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEY
    DDS_CUBEMAP_POSITIVEZ = 0x00004200, // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_POSITIVEZ
    DDS_CUBEMAP_NEGATIVEZ = 0x00008200, // DDSCAPS2_CUBEMAP | DDSCAPS2_CUBEMAP_NEGATIVEZ

    DDS_CUBEMAP_ALLFACES = (DDS_CUBEMAP_POSITIVEX | DDS_CUBEMAP_NEGATIVEX |
      DDS_CUBEMAP_POSITIVEY | DDS_CUBEMAP_NEGATIVEY | DDS_CUBEMAP_POSITIVEZ |
      DDS_CUBEMAP_NEGATIVEZ)
  };

  public enum VolumeFlags {
    DDS_FLAGS_VOLUME = 0x00200000,  // DDSCAPS2_VOLUME
  };

  public class DdsPixelFormat {
    private UInt32 m_flags;
    private UInt32 m_fourCC;
    private UInt32 m_rgbBitCount;
    private UInt32 m_bitMaskA;
    private UInt32 m_bitMaskR;
    private UInt32 m_bitMaskG;
    private UInt32 m_bitMaskB;

    public void Initialize(DdsFileFormat fileFormat) {
      switch (fileFormat) {
        case DdsFileFormat.DDS_FORMAT_DXT1:
        case DdsFileFormat.DDS_FORMAT_DXT3:
        case DdsFileFormat.DDS_FORMAT_DXT5:
          this.Flags = (UInt32)PixelFormatFlags.DDS_FOURCC;
          this.RgbBitCount = 0x00000000;
          this.BitMaskA = 0x00000000;
          this.BitMaskR = 0x00000000;
          this.BitMaskG = 0x00000000;
          this.BitMaskB = 0x00000000;
          if (fileFormat == DdsFileFormat.DDS_FORMAT_DXT1)
            this.FourCC = 0x31545844;  //"DXT1"
          if (fileFormat == DdsFileFormat.DDS_FORMAT_DXT3)
            this.FourCC = 0x33545844;  //"DXT3"
          if (fileFormat == DdsFileFormat.DDS_FORMAT_DXT5)
            this.FourCC = 0x35545844;  //"DXT5"
          break;

        case DdsFileFormat.DDS_FORMAT_A8R8G8B8:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGBA;
          this.RgbBitCount = 0x000000020;
          this.FourCC = 0;
          this.BitMaskA = 0xff000000;
          this.BitMaskR = 0x00ff0000;
          this.BitMaskG = 0x0000ff00;
          this.BitMaskB = 0x000000ff;
          break;

        case DdsFileFormat.DDS_FORMAT_X8R8G8B8:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGB;
          this.RgbBitCount = 0x00000020;
          this.FourCC = 0x00000000;
          this.BitMaskA = 0x00000000;
          this.BitMaskR = 0x00ff0000;
          this.BitMaskG = 0x0000ff00;
          this.BitMaskB = 0x000000ff;
          break;

        case DdsFileFormat.DDS_FORMAT_A8B8G8R8:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGBA;
          this.RgbBitCount = 0x00000020;
          this.FourCC = 0x00000000;
          this.BitMaskA = 0xff000000;
          this.BitMaskR = 0x000000ff;
          this.BitMaskG = 0x0000ff00;
          this.BitMaskB = 0x00ff0000;
          break;

        case DdsFileFormat.DDS_FORMAT_X8B8G8R8:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGB;
          this.RgbBitCount = 0x00000020;
          this.FourCC = 0x00000000;
          this.BitMaskA = 0x00000000;
          this.BitMaskR = 0x000000ff;
          this.BitMaskG = 0x0000ff00;
          this.BitMaskB = 0x00ff0000;
          break;

        case DdsFileFormat.DDS_FORMAT_A1R5G5B5:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGBA;
          this.RgbBitCount = 0x00000010;
          this.FourCC = 0x00000000;
          this.BitMaskA = 0x00008000;
          this.BitMaskR = 0x00007c00;
          this.BitMaskG = 0x000003e0;
          this.BitMaskB = 0x0000001f;
          break;

        case DdsFileFormat.DDS_FORMAT_A4R4G4B4:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGBA;
          this.RgbBitCount = 16;
          this.FourCC = 0;
          this.BitMaskA = 0x0000f000;
          this.BitMaskR = 0x00000f00;
          this.BitMaskG = 0x000000f0;
          this.BitMaskB = 0x0000000f;
          break;

        case DdsFileFormat.DDS_FORMAT_R8G8B8:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGB;
          this.RgbBitCount = 0x00000018;
          this.FourCC = 0x00000000;
          this.BitMaskA = 0x00000000;
          this.BitMaskR = 0x00ff0000;
          this.BitMaskG = 0x0000ff00;
          this.BitMaskB = 0x000000ff;
          break;

        case DdsFileFormat.DDS_FORMAT_R5G6B5:
          this.Flags = (UInt32)PixelFormatFlags.DDS_RGB;
          this.RgbBitCount = 0x00000010;
          this.FourCC = 0x00000000;
          this.BitMaskA = 0x00000000;
          this.BitMaskR = 0x0000f800;
          this.BitMaskG = 0x000007e0;
          this.BitMaskB = 0x0000001f;
          break;

        default:
          break;
      }
    }

    public void Read(BinaryReader input) {
      if (input.ReadUInt32() != this.Size) {
        throw new FormatException(
          String.Format(@"Invalid DdsPixelFormat size read from input " +
          "stream at position 0x{0:08x}", input.BaseStream.Position));
      }
      this.Flags = input.ReadUInt32();
      this.FourCC = input.ReadUInt32();
      this.RgbBitCount = input.ReadUInt32();
      this.BitMaskR = input.ReadUInt32();
      this.BitMaskG = input.ReadUInt32();
      this.BitMaskB = input.ReadUInt32();
      this.BitMaskA = input.ReadUInt32();
    }

    public void Write(BinaryWriter output) {
      output.Write(this.Size);
      output.Write(this.Flags);
      output.Write(this.FourCC);
      output.Write(this.RgbBitCount);
      output.Write(this.BitMaskR);
      output.Write(this.BitMaskG);
      output.Write(this.BitMaskB);
      output.Write(this.BitMaskA);
    }

    /// <summary>Size of DdsPixelFormat structure</summary>
    public UInt32 Size {
      // A DdsPixelFormat structure is eight UInt32 fields long.
      get { return 8 * sizeof(UInt32); }
    }
    public UInt32 Flags {
      get { return m_flags; }
      set { m_flags = value; }
    }
    public UInt32 FourCC {
      get { return m_fourCC; }
      set { m_fourCC = value; }
    }
    public UInt32 RgbBitCount {
      get { return m_rgbBitCount; }
      set { m_rgbBitCount = value; }
    }
    public UInt32 BitMaskA {
      get { return m_bitMaskA; }
      set { m_bitMaskA = value; }
    }
    public UInt32 BitMaskR {
      get { return m_bitMaskR; }
      set { m_bitMaskR = value; }
    }
    public UInt32 BitMaskG {
      get { return m_bitMaskG; }
      set { m_bitMaskG = value; }
    }
    public UInt32 BitMaskB {
      get { return m_bitMaskB; }
      set { m_bitMaskB = value; }
    }
  }

  public class DdsHeader {
    private UInt32 m_headerFlags;
    private UInt32 m_height;
    private UInt32 m_width;
    private UInt32 m_pitchOrLinearSize;
    private UInt32 m_depth;
    private UInt32 m_mipMapCount;
    private UInt32[] m_reserved1;
    private DdsPixelFormat m_pixelFormat;
    private UInt32 m_surfaceFlags;
    private UInt32 m_cubemapFlags;
    private UInt32[] m_reserved2;


    public DdsHeader() {
      m_pixelFormat = new DdsPixelFormat();
    }

    public void Read(BinaryReader input) {
      if (input.ReadUInt32() != this.Size) {
        throw new FormatException(
          String.Format(@"Invalid DdsHeader size read from input " +
          "stream at position 0x{0:08x}", input.BaseStream.Position));
      }
      this.Flags = input.ReadUInt32();
      this.Height = input.ReadUInt32();
      this.Width = input.ReadUInt32();
      this.PitchOrLinearSize = input.ReadUInt32();
      this.PixelDepth = input.ReadUInt32();
      this.MipMapCount = input.ReadUInt32();
      this.Reserved1 = new UInt32[11];
      for (int i = 0; i < this.Reserved1.Length; i++) {
        this.Reserved1[i] = input.ReadUInt32();
      }
      this.PixelFormat.Read(input);
      this.SurfaceFlags = input.ReadUInt32();
      this.CubemapFlags = input.ReadUInt32();
      this.Reserved2 = new UInt32[3];
      for (int i = 0; i < this.Reserved2.Length; i++) {
        this.Reserved2[i] = input.ReadUInt32();
      }
    }

    public void Write(BinaryWriter output) {
      output.Write(this.Size);
      output.Write(this.Flags);
      output.Write(this.Height);
      output.Write(this.Width);
      output.Write(this.PitchOrLinearSize);
      output.Write(this.PixelDepth);
      output.Write(this.MipMapCount);
      for (int i = 0; i < this.Reserved1.Length; i++)
        output.Write(this.Reserved1[i]);
      this.m_pixelFormat.Write(output);
      output.Write(this.m_surfaceFlags);
      output.Write(this.m_cubemapFlags);
      for (int i = 0; i < 3; i++) { output.Write(this.m_reserved2[i]); }
    }

    public UInt32 Size {
      get {
        // 23 UInt32 fields and a DdsPixelFormat structure
        return 23 * sizeof(UInt32) + m_pixelFormat.Size;
      }
    }
    public UInt32 Flags {
      get { return m_headerFlags; }
      set { m_headerFlags = value; }
    }
    public UInt32 Height {
      get { return m_height; }
      set { m_height = value; }
    }
    public UInt32 Width {
      get { return m_width; }
      set { m_width = value; }
    }
    public UInt32 PitchOrLinearSize {
      get { return m_pitchOrLinearSize; }
      set { m_pitchOrLinearSize = value; }
    }
    public UInt32 PixelDepth {
      get { return m_depth; }
      set { m_depth = value; }
    }
    public UInt32 MipMapCount {
      get { return m_mipMapCount; }
      set { m_mipMapCount = value; }
    }
    public UInt32[] Reserved1 {
      get { return m_reserved1; }
      set { m_reserved1 = value; }
    }
    public DdsPixelFormat PixelFormat {
      get { return m_pixelFormat; }
      set { m_pixelFormat = value; }
    }
    public UInt32 SurfaceFlags {
      get { return m_surfaceFlags; }
      set { m_surfaceFlags = value; }
    }
    public UInt32 CubemapFlags {
      get { return m_cubemapFlags; }
      set { m_cubemapFlags = value; }
    }
    public UInt32[] Reserved2 {
      get { return m_reserved2; }
      set { m_reserved2 = value; }
    }
  }

  public class DdsFile {
    private DdsHeader m_header;

    public DdsFile() {
      m_header = new DdsHeader();
    }

    /*
    public void Save(System.IO.Stream output, Surface surface, DdsSaveConfigToken ddsToken, ProgressEventHandler progressCallback) {
      // For non-compressed textures, we need pixel width.
      int pixelWidth = 0;

      // Identify if we're a compressed image
      bool isCompressed = ((ddsToken.m_fileFormat == DdsFileFormat.DDS_FORMAT_DXT1) ||
                  (ddsToken.m_fileFormat == DdsFileFormat.DDS_FORMAT_DXT3) ||
                  (ddsToken.m_fileFormat == DdsFileFormat.DDS_FORMAT_DXT5));

      // Compute mip map count..
      int mipCount = 1;
      int mipWidth = surface.Width;
      int mipHeight = surface.Height;

      if (ddsToken.m_generateMipMaps) {
        // This breaks!

        while ((mipWidth > 1) || (mipHeight > 1)) {
          mipCount++;
          mipWidth /= 2;
          mipHeight /= 2;
        }
      }

      // Populate bulk of our DdsHeader
      m_header.m_size = m_header.Size();
      m_header.m_headerFlags = (uint)(DdsHeader.HeaderFlags.DDS_HEADER_FLAGS_TEXTURE);

      if (isCompressed)
        m_header.m_headerFlags |= (uint)(DdsHeader.HeaderFlags.DDS_HEADER_FLAGS_LINEARSIZE);
      else
        m_header.m_headerFlags |= (uint)(DdsHeader.HeaderFlags.DDS_HEADER_FLAGS_PITCH);

      if (mipCount > 1)
        m_header.m_headerFlags |= (uint)(DdsHeader.HeaderFlags.DDS_HEADER_FLAGS_MIPMAP);

      m_header.m_height = (uint)surface.Height;
      m_header.m_width = (uint)surface.Width;

      if (isCompressed) {
        // Compresssed textures have the linear flag set.So pitchOrLinearSize
        // needs to contain the entire size of the DXT block.
        int blockCount = ((surface.Width + 3) / 4) * ((surface.Height + 3) / 4);
        int blockSize = (ddsToken.m_fileFormat == 0) ? 8 : 16;
        m_header.m_pitchOrLinearSize = (uint)(blockCount * blockSize);
      }
      else {
        // Non-compressed textures have the pitch flag set. So pitchOrLinearSize
        // needs to contain the row pitch of the main image. DWORD aligned too.
        switch (ddsToken.m_fileFormat) {
        case DdsFileFormat.DDS_FORMAT_A8R8G8B8:
        case DdsFileFormat.DDS_FORMAT_X8R8G8B8:
        case DdsFileFormat.DDS_FORMAT_A8B8G8R8:
        case DdsFileFormat.DDS_FORMAT_X8B8G8R8:
          pixelWidth = 4;    // 32bpp
          break;

        case DdsFileFormat.DDS_FORMAT_A1R5G5B5:
        case DdsFileFormat.DDS_FORMAT_A4R4G4B4:
        case DdsFileFormat.DDS_FORMAT_R5G6B5:
          pixelWidth = 2;    // 16bpp
          break;

        case DdsFileFormat.DDS_FORMAT_R8G8B8:
          pixelWidth = 3;    // 24bpp
          break;
        }

        // Compute row pitch
        m_header.m_pitchOrLinearSize = (uint)((int)m_header.m_width * pixelWidth);

#if  APPLY_PITCH_ALIGNMENT
        // Align to DWORD, if we need to.. (see notes about pitch alignment all over this code)
        m_header.m_pitchOrLinearSize = ( uint )( ( ( int )m_header.m_pitchOrLinearSize + 3 ) & ( ~3 ) );
#endif  //APPLY_PITCH_ALIGNMENT
      }

      m_header.m_depth = 0;
      m_header.m_mipMapCount = (mipCount == 1) ? 0 : (uint)mipCount;
      m_header.m_reserved1_0 = 0;
      m_header.m_reserved1_1 = 0;
      m_header.m_reserved1_2 = 0;
      m_header.m_reserved1_3 = 0;
      m_header.m_reserved1_4 = 0;
      m_header.m_reserved1_5 = 0;
      m_header.m_reserved1_6 = 0;
      m_header.m_reserved1_7 = 0;
      m_header.m_reserved1_8 = 0;
      m_header.m_reserved1_9 = 0;
      m_header.m_reserved1_10 = 0;

      // Populate our DdsPixelFormat object
      m_header.m_pixelFormat.Initialise(ddsToken.m_fileFormat);

      // Populate miscellanous header flags
      m_header.m_surfaceFlags = (uint)DdsHeader.SurfaceFlags.DDS_SURFACE_FLAGS_TEXTURE;

      if (mipCount > 1)
        m_header.m_surfaceFlags |= (uint)DdsHeader.SurfaceFlags.DDS_SURFACE_FLAGS_MIPMAP;

      m_header.m_cubemapFlags = 0;
      m_header.m_reserved2_0 = 0;
      m_header.m_reserved2_1 = 0;
      m_header.m_reserved2_2 = 0;

      // Write out our DDS tag
      Utility.WriteUInt32(output, 0x20534444); // 'DDS '

      // Write out the header
      m_header.Write(output);

      int squishFlags = ddsToken.GetSquishFlags();

      // Our output data array will be sized as necessary
      byte[] outputData;

      // Reset our mip width & height variables...
      mipWidth = surface.Width;
      mipHeight = surface.Height;

      // Figure out how much total work each mip map is
      Size[] writeSizes = new Size[mipCount];
      int[] mipPixels = new int[mipCount];
      int[] pixelsCompleted = new int[mipCount]; // # pixels completed once we have reached this mip
      long totalPixels = 0;
      for (int mipLoop = 0; mipLoop < mipCount; mipLoop++) {
        Size writeSize = new Size((mipWidth > 0) ? mipWidth : 1, (mipHeight > 0) ? mipHeight : 1);
        writeSizes[mipLoop] = writeSize;

        int thisMipPixels = writeSize.Width * writeSize.Height;
        mipPixels[mipLoop] = thisMipPixels;

        if (mipLoop == 0) {
          pixelsCompleted[mipLoop] = 0;
        }
        else {
          pixelsCompleted[mipLoop] = pixelsCompleted[mipLoop - 1] + mipPixels[mipLoop - 1];
        }

        totalPixels += thisMipPixels;
        mipWidth /= 2;
        mipHeight /= 2;
      }

      mipWidth = surface.Width;
      mipHeight = surface.Height;

      for (int mipLoop = 0; mipLoop < mipCount; mipLoop++) {
        Size writeSize = writeSizes[mipLoop];
        Surface writeSurface = new Surface(writeSize);

        if (mipLoop == 0) {
          // No point resampling the first level.. it's got exactly what we want.
          writeSurface = surface;
        }
        else {
          // I'd love to have a UI component to select what kind of resampling, but
          // there's hardly any space for custom UI stuff in the Save Dialog. And I'm
          // not having any scrollbars in there..! 
          // Also, note that each mip level is formed from the main level, to reduce
          // compounded errors when generating mips. 
          writeSurface.SuperSamplingFitSurface(surface);
        }

        DdsSquish.ProgressFn progressFn =
            delegate(int workDone, int workTotal) {
              long thisMipPixelsDone = workDone * (long)mipWidth;
              long previousMipsPixelsDone = pixelsCompleted[mipLoop];
              double progress = (double)((double)thisMipPixelsDone +
    (double)previousMipsPixelsDone) / (double)totalPixels;
              progressCallback(this, new ProgressEventArgs(100.0 * progress));
            };

        if ((ddsToken.m_fileFormat >= DdsFileFormat.DDS_FORMAT_DXT1) &&
      (ddsToken.m_fileFormat <= DdsFileFormat.DDS_FORMAT_DXT5))
          outputData = DdsSquish.CompressImage(writeSurface, squishFlags,
      (progressCallback == null) ? null : progressFn);
        else {
          int mipPitch = pixelWidth * writeSurface.Width;

          // From the DDS documents I read, I'd expected the pitch of each mip level to be
          // DWORD aligned. As it happens, that's not the case. Re-aligning the pitch of 
          // each level results in later mips getting sheared as the pitch is incorrect.
          // So, the following line is intentionally optional. Maybe the documentation
          // is referring to the pitch when accessing the mip directly.. who knows. 
          //
          // Infact, all the talk of non-compressed textures having DWORD alignment of pitch
          // seems to be bollocks.. If I apply alignment, then they fail to load in 3rd Party
          // or Microsoft DDS viewing applications.
          //

#if  APPLY_PITCH_ALIGNMENT
          mipPitch = ( mipPitch + 3 ) & ( ~3 );
#endif // APPLY_PITCH_ALIGNMENT

          outputData = new byte[mipPitch * writeSurface.Height];
          outputData.Initialize();

          for (int y = 0; y < writeSurface.Height; y++) {
            for (int x = 0; x < writeSurface.Width; x++) {
              // Get colour from surface
              ColorBgra pixelColour = writeSurface.GetPoint(x, y);
              uint pixelData = 0;

              switch (ddsToken.m_fileFormat) {
              case DdsFileFormat.DDS_FORMAT_A8R8G8B8: {
                  pixelData = ((uint)pixelColour.A << 24) |
                        ((uint)pixelColour.R << 16) |
                        ((uint)pixelColour.G << 8) |
                        ((uint)pixelColour.B << 0);
                  break;
                }

              case DdsFileFormat.DDS_FORMAT_X8R8G8B8: {
                  pixelData = ((uint)pixelColour.R << 16) |
                        ((uint)pixelColour.G << 8) |
                        ((uint)pixelColour.B << 0);
                  break;
                }

              case DdsFileFormat.DDS_FORMAT_A8B8G8R8: {
                  pixelData = ((uint)pixelColour.A << 24) |
                        ((uint)pixelColour.B << 16) |
                        ((uint)pixelColour.G << 8) |
                        ((uint)pixelColour.R << 0);
                  break;
                }

              case DdsFileFormat.DDS_FORMAT_X8B8G8R8: {
                  pixelData = ((uint)pixelColour.B << 16) |
                        ((uint)pixelColour.G << 8) |
                        ((uint)pixelColour.R << 0);
                  break;
                }

              case DdsFileFormat.DDS_FORMAT_A1R5G5B5: {
                  pixelData = ((uint)((pixelColour.A != 0) ? 1 : 0) << 15) |
                        ((uint)(pixelColour.R >> 3) << 10) |
                        ((uint)(pixelColour.G >> 3) << 5) |
                        ((uint)(pixelColour.B >> 3) << 0);
                  break;
                }

              case DdsFileFormat.DDS_FORMAT_A4R4G4B4: {
                  pixelData = ((uint)(pixelColour.A >> 4) << 12) |
                        ((uint)(pixelColour.R >> 4) << 8) |
                        ((uint)(pixelColour.G >> 4) << 4) |
                        ((uint)(pixelColour.B >> 4) << 0);
                  break;
                }

              case DdsFileFormat.DDS_FORMAT_R8G8B8: {
                  pixelData = ((uint)pixelColour.R << 16) |
                        ((uint)pixelColour.G << 8) |
                        ((uint)pixelColour.B << 0);
                  break;
                }

              case DdsFileFormat.DDS_FORMAT_R5G6B5: {
                  pixelData = ((uint)(pixelColour.R >> 3) << 11) |
                        ((uint)(pixelColour.G >> 2) << 5) |
                        ((uint)(pixelColour.B >> 3) << 0);
                  break;
                }
              }

              // pixelData contains our target data.. so now set the pixel bytes
              int pixelOffset = (y * mipPitch) + (x * pixelWidth);
              for (int loop = 0; loop < pixelWidth; loop++) {
                outputData[pixelOffset + loop] = (byte)((pixelData >> (8 * loop)) & 0xff);
              }
            }

            if (progressCallback != null) {
              long thisMipPixelsDone = (y + 1) * (long)mipWidth;
              long previousMipsPixelsDone = pixelsCompleted[mipLoop];
              double progress = (double)((double)thisMipPixelsDone +
  (double)previousMipsPixelsDone) / (double)totalPixels;
              progressCallback(this, new ProgressEventArgs(100.0 * progress));
            }
          }
        }

        // Write the data for this mip level out.. 
        output.Write(outputData, 0, outputData.GetLength(0));

        mipWidth = mipWidth / 2;
        mipHeight = mipHeight / 2;
      }
    }*/

    public Bitmap Load(BinaryReader input) {
      // Read the DDS tag. If it's not right, then bail.. 
      UInt32 ddsTag = input.ReadUInt32();
      if (ddsTag != 0x20534444)
        throw new FormatException("File does not appear to be a DDS image");

      m_header.Read(input);

      if ((Header.PixelFormat.Flags &
        (UInt32)PixelFormatFlags.DDS_FOURCC) != 0) {
        UInt32 squishFlags = 0;

        switch (Header.PixelFormat.FourCC) {
          case 0x31545844:
            squishFlags = (UInt32)SquishFlags.Dxt1;
            break;

          case 0x33545844:
            squishFlags = (UInt32)SquishFlags.Dxt3;
            break;

          case 0x35545844:
            squishFlags = (UInt32)SquishFlags.Dxt5;
            break;

          default:
            throw new FormatException("File is not a supported DDS format");
        }

        // Compute size of compressed block area
        int blockCount = ((this.Width + 3) / 4) * ((this.Height + 3) / 4);
        int blockSize = ((squishFlags & (UInt32)SquishFlags.Dxt1) != 0) ? 8 : 16;

        // Allocate room for compressed blocks, and read data into it.
        byte[] compressedBlocks = new byte[blockCount * blockSize];
        input.Read(compressedBlocks, 0, compressedBlocks.GetLength(0));

        // Now decompress..
        return DdsSquish.DecompressImage(compressedBlocks,
          this.Width, this.Height, (int)squishFlags);
      }
      else {
        // We can only deal with the non-DXT formats we know about..  this is a bit of a mess..
        // Sorry..
        DdsFileFormat fileFormat = DdsFileFormat.DDS_FORMAT_INVALID;

        if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGBA) &&
          (this.Header.PixelFormat.RgbBitCount == 0x20) &&
          (this.Header.PixelFormat.BitMaskA == 0xff000000) &&
          (this.Header.PixelFormat.BitMaskR == 0x00ff0000) &&
          (this.Header.PixelFormat.BitMaskG == 0x0000ff00) &&
          (this.Header.PixelFormat.BitMaskB == 0x000000ff))
          fileFormat = DdsFileFormat.DDS_FORMAT_A8R8G8B8;

        else if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGB) &&
          (this.Header.PixelFormat.RgbBitCount == 0x20) &&
          (this.Header.PixelFormat.BitMaskA == 0x00000000) &&
          (this.Header.PixelFormat.BitMaskR == 0x00ff0000) &&
          (this.Header.PixelFormat.BitMaskG == 0x0000ff00) &&
          (this.Header.PixelFormat.BitMaskB == 0x000000ff))
          fileFormat = DdsFileFormat.DDS_FORMAT_X8R8G8B8;

        else if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGBA) &&
          (this.Header.PixelFormat.RgbBitCount == 0x20) &&
          (this.Header.PixelFormat.BitMaskA == 0xff000000) &&
          (this.Header.PixelFormat.BitMaskR == 0x000000ff) &&
          (this.Header.PixelFormat.BitMaskG == 0x0000ff00) &&
          (this.Header.PixelFormat.BitMaskB == 0x00ff0000))
          fileFormat = DdsFileFormat.DDS_FORMAT_A8B8G8R8;

        else if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGB) &&
          (this.Header.PixelFormat.RgbBitCount == 0x20) &&
          (this.Header.PixelFormat.BitMaskA == 0x00000000) &&
          (this.Header.PixelFormat.BitMaskR == 0x000000ff) &&
          (this.Header.PixelFormat.BitMaskG == 0x0000ff00) &&
          (this.Header.PixelFormat.BitMaskB == 0x00ff0000))
          fileFormat = DdsFileFormat.DDS_FORMAT_X8B8G8R8;

        else if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGBA) &&
          (this.Header.PixelFormat.RgbBitCount == 0x10) &&
          (this.Header.PixelFormat.BitMaskA == 0x00008000) &&
          (this.Header.PixelFormat.BitMaskR == 0x00007c00) &&
          (this.Header.PixelFormat.BitMaskG == 0x000003e0) &&
          (this.Header.PixelFormat.BitMaskB == 0x0000001f))
          fileFormat = DdsFileFormat.DDS_FORMAT_A1R5G5B5;

        else if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGBA) &&
          (this.Header.PixelFormat.RgbBitCount == 0x10) &&
          (this.Header.PixelFormat.BitMaskA == 0x0000f000) &&
          (this.Header.PixelFormat.BitMaskR == 0x00000f00) &&
          (this.Header.PixelFormat.BitMaskG == 0x000000f0) &&
          (this.Header.PixelFormat.BitMaskB == 0x0000000f))
          fileFormat = DdsFileFormat.DDS_FORMAT_A4R4G4B4;

        else if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGB) &&
          (this.Header.PixelFormat.RgbBitCount == 0x18) &&
          (this.Header.PixelFormat.BitMaskA == 0x00000000) &&
          (this.Header.PixelFormat.BitMaskR == 0x00ff0000) &&
          (this.Header.PixelFormat.BitMaskG == 0x0000ff00) &&
          (this.Header.PixelFormat.BitMaskB == 0x000000ff))
          fileFormat = DdsFileFormat.DDS_FORMAT_R8G8B8;

        else if (
          (this.Header.PixelFormat.Flags == (UInt32)PixelFormatFlags.DDS_RGB) &&
          (this.Header.PixelFormat.RgbBitCount == 0x10) &&
          (this.Header.PixelFormat.BitMaskA == 0x00000000) &&
          (this.Header.PixelFormat.BitMaskR == 0x0000f800) &&
          (this.Header.PixelFormat.BitMaskG == 0x000007e0) &&
          (this.Header.PixelFormat.BitMaskB == 0x0000001f))
          fileFormat = DdsFileFormat.DDS_FORMAT_R5G6B5;

        // If fileFormat is still invalid, then it's an unsupported format.
        if (fileFormat == DdsFileFormat.DDS_FORMAT_INVALID)
          throw new FormatException("File is not a supported DDS format");

        // Size of a source pixel, in bytes
        UInt32 srcPixelSize = ((UInt32)this.Header.PixelFormat.RgbBitCount / 8);

        // We need the pitch for a row, so we can allocate enough memory for the load.
        UInt32 rowPitch = 0;

        if ((this.Header.Flags & (UInt32)HeaderFlags.DDS_HEADER_FLAGS_PITCH) != 0) {
          // Pitch specified.. so we can use directly
          rowPitch = (UInt32)this.Header.PitchOrLinearSize;
        }
        else
          if ((this.Header.Flags & (int)HeaderFlags.DDS_HEADER_FLAGS_LINEARSIZE) != 0) {
            // Linear size specified. compute row pitch. Of course, this should never happen
            // as linear size is *supposed* to be for compressed textures. But Microsoft don't 
            // always play by the rules when it comes to DDS output. 
            rowPitch = (UInt32)this.Header.PitchOrLinearSize / this.Header.Height;
          }
          else {
            // Another case of Microsoft not obeying their standard is the
            // 'Convert to..' shell extension that ships in the DirectX SDK.
            // Seems to always leave flags empty..so no indication of pitch
            // or linear size. And - to cap it all off - they leave
            // pitchOrLinearSize as *zero*. Zero??? If we get this bizarre set
            // of inputs, we just go 'screw it' and compute row pitch
            // ourselves, making sure we DWORD align it (if that code path is
            // enabled).
            rowPitch = (this.Header.Width * srcPixelSize);

            #if APPLY_PITCH_ALIGNMENT
            rowPitch = ( ( ( int )rowPitch + 3 ) & ( ~3 ) );
            #endif
          }

        // Ok.. now, we need to allocate room for the bytes to read in from.
        // It's rowPitch bytes * height
        byte[] readPixelData = new byte[rowPitch * this.Header.Height];
        input.Read(readPixelData, 0, readPixelData.GetLength(0));

        // We now need space for the real pixel data.
        byte[] pixelData = new byte[this.Header.Width * this.Header.Height * 4];

        // And now we have the arduous task of filling that up with stuff..
        for (UInt32 destY = 0; destY < this.Header.Height; destY++) {
          for (UInt32 destX = 0; destX < this.Header.Width; destX++) {
            // Compute source pixel offset
            UInt32 srcPixelOffset = (destY * rowPitch) + (destX * srcPixelSize);

            // Read our pixel
            UInt32 pixelColor = 0;
            UInt32 pixelR = 0;
            UInt32 pixelG = 0;
            UInt32 pixelB = 0;
            UInt32 pixelA = 0;

            // Build our pixel colour as a DWORD  
            for (int loop = 0; loop < srcPixelSize; loop++) {
              pixelColor |= (uint)(readPixelData[srcPixelOffset + loop] << (8 * loop));
            }

            switch (fileFormat) {
              case DdsFileFormat.DDS_FORMAT_A8R8G8B8:
                pixelA = (pixelColor >> 0x18) & 0xff;
                pixelR = (pixelColor >> 0x10) & 0xff;
                pixelG = (pixelColor >> 0x08) & 0xff;
                pixelB = pixelColor & 0xff;
                break;
              case DdsFileFormat.DDS_FORMAT_X8R8G8B8:
                pixelA = 0xff;
                pixelR = (pixelColor >> 0x10) & 0xff;
                pixelG = (pixelColor >> 0x08) & 0xff;
                pixelB = pixelColor & 0xff;
                break;
              case DdsFileFormat.DDS_FORMAT_A8B8G8R8:
                pixelA = (pixelColor >> 0x18) & 0xff;
                pixelR = pixelColor & 0xff;
                pixelG = (pixelColor >> 0x08) & 0xff;
                pixelB = (pixelColor >> 0x10) & 0xff;
                break;
              case DdsFileFormat.DDS_FORMAT_X8B8G8R8:
                pixelA = 0xff;
                pixelR = pixelColor & 0xff;
                pixelG = (pixelColor >> 0x08) & 0xff;
                pixelB = (pixelColor >> 0x10) & 0xff;
                break;
              case DdsFileFormat.DDS_FORMAT_A1R5G5B5:
                pixelA = (pixelColor >> 15) * 0xff;
                pixelR = (pixelColor >> 10) & 0x1f;
                pixelR = (pixelR << 3) | (pixelR >> 2);
                pixelG = (pixelColor >> 5) & 0x1f;
                pixelG = (pixelG << 3) | (pixelG >> 2);
                pixelB = (pixelColor >> 0) & 0x1f;
                pixelB = (pixelB << 3) | (pixelB >> 2);
                break;
              case DdsFileFormat.DDS_FORMAT_A4R4G4B4:
                pixelA = (pixelColor >> 12) & 0xff;
                pixelA = (pixelA << 4) | (pixelA >> 0);
                pixelR = (pixelColor >> 8) & 0x0f;
                pixelR = (pixelR << 4) | (pixelR >> 0);
                pixelG = (pixelColor >> 4) & 0x0f;
                pixelG = (pixelG << 4) | (pixelG >> 0);
                pixelB = (pixelColor >> 0) & 0x0f;
                pixelB = (pixelB << 4) | (pixelB >> 0);
                break;
              case DdsFileFormat.DDS_FORMAT_R8G8B8:
                pixelA = 0xff;
                pixelR = (pixelColor >> 0x10) & 0xff;
                pixelG = (pixelColor >> 0x08) & 0xff;
                pixelB = pixelColor & 0xff;
                break;
              case DdsFileFormat.DDS_FORMAT_R5G6B5:
                pixelA = 0xff;
                pixelR = (pixelColor >> 0x0b) & 0x1f;
                pixelR = (pixelR << 0x03) | (pixelR >> 0x02);
                pixelG = (pixelColor >> 0x05) & 0x3f;
                pixelG = (pixelG << 0x02) | (pixelG >> 0x04);
                pixelB = pixelColor & 0x1f;
                pixelB = (pixelB << 0x03) | (pixelB >> 0x02);
                break;
            }

            UInt32 destPixelOffset = (destY * this.Header.Width * 4) +
              (destX * 4);
            pixelData[destPixelOffset + 0] = (byte)pixelB;
            pixelData[destPixelOffset + 1] = (byte)pixelG;
            pixelData[destPixelOffset + 2] = (byte)pixelR;
            pixelData[destPixelOffset + 3] = (byte)pixelA;
          }
        }

        // Copy the pixel data to a new bitmap
        Bitmap bmp = new Bitmap(this.Width, this.Height,
          PixelFormat.Format32bppArgb);
        Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
        BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly,
          bmp.PixelFormat);
        System.Runtime.InteropServices.Marshal.Copy(pixelData, 0,
          bmpData.Scan0, this.Stride * this.Height);
        bmp.UnlockBits(bmpData);
        return bmp;
      }
    }

    public int Width {
      get { return (int)this.Header.Width; }
    }

    public int Height {
      get { return (int)this.Header.Height; }
    }

    public int Stride {
      get { return (int)this.Width * 0x04; }
    }

    public DdsHeader Header {
      get { return m_header; }
      set { m_header = value; }
    }
  }
}

/***
 * License Note
 * 
 * This file is a modified version of the Paint.NET DDS File Type Plugin.  The
 * following note pertains to the plug-in:
 * ----------------------------------------------------------------------------
 * Copyright (c) 2007 Dean Ashton (http://www.dmashton.co.uk)
 *
 * Permission is hereby granted, free of charge, to any person obtaining a
 * copy of this software and associated documentation files (the "Software",
 * to	deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS or
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
 * DEALINGS IN THE SOFTWARE.
 * ----------------------------------------------------------------------------
 *
 * The DDS File Type Plugin author's home page can currently be found at the
 * following URL:
 * http://www.dmashton.co.uk/articles/dds-support-for-paintnet/
 * 
 * Paint.NET can currently be found at the following URL:
 * http://www.getpaint.net/
 ***/