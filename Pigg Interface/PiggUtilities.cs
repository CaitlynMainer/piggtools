using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;

namespace ParagonForge.PiggInterface {
  /// <summary>
  /// Represents various utilities used to work with pigg files and objects.
  /// </summary>
  public static class PiggUtilities {
    private const string live_regkey_path = @"Software\Cryptic\Coh";
    private const long TicksAtZero = 0x089F7FF5F7B58000;

    /// <summary>
    /// Converts a Unix epoch time (time since midnight, January 1, 1970 UTC)
    /// to a DateTime ticks value (hundreds of nanoseconds since midnight,
    /// January 1, 0001).
    /// </summary>
    /// <param name="Epoch">Unix epoch time to convert.</param>
    /// <returns>Time converted to ticks since midnight, January 1,
    /// 0001.</returns>
    public static long EpochToTicks(long Epoch) {
      return Epoch * 10000000L + 0x089F7FF5F7B58000L;
    }

    /// <summary>
    /// Converts a DateTime ticks value (hundreds of nanoseconds since
    /// midnight, January 1, 0001) to a Unix epoch time (time since midnight,
    /// January 1, 1970 UTC).
    /// </summary>
    /// <param name="Ticks">DateTime ticks value to convert.</param>
    /// <returns>Time converted to seconds since midnight, January 1,
    /// 1970.</returns>
    public static long TicksToEpoch(long Ticks) {
      return (Ticks - 0x089F7FF5F7B58000) / 10000000L;
    }
  }
}
