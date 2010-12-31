using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace ParagonForge.PiggInterface {
  /// <summary>
  /// Facilitates reading from and writing to Pigg files.
  /// </summary>
  public class PiggStream : Stream {
    #region PiggStream Constructors
    /// <summary>
    /// Create a new PiggStream object.
    /// </summary>
    /// <param name="LeafInfo">PiggLeafInfo object that describes the file
    /// that will be read or written.</param>
    public PiggStream(PiggLeafInfo LeafInfo) {
      this.LeafInfo = LeafInfo;

      // Only open the file for reading when we need to.
      this.FileSource = null;
      this.DeflateSource = null;
      this.Cache = null;
      this.StreamPosition = 0;
    }
    #endregion

    #region PiggStream Member Functions

    /// <summary>
    /// Clears all buffers for this stream and causes any buffered data to be
    /// written to the underlying device.
    /// </summary>
    public override void Flush() {
      // Since our PiggStream is currently read-only, Flush() doesn't need
      // to do anything.
    }

    /// <summary>
    /// Sets the current position of this stream to the given value.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="origin"></param>
    /// <returns></returns>
    public override long Seek(long offset, SeekOrigin origin) {
      long absolute_position = 0;
      switch (origin) {
        case SeekOrigin.Begin:
          break;
        case SeekOrigin.Current:
          absolute_position = this.StreamPosition;
          break;
        case SeekOrigin.End:
          absolute_position = this.Length;
          break;
      }
      absolute_position += offset;
      if (absolute_position < 0)
        throw new ArgumentException("Attempt to seek before beginning of " +
          "stream.");
      if (absolute_position > this.Length) absolute_position = this.Length;
      this.StreamPosition = (int)absolute_position;
      return this.StreamPosition;
    }

    /// <summary>
    /// Reads data into the back-end buffer.
    /// </summary>
    /// <param name="count">Number of bytes to read into the buffer.</param>
    private int CacheRead(int count) {
      // Sanity check: If we're not being asked to actually read anything,
      // just bail out.
      if (count == 0) return 0;

      // If we're asked to read an invalid number of bytes, gripe.  Since we
      // will only ever call this function internally, we'd have to be pretty
      // damn stupid to ever have this happen, but I've been known to have
      // some pretty glorious moments of stupidity.
      if (count < 0)
        throw new ArgumentOutOfRangeException("Invalid count specified.  " +
          "Count must be non-negative.");

      // If we don't have a cache already, create one.
      if (this.Cache == null) {
        this.Cache = new byte[this.LeafInfo.UncompressedSize];
        this.CachePosition = 0;
        this.CacheBytesRead = 0;
      }

      // If we haven't hooked up to the back-end source of the data, do so.
      if (this.FileSource == null) {
        // Open the source file and go to where the info is.
        this.FileSource = new FileStream(this.LeafInfo.File.FullPath,
          FileMode.Open, FileAccess.Read, FileShare.Read);
        this.FileSource.Seek(this.LeafInfo.Offset, SeekOrigin.Begin);

        if (this.LeafInfo.CompressedSize != 0) {  // File is compressed
          // The zlib spec that pigg files use (RFC 1950) include a two-byte
          // header that a DeflateStream object (RFC 1951) doesn't.  We need
          // to eat those two bytes.  The zlib format is found here:
          // http://www.ietf.org/rfc/rfc1950.txt
          // The DeflateStream format is found here:
          // http://www.ietf.org/rfc/rfc1951.txt
          int zlib_cmf = this.FileSource.ReadByte();
          int zlib_flg = this.FileSource.ReadByte();

          // As long as we're eating bytes, let's parse them out just for
          // curiosity and some error checking.  For details on what these
          // bytes are used for, please see RFC 1950 documentation at the
          // above link.  They're the CMF and FLG bytes.
          if ((zlib_cmf & 0x08) != 8) {
            // Compression method isn't deflate
            throw
              new FormatException("Compressed file format not recognized.");
          }
          int lz77_window_size = 1 << (int)((zlib_cmf >> 0x04) + 0x08);
          if ((zlib_cmf << 0x08 | zlib_flg) % 0x1f != 0) {
            // Bits 0 - 3 of the FLG byte are a checksum such that these two
            // bytes together should be divisible by 31.
            throw new FormatException("Compressed file FCHECK flag invalid");
          }
          bool zlib_fdict = (zlib_flg & 0x20) != 0;
          // Compression level: 0 is fastest, 3 is slowest and max compression.
          int zlib_compression_level = zlib_flg >> 0x06;
        }
      }

      Stream source;
      // If the back-end file is not compressed, read it in directly from the
      // pigg file.
      if (this.LeafInfo.CompressedSize == 0) {
        source = this.FileSource;
      }

      // If the back-end file is compressed, read it from an intermediary
      // DeflateStream object using the pigg file as a back-end source.
      else {
        if (this.DeflateSource == null) {
          // Set up the deflation stream
          this.DeflateSource = new DeflateStream(this.FileSource,
            CompressionMode.Decompress);
        }
        source = this.DeflateSource;
      }

      // Now that everything is set up, actually read the data.
      int bytes_to_read = Math.Min(count, Cache.Length - CachePosition);
      int bytes_read = source.Read(this.Cache, (int)this.CachePosition, bytes_to_read);
      this.CachePosition += bytes_read;
      this.CacheBytesRead += bytes_read;
      return bytes_read;
    }

    /// <summary>
    /// Sets the length of this stream to the given value.
    /// </summary>
    /// <param name="value">Lenght</param>
    public override void SetLength(long value) {
      throw new NotSupportedException();
    }

    /// <summary>
    /// Reads a block of bytes from the stream and writes the data in a given
    /// buffer.
    /// </summary>
    /// <param name="buffer">When this method returns, contains the specified
    /// byte array with the values between offset and (offset + count - 1)
    /// replaced by the bytes read from the current source.</param>
    /// <param name="offset">The byte offset in array at which to begin
    /// reading.</param>
    /// <param name="count">The maximum number of bytes to read.</param>
    /// <returns>The total number of bytes read into the buffer. This might be
    /// less than the number of bytes requested if that number of bytes are
    /// not currently available, or zero if the end of the stream is reached.
    /// </returns>
    public override int Read(byte[] buffer, int offset, int count) {
      // Sanity checks to make sure we're not trying to do something stupid.
      // (Or evil.)
      if (count == 0) return 0;  // Nothing to read.
      if (buffer == null)
        throw new ArgumentNullException("Null buffer provided.");
      if (offset < 0)
        throw new ArgumentOutOfRangeException("Invalid offset specified.  " +
          "Offset must be non-negative.");
      if (count < 0)
        throw new ArgumentOutOfRangeException("Invalid count specified.  " +
          "Count must be non-negative.");
      if (offset + count > buffer.Length)
        throw new ArgumentException("Requested data extends beyond size of " +
          "buffer.");

      // Check to see if we need to read anything into the back-end cache, and
      // if so, do it.
      int to_position_needed = this.StreamPosition + count;
      if (to_position_needed > this.CacheBytesRead) {
        // Try to read enough bytes into the cache to satisfy the request.
        this.CacheRead(to_position_needed - this.CacheBytesRead);
      }
      // bytes_available is the number of bytes we have been able to retrieve.
      int bytes_available = Math.Min(this.CacheBytesRead - this.StreamPosition,
        count);
      // Ideally, it will be enough to satisfy the request.  If it's not, then
      // we'll return what we could get.
      Array.Copy(this.Cache, this.StreamPosition, buffer, offset,
        bytes_available);
      this.StreamPosition += bytes_available;
      return bytes_available;
    }

    /// <summary>
    /// Reads a byte from the stream and advances the position within the
    /// stream by one byte, or returns -1 if at the end of the stream.
    /// </summary>
    /// <returns>The unsigned byte cast to an Int32, or -1 if at the end of
    /// the stream.</returns>
    public override int ReadByte() {
      byte[] buffer = new byte[1];
      int bytes_read = this.Read(buffer, 0, 1);
      return bytes_read > 0 ? buffer[0] : -1;
    }

    /// <summary>
    /// Writes a block of bytes to this stream using data from a buffer.
    /// </summary>
    /// <param name="buffer">The buffer containing data to write to the
    /// stream.</param>
    /// <param name="offset">The zero-based byte offset in array at which to
    /// begin copying bytes to the current stream.</param>
    /// <param name="count">The maximum number of bytes to be written to the
    /// current stream.</param>
    public override void Write(byte[] buffer, int offset, int count) {
      throw new NotSupportedException();
    }

    /// <summary>
    /// Closes the current stream and releases any resources (such as sockets
    /// and file handles) associated with the current stream.
    /// </summary>
    public override void Close() {
      // Close any underlying streams we have open.
      if (this.DeflateSource != null) {
        this.DeflateSource.Close();
        this.DeflateSource = null;
      }
      if (this.FileSource != null) {
        this.FileSource.Close();
        this.FileSource = null;
      }

      // Release the cache
      this.Cache = null;
      base.Close();
    }

    #endregion

    #region PiggStream Properties

    private PiggLeafInfo m_leaf_info;
    private FileStream m_file_source;
    private DeflateStream m_deflate_source;
    private int m_stream_position;
    private int m_cache_position;
    private byte[] m_cache;
    private int m_cache_bytes_read;

    /// <summary>
    /// PiggLeafInfo object that describes the file that will be read or
    /// written.
    /// </summary>
    protected PiggLeafInfo LeafInfo {
      get { return m_leaf_info; }
      set { m_leaf_info = value; }
    }

    /// <summary>
    /// Source file of the pigg stream.
    /// </summary>
    protected FileStream FileSource {
      get { return m_file_source; }
      set { m_file_source = value; }
    }

    /// <summary>
    /// Source deflation stream of the pigg stream.
    /// </summary>
    protected DeflateStream DeflateSource {
      get { return m_deflate_source; }
      set { m_deflate_source = value; }
    }

    /// <summary>
    /// Internal reference to where we are within the stream.
    /// </summary>
    protected int StreamPosition {
      get { return m_stream_position; }
      set { m_stream_position = value; }
    }

    /// <summary>
    /// Cached raw data backing the PiggStream object.  Used for random access
    /// seeking.
    /// </summary>
    protected byte[] Cache {
      get { return m_cache; }
      set { m_cache = value; }
    }

    /// <summary>
    /// The total number of bytes that have been read into the back-end raw
    /// data array.
    /// </summary>
    protected int CacheBytesRead {
      get { return m_cache_bytes_read; }
      set { m_cache_bytes_read = value; }
    }

    /// <summary>
    /// Position in the back-end source stream.
    /// </summary>
    protected int CachePosition {
      get { return m_cache_position; }
      set { m_cache_position = value; }
    }

    /// <summary>
    /// Gets a value indicating whether the current stream supports reading.
    /// </summary>
    public override bool CanRead {
      get { return true; }
    }

    /// <summary>
    /// Gets a value indicating whether the current stream supports seeking.
    /// </summary>
    public override bool CanSeek {
      get { return false; }
    }

    /// <summary>
    /// Gets a value indicating whether the current stream supports writing.
    /// </summary>
    public override bool CanWrite {
      get { return false; }
    }

    /// <summary>
    /// Gets the length in bytes of the stream.
    /// </summary>
    public override long Length {
      get { return LeafInfo.UncompressedSize; }
    }

    /// <summary>
    /// Gets or sets the current position of this stream.
    /// </summary>
    public override long Position {
      get { return this.StreamPosition; }
      set { throw new NotSupportedException(); }
    }

    #endregion
  }
}
