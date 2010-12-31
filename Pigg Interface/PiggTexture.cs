using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ParagonForge.Squish;

namespace ParagonForge.PiggInterface {
  /// <summary>
  /// Encapulates the header at the beginning of all texture files.
  /// </summary>
  internal class PiggTextureHeader {
    #region PiggTextureHeader Constructors

    /// <summary>
    /// Creates a new PiggTextureHeader object.
    /// </summary>
    /// <param name="Reader"><typeparamref name="BinaryReader"/> object from
    /// which to read the texture header object.</param>
    public PiggTextureHeader(BinaryReader Reader) {
      this.Read(Reader);
    }

    #endregion

    #region PiggTextureHeader Member Functions
    public void Extract(Stream output) {
      byte[] size_bytes = BitConverter.GetBytes((Int32)this.Size);
      output.Write(size_bytes, 0, size_bytes.Length);
      output.Write(this.Source, 0, this.Source.Length);
    }

    /// <summary>
    /// Reads a texture header structure.
    /// </summary>
    /// <param name="Reader"><typeparamref name="BinaryReader"/> object from
    /// which to read the texture header.</param>
    public void Read(BinaryReader Reader) {
      this.Size = Reader.ReadInt32();

      // We're going to read the source of the texture header into a buffer
      // and keep it so that if we have to extract the original texture file
      // later, we can pull the original data instead of trying to reconstruct
      // it.
      this.Source = Reader.ReadBytes(this.Size - sizeof(Int32));
      MemoryStream ms = new MemoryStream(this.Source);
      BinaryReader m_reader = new BinaryReader(ms);

      this.DataSize = m_reader.ReadInt32();
      this.Width = m_reader.ReadInt32();
      this.Height = m_reader.ReadInt32();
      this.Unknown = new int[3];
      for (int i = 0; i < this.Unknown.Length; i++)
        Unknown[i] = m_reader.ReadInt32();
      this.Marker = m_reader.ReadInt32();  // Should always be "\0TX2"


      // There is sometimes stuff after the filename in the header.  I don't
      // know what that stuff is, but for now, let's read the string up to the
      // terminating zero.
      List<byte> string_bytes = new List<byte>();

      while (true) {
        int i = m_reader.ReadByte();
        if (i < 1) break;  // String terminator or end of file reached
        string_bytes.Add((byte)(i & 0xff));
      }
      this.Filename = System.Text.Encoding.ASCII.GetString(string_bytes.ToArray());

      // If we read directly from the stream, we would have to do a
      // Reader.BaseStream.Seek(this.Size, SeekOrigin.Begin);
      // here to account for the string sometimes not taking up all of the
      // header as noted above.  As it is, though, since we read the entire
      // header size into the Source property, we don't have to do the seek.
    }

    #endregion

    #region PiggTextureHeader Properties

    private int m_size;
    private int m_marker;
    private int m_data_size;
    private int m_width;
    private int m_height;
    private int[] m_unknown;
    private string m_filename;
    private byte[] m_source;

    internal byte[] Source {
      get { return m_source; }
      set { m_source = value; }
    }

    /// <summary>
    /// Size of the physical texture header structure.
    /// </summary>
    public int Size {
      get { return m_size; }
      set { m_size = value; }
    }
    /// <summary>
    /// Marker to ensure data file integrity.  (Currently always "\0TX2")
    /// </summary>
    public int Marker {
      get { return m_marker; }
      set { m_marker = value; }
    }
    /// <summary>
    /// Size of the image data.
    /// </summary>
    public int DataSize {
      get { return m_data_size; }
      set { m_data_size = value; }
    }
    /// <summary>
    /// Width of the image.  Note that this might be smaller than the width
    /// of the image data if the image is a DDS-formatted file.
    /// </summary>
    public int Width {
      get { return m_width; }
      set { m_width = value; }
    }
    /// <summary>
    /// Height of the image.  Note that this might be smaller than the height
    /// of the image data if the image is a DDS-formatted file.
    /// </summary>
    public int Height {
      get { return m_height; }
      set { m_height = value; }
    }
    /// <summary>
    /// Size of the image.  Note that this might be smaller than the size
    /// of the image data if the image is a DDS-formatted file.
    /// </summary>
    public Size ImageSize {
      get { return new Size(this.Width, this.Height); }
      set { this.Width = value.Width; this.Height = value.Height; }
    }
    /// <summary>
    /// Unknonwn fields.  Might be flags or markers.
    /// </summary>
    public int[] Unknown {
      get { return m_unknown; }
      set { m_unknown = value; }
    }
    /// <summary>
    /// Filename of encapsulated file within texture.
    /// </summary>
    public string Filename {
      get { return m_filename; }
      set { m_filename = value; }
    }

    #endregion
  }

  /// <summary>
  /// Encapsulates a texture file within the Pigg directory.
  /// </summary>
  public class PiggTexture {
    #region PiggTexture Constructors

    /// <summary>
    /// Create a new PiggTexture object.
    /// </summary>
    public PiggTexture() {
    }

    /// <summary>
    /// Create a new PiggTexture object.
    /// </summary>
    /// <param name="LeafInfo"><typeparamref name="PiggLeafInfo"/> object from
    /// which to read the PiggTextureHeader object.</param>
    public PiggTexture(PiggLeafInfo LeafInfo) {
      this.LeafInfo = LeafInfo;
    }

    #endregion

    #region PiggTexture Member Functions

    /// <summary>
    /// Extracts a PiggTexture object to the provided stream.
    /// </summary>
    /// <param name="output">Stream to which to write the texture.</param>
    /// <param name="type">How the texture is to be extracted.</param>
    public void Extract(Stream output, TextureExtractType type) {
      int block_size = 0x1000;
      byte[] buffer = new byte[block_size];
      switch (type) {
        case TextureExtractType.Original:
          output.Write(this.Source, 0, this.Source.Length);
          break;
        case TextureExtractType.Texture:
          this.Header.Extract(output);
          output.Write(this.Source, 0, this.Source.Length);
          break;
        case TextureExtractType.Png:
          this.Image.Save(output, ImageFormat.Png);
          break;
        case TextureExtractType.Bmp:
          this.Image.Save(output, ImageFormat.Bmp);
          break;
        case TextureExtractType.Jpeg:
          this.Image.Save(output, ImageFormat.Jpeg);
          break;
        case TextureExtractType.Gif:
          this.Image.Save(output, ImageFormat.Gif);
          break;
        case TextureExtractType.Tiff:
          this.Image.Save(output, ImageFormat.Tiff);
          break;
        default:
          throw new FormatException("Invalid extraction format.");
      }
    }

    /// <summary>
    /// Extracts the original image to a stream.
    /// </summary>
    /// <param name="output">A binary writer to the stream.</param>
    public void ExtractOriginalImage(Stream output) {
      PiggStream p_stream = new PiggStream(this.LeafInfo);
      BinaryReader reader = new BinaryReader(p_stream);

      PiggTextureHeader header = this.Header;
      int block_size = 0x1000;  // Read four kilobytes at a time
      int bytes_read;
      do {
        byte[] buffer = new byte[block_size];
        bytes_read = p_stream.Read(buffer, 0, block_size);
        if (bytes_read > 0) output.Write(buffer, 0, bytes_read);
      } while (bytes_read > 0);
    }

    /// <summary>
    /// Loads the PiggTextureHeader structure from the specified texture
    /// file.  Note that this function must be called before LoadImage().
    /// </summary>
    /// <returns>An initialized PiggTextureHeader object.</returns>
    internal PiggTextureHeader LoadHeader() {
      return new PiggTextureHeader(this.BackEndReader);
    }

    /// <summary>
    /// Loads the source data from the pigg file.
    /// </summary>
    /// <returns>A byte array containing the source data.</returns>
    internal byte[] LoadSource() {
      byte[] result = null;
      if (this.Header != null) {
        int source_size = this.LeafInfo.UncompressedSize - this.Header.Size;
        result = new byte[source_size];
        this.BackEndReader.Read(result, 0, source_size);
      }
      return result;
    }

    /// <summary>
    /// Loads an image from the specified texture file.
    /// </summary>
    /// <returns></returns>
    internal Bitmap LoadImage() {
      if (this.Source != null) {
        string ext = Path.GetExtension(this.Header.Filename).ToLower();
        MemoryStream ms = new MemoryStream(this.Source);
        switch (ext) {
          case ".dds":
            DdsFile ddsfile = new DdsFile();
            BinaryReader reader = new BinaryReader(ms);
            Bitmap image = ddsfile.Load(reader);
            reader.Close();  // This also closes the back-end memory stream.

            // Because a DDS file's dimensions must be powers of two, the bitmap
            // read might be bigger than the actual bitmap.  Crop it down to the
            // correct dimensions before returning the result.
            if (image.Width == this.Width && image.Height == this.Height) {
              // Avoid cloning the bitmap if possible for performance.
              return image;
            }
            else {
              // Crop the bitmap to the dimensions specified in the texture header.
              Rectangle rect = new Rectangle(0, 0, this.Header.Width,
                this.Header.Height);
              return image.Clone(rect, image.PixelFormat);
            }
          default:
            return new Bitmap(ms);
        }
      }
      return null;
    }

    #endregion

    #region PiggTexture Properties

    private PiggLeafInfo m_leaf_info;
    private PiggTextureHeader m_header;
    private BinaryReader m_reader;
    private Bitmap m_image;
    private byte[] m_source;

    /// <summary>
    /// PiggLeaf object that is the back-end data structure that describes
    /// this PiggTexture object.
    /// </summary>
    public PiggLeafInfo LeafInfo {
      get { return m_leaf_info; }
      set { m_leaf_info = value; }
    }

    /// <summary>
    /// Source bytes of the texture object.
    /// </summary>
    protected byte[] Source {
      get {
        if (m_source == null) {
          if (this.Header != null) {
            m_source = this.LoadSource();
          }
        }
        return m_source;
      }
    }

    /// <summary>
    /// Header of PiggTexture object.
    /// </summary>
    internal PiggTextureHeader Header {
      get {
        if (m_header == null) m_header = this.LoadHeader();
        return m_header;
      }
    }

    internal BinaryReader BackEndReader {
      get {
        if (m_reader == null) {
          if (this.LeafInfo != null) {
            PiggStream stream = new PiggStream(this.LeafInfo);
            m_reader = new BinaryReader(stream);
          }
        }
        return m_reader;
      }
    }

    /// <summary>
    /// Width of the image.
    /// </summary>
    public int Width {
      get { return Header.Width; }
      set { Header.Width = value; }
    }

    /// <summary>
    /// Height of the image.
    /// </summary>
    public int Height {
      get { return Header.Height; }
      set { Header.Height = value; }
    }

    /// <summary>
    /// A <typeparamref name="Bitmap"/> object representing the image data.
    /// </summary>
    public Bitmap Image {
      get {
        if (m_image == null) m_image = this.LoadImage();
        return m_image;
      }
      set { m_image = value; }
    }

    /// <summary>
    /// Original image filename.
    /// </summary>
    public string Filename {
      get { return this.Header.Filename; }
    }

    #endregion
  }

}
