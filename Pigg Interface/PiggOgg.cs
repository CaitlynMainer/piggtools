using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OggVorbisDecoder;

namespace ParagonForge.PiggInterface {
  /// <summary>
  /// Encapsulates a file within a Pigg directory that is an Ogg file.
  /// </summary>
  public class PiggOgg {
    private PiggLeaf m_leaf;
    private byte[] m_sound_file;
    private double m_duration;

    /// <summary>
    /// Initialize an instance of a PiggOgg object.
    /// </summary>
    public PiggOgg() { Initialize(null, null); }
    /// <summary>
    /// Initialize an instance of a PiggOgg object.
    /// </summary>
    /// <param name="Leaf">PiggLeaf object to use as the source for the Ogg
    /// stream.</param>
    public PiggOgg(PiggLeaf Leaf) { Initialize(Leaf, null); }
    /// <summary>
    /// Initialize an instance of a PiggOgg object.
    /// </summary>
    /// <param name="Leaf">PiggLeaf object to use as the source for the Ogg
    /// stream.</param>
    /// <param name="SoundFile"></param>
    public PiggOgg(PiggLeaf Leaf, byte[] SoundFile) {
      m_leaf = Leaf;
      m_sound_file = SoundFile;
      m_duration = 0;
    }

    private void Initialize(PiggLeaf Leaf, byte[] SoundFile) {
      m_leaf = Leaf;
      m_sound_file = SoundFile;
    }

    /// <summary>
    /// Returns a byte array of the Ogg file decoded into WAV file format.
    /// </summary>
    public byte[] SoundFile {
      get {
        // If we've already done all this, just return the file we have.
        if (m_sound_file == null) {
          PiggStream p_stream = new PiggStream(m_leaf.PiggReferences[0]);
          byte[] ogg_source = new byte[p_stream.Length];
          p_stream.Read(ogg_source, 0, (int)p_stream.Length);

          OggVorbisEncodedStream ogg_stream =
            new OggVorbisEncodedStream(ogg_source);
          m_duration = ogg_stream.Duration;
          int sample_count = (int)ogg_stream.Length;

          // For 2-channel 16-bit (i.e. 2-byte) samples, we need a buffer that
          // is four times the number of samples.
          int data_size = sample_count * 4;

          // The sound file buffer will be the data size + 44 bytes, in order
          // to hold the WAV header.
          m_sound_file = new byte[data_size + 0x2c];

          // Let's go ahead and create the WAV header.
          WAVHeader(data_size).CopyTo(m_sound_file, 0);

          // The WAV header is 0x2c bytes, so copy the rest of the raw PCM
          // data starting at that offset.
          ogg_stream.Read(m_sound_file, 0x2c, data_size);
        }
        return m_sound_file;
      }
    }

    /// <summary>
    /// PiggLeaf object that is the source of the Ogg stream.
    /// </summary>
    public PiggLeaf Leaf { get { return m_leaf; } }
    /// <summary>
    /// Length of sound.
    /// </summary>
    public double Duration { get { return m_duration; } }

    /// <summary>
    /// Creates a WAV file header compatible with the Ogg decoder inside a
    /// byte array and returns it.
    /// </summary>
    /// <param name="DataSize">Size of the raw data.</param>
    /// <returns>Byte array containing the WAV file header.</returns>
    private byte[] WAVHeader(int DataSize) {
      // Our WAV headers will all be 44-bytes long.
      byte[] header = new byte[0x2c];

      // RIFF is a "magic string" identifying this as a WAV file
      ASCIIEncoding.ASCII.GetBytes("RIFF").CopyTo(header, 0x00);

      // This is the size of remainder of the WAV header plus the data.
      BitConverter.GetBytes(0x24 + DataSize).CopyTo(header, 0x04);

      // A couple more "magic strings."
      ASCIIEncoding.ASCII.GetBytes("WAVE").CopyTo(header, 0x08);
      ASCIIEncoding.ASCII.GetBytes("fmt ").CopyTo(header, 0x0c);

      // Size of this particular part of the header, 16 bytes.
      BitConverter.GetBytes((Int32)0x00000010).CopyTo(header, 0x10);

      // Audio format (1 = PCM)
      BitConverter.GetBytes((Int16)0x0001).CopyTo(header, 0x14);

      // Number of channels (2-channel stereo)
      BitConverter.GetBytes((Int16)0x0002).CopyTo(header, 0x16);

      // Sample rate.  In our case, 44100 samples per second.
      BitConverter.GetBytes((Int32)0x0000ac44).CopyTo(header, 0x18);

      // Byte rate, which is the sample rate * the number of channels *
      // the bytes per sample.  This is the total number of bytes that are
      // streamed in one second.  In our case, 44100 * 2 * 2 = 176400.
      BitConverter.GetBytes((Int32)0x0002b110).CopyTo(header, 0x1c);

      // Block alignment.  This is how many bytes total are in one sample,
      // including all channels.  In our case, two bytes per sample * two
      // channels = 4.
      BitConverter.GetBytes((Int16)0x0004).CopyTo(header, 0x20);

      // Bits per sample.  This isn't per channel, just per sample.
      // In our case, it's 16-bits.
      BitConverter.GetBytes((Int16)0x0010).CopyTo(header, 0x22);

      // Marker indicating the start of the raw data.
      ASCIIEncoding.ASCII.GetBytes("data").CopyTo(header, 0x24);

      // Size of the raw data.
      BitConverter.GetBytes(DataSize).CopyTo(header, 0x28);
      return header;
    }
  }
}
