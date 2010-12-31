using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ParagonForge.Squish {
  public enum SquishFlags {
    /// <summary>Use DXT1 compression.</summary>
    Dxt1 = 0x0001,
    /// <summary>Use DXT3 compression.</summary>
    Dxt3 = 0x0002,
    /// <summary>Use DXT5 compression.</summary>
    Dxt5 = 0x0004,

    /// <summary>Use a slow but high quality color compressor</summary>
    ColorClusterFit = 0x0008,
    /// <summary>Use a fast but low quality color compressor.</summary>
    ColorRangeFit = 0x0010,
    /// <summary>Use a very slow but very high quality color compressor.</summary>
    ColorIterativeClusterFit = 0x0100,

    /// <summary>Use a perceptual metric for color error (default).</summary>
    ColorMetricPerceptual = 0x0020,
    /// <summary>Use a uniform metric for color error.</summary>
    ColorMetricUniform = 0x0040,

    /// <summary>Weight the color by alpha during cluster fit (disabled by
    /// default).</summary>
    WeightColorByAlpha = 0x0080,
  }
  public class DdsSquish {
    private static bool Is64Bit() {
      return (Marshal.SizeOf(IntPtr.Zero) == 8);
    }

    internal delegate void ProgressFn(int workDone, int workTotal);

    private sealed class SquishInterface_32 {
      [DllImport("Squish_x86.dll")]
      internal static extern unsafe void SquishCompressImage(byte* rgba, int width, int height, byte* blocks, int flags);

      [DllImport("Squish_x86.dll")]
      internal static extern unsafe void SquishDecompressImage(byte* rgba, int width, int height, byte* blocks, int flags);

      [DllImport("Squish_x86.dll")]
      internal static extern void SquishInitialize();
    }

    private sealed class SquishInterface_32_SSE2 {
      [DllImport("Squish_x86_SSE2.dll")]
      internal static extern unsafe void SquishCompressImage(byte* rgba, int width, int height, byte* blocks, int flags);

      [DllImport("Squish_x86_SSE2.dll")]
      internal static extern unsafe void SquishDecompressImage(byte* rgba, int width, int height, byte* blocks, int flags);

      [DllImport("Squish_x86_SSE2.dll")]
      internal static extern void SquishInitialize();
    }

    private sealed class SquishInterface_64 {
      [DllImport("Squish_x64.dll")]
      internal static extern unsafe void SquishCompressImage(byte* rgba, int width, int height, byte* blocks, int flags);

      [DllImport("Squish_x64.dll")]
      internal static extern unsafe void SquishDecompressImage(byte* rgba, int width, int height, byte* blocks, int flags);

      [DllImport("Squish_x64.dll")]
      internal static extern void SquishInitialize();
    }

    private static unsafe void CallCompressImage(byte[] rgba, int width, int height, byte[] blocks, int flags) {
      fixed (byte* pRGBA = rgba) {
        fixed (byte* pBlocks = blocks) {
          if (Is64Bit())
            SquishInterface_64.SquishCompressImage(pRGBA, width, height, pBlocks, flags);
          else
            SquishInterface_32.SquishCompressImage(pRGBA, width, height, pBlocks, flags);
        }
      }
    }

    private static unsafe void CallDecompressImage(byte[] rgba, int width, int height, byte[] blocks, int flags) {
      fixed (byte* pRGBA = rgba) {
        fixed (byte* pBlocks = blocks) {
          if (Is64Bit())
            SquishInterface_64.SquishDecompressImage(pRGBA, width, height, pBlocks, flags);
          else
            SquishInterface_32.SquishDecompressImage(pRGBA, width, height, pBlocks, flags);
        }
      }
    }

    public static void Initialize() {
      if (Is64Bit()) {
        SquishInterface_64.SquishInitialize();
      }
      else {
        SquishInterface_32.SquishInitialize();
      }
    }

    /// <summary>
    /// Compresses an image
    /// </summary>
    /// <param name="inputSurface">Source byte array containing RGBA pixel data</param>
    /// <param name="squishFlags">Flags for squish compression control</param>
    /// <param name="progressFn">Array of bytes containing compressed blocks</param>
    /// <returns>Byte array of compressed image data.</returns>
    internal static byte[] CompressImage(Bitmap Image, int Flags, ProgressFn Progress) {
      // We need the input to be in a byte array for squish.. so create one.
      byte[] pixelData = new byte[Image.Width * Image.Height * 4];

      //Rectangle rect = new Rectangle(0, 0, Image.Width, Image.Height);
      //BitmapData bmpData = Image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

      for (int y = 0; y < Image.Height; y++) {
        for (int x = 0; x < Image.Width; x++) {
          int pixelOffset = (y * Image.Width * 4) + (x * 4);

          pixelData[pixelOffset + 0] = Image.GetPixel(x, y).R;
          pixelData[pixelOffset + 1] = Image.GetPixel(x, y).G;
          pixelData[pixelOffset + 2] = Image.GetPixel(x, y).B;
          pixelData[pixelOffset + 3] = Image.GetPixel(x, y).A;
        }
      }

      // Compute size of compressed block area, and allocate 
      int blockCount = ((Image.Width + 3) / 4) * ((Image.Height + 3) / 4);
      int blockSize = ((Flags & (int)SquishFlags.Dxt1) != 0) ? 8 : 16;

      // Allocate room for compressed blocks
      byte[] blockData = new byte[blockCount * blockSize];

      // Invoke squish::CompressImage() with the required parameters
      CallCompressImage(pixelData, Image.Width, Image.Height, blockData, Flags);

      // Return our block data to caller..
      return blockData;
    }

    /// <summary>
    /// Decompress an image
    /// </summary>
    /// <param name="blocks">Source byte array containing DXT block data</param>
    /// <param name="width">Width of image in pixels</param>
    /// <param name="height">Height of image in pixels</param>
    /// <param name="flags">Flags for squish decompression control</param>
    /// <returns>Array of bytes containing decompressed blocks</returns>
    internal static Bitmap DecompressImage(byte[] blocks, int width, int height, int flags) {
      byte[] pixelOutput = new byte[width * height * 4];
      CallDecompressImage(pixelOutput, width, height, blocks, flags);
      byte[] adjustedOutput = new byte[pixelOutput.Length];
      for (int i = 0; i < pixelOutput.Length; i += 4) {
        adjustedOutput[i] = pixelOutput[i + 2];
        adjustedOutput[i + 1] = pixelOutput[i + 1];
        adjustedOutput[i + 2] = pixelOutput[i];
        adjustedOutput[i + 3] = pixelOutput[i + 3];
      }

      Bitmap bmp;
      unsafe {
        fixed (Byte* pBytes = adjustedOutput) {
          IntPtr intPtr = new IntPtr((void*)pBytes);
          bmp = new Bitmap(width, height, width * 4, PixelFormat.Format32bppArgb, intPtr);
        }
      }
      return bmp;
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