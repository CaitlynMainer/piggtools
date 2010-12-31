using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Globalization;
using System.Drawing;
using Microsoft.Win32;

namespace ParagonForge {
  /// <summary>
  /// The type of installation this interface represents.
  /// </summary>
  /// <seealso cref="CohInfo.Installation" />
  public enum InstallationType {
    /// <summary>An instance of the live copy of the game.</summary>
    Live = 0,
    /// <summary>An instance of the test copy of the game.</summary>
    Test = 1
  };

  /// <summary>
  /// The mechanism used to render game audio.
  /// </summary>
  /// <seealso cref="CohInfo.AudioRender" />
  public enum AudioType {
    /// <summary>Use the hardware (sound card) to render audio.</summary>
    Hardware = 0,
    /// <summary>Use software to render audio.</summary>
    Software = 1
  }

  /// <summary>
  /// Whether the game should be displayed in fullscreen or windowed mode.
  /// </summary>
  public enum DisplayModeType {
    /// <summary>Display the game in fullscreen mode.</summary>
    Fullscreen = 0,
    /// <summary>Display the game in windowed mode.</summary>
    Windowed = 1
  }

  /// <summary>
  /// Represents the configuration of an installation of City of Heroes.
  /// </summary>
  public class CohInfo {
    InstallationType m_installation;
    private const string live_key_path = @"Software\Cryptic\Coh";
    private const string test_key_path = @"Software\Cryptic\CohTest";
    private RegistryKey m_coh_key;

    # region Constructors
    /// <summary>
    /// Creates a default instance of a pigg information class.
    /// </summary>
    public CohInfo() {
      Initialize(InstallationType.Live);
    }

    /// <summary>
    /// Creates a new City of Heroes configuration object based on the
    /// installation type specified.
    /// </summary>
    /// <param name="Installation">The type of installation (live or test)
    /// that this object represents.</param>
    public CohInfo(InstallationType Installation) {
      Initialize(Installation);
    }

    /// <summary>
    /// Initializes a new City of Heores configuration object based on the
    /// installation type specified.
    /// </summary>
    /// <param name="Installation">The type of installation (live or test)
    /// that this object represents.</param>
    private void Initialize(InstallationType Installation) {
      m_installation = Installation;
      m_coh_key = this.CohRegistryKey;
    }
    #endregion

    # region Member Functions
    /// <summary>
    /// Retrieves a setting from the registry.
    /// </summary>
    /// <param name="Setting">The name of the setting to retrieve.</param>
    /// <param name="Type">The type of the setting.</param>
    /// <returns></returns>
    private object GetValue(string Setting, RegistryValueKind Type) {
      if (this.CohRegistryKey != null) {
        object result = CohRegistryKey.GetValue(Setting);
        if (result != null) {
          return CohRegistryKey.GetValueKind(Setting) == Type
            ? result : null;
        }
      }
      return null;
    }

    /// <summary>
    /// Reads a setting as a string and converts to a boolean value.
    /// </summary>
    /// <param name="Setting">The setting to read from the City of Heroes
    /// registry key.</param>
    /// <returns>false if the setting does not exist, is "0", or is a blank
    /// string; true if it is anything else.</returns>
    private bool ReadBoolean(string Setting) {
      object value = GetValue(Setting, RegistryValueKind.String);
      if (value == null) { return false; }
      else {
        if (value.ToString() == "0" || value.ToString().Trim() == "") {
          return false;
        }
        else { return true; }
      }
    }

    /// <summary>
    /// Reads a setting as a string and converts to a float value.
    /// </summary>
    /// <param name="Setting">The setting to read from the City of Heroes
    /// registry key.</param>
    /// <returns>Zero if the setting does not exist or is an invalid string,
    /// or the value of the setting otherwise.</returns>
    private double ReadDouble(string Setting) {
      object value = CohRegistryKey.GetValue(Setting);
      double result = 0F;
      if (value != null) {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            try {
              result = double.Parse(value.ToString());
            }
            catch (FormatException) {
              // We can't parse the floating point value; leave result set to 0F.
            }
            break;
          case RegistryValueKind.DWord:
            result = (double)((int)value);
            break;
          case RegistryValueKind.QWord:
            result = (double)((long)value);
            break;
          default:
            // We won't process other registry setting types.
            break;
        }
      }
      return result;
    }

    /// <summary>
    /// Reads a setting as a string and converts to an integer value.
    /// </summary>
    /// <param name="Setting">The setting to read from the City of Heroes
    /// registry key.</param>
    /// <returns>Zero if the setting does not exist or is an invalid string,
    /// or the value of the setting otherwise.</returns>
    private int ReadInteger(string Setting) {
      object value = CohRegistryKey.GetValue(Setting);
      int result = 0;  // If we can't get a legal value, return zero.
      if (value != null) {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            try {
              result = int.Parse(value.ToString());
            }
            catch (FormatException) {
              // We can't parse the floating point value; leave result set
              // to 0.
            }
            return result;
          case RegistryValueKind.DWord:
            // No need to sanity check, DWORDs are integers.
            result = (int)value;
            break;
          case RegistryValueKind.QWord:
            if ((long)value >= int.MinValue && (long)value <= int.MaxValue) {
              result = (int)value;
            }
            break;
          default:
            // We're trying to read something that either I can't or I don't
            // want to bother handling.
            break;
        }
      }
      return result;
    }

    /// <summary>
    /// Reads a setting and converts to an IP address if possible.
    /// </summary>
    /// <param name="Setting">The setting to read from the City of Heroes
    /// registry key.</param>
    /// <returns>Null if the setting cannot be read or converted to an IP
    /// address, or the value of the setting otherwise.</returns>
    private IPAddress ReadIPAddress(string Setting) {
      object value = CohRegistryKey.GetValue(Setting);
      IPAddress address = null;
      if (value != null) {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            try {
              address = IPAddress.Parse((string)value);
            }
            catch (FormatException) {
              // We can't parse the IP address from the string, so leave the
              // result set to null.
            }
            break;
          case RegistryValueKind.Binary:
            address = new IPAddress((byte[])value);
            break;
          default:
            break;
        }
      }
      return address;
    }

    /// <summary>
    /// Reads a setting as a string and converts to a Uri value.
    /// </summary>
    /// <param name="Setting">The setting to read from the City of Heroes
    /// registry key.</param>
    /// <returns>Null if the setting cannot be read or converted to a Uri, or
    /// the value of the setting otherwise.</returns>
    private Uri ReadUri(string Setting) {
      object value = CohRegistryKey.GetValue(Setting);
      Uri result = null;
      if (value != null) {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            try {
              result = new Uri(value.ToString());
            }
            catch (UriFormatException) {
              // We can't parse the Uri from the string, so leave the result
              // to null.
            }
            break;
          default:
            // Don't do anything, leave the result as null.
            break;
        }
      }
      return result;
    }

    /// <summary>
    /// Reads a setting as a string, parses it, and converts it to a Version
    /// value.
    /// </summary>
    /// <param name="Setting">The setting to read from the City of Heroes
    /// registry key.</param>
    /// <returns>Null if the setting cannot be read or converted to a Version,
    /// or the value of the setting otherwise.</returns>
    /// <remarks>The version is parsed according to the format of the version
    /// that has been used in the latest versions of the client.</remarks>
    public Version ReadVersion(string Setting) {
      object value = CohRegistryKey.GetValue(Setting);
      Version result = null;
      if (value != null) {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            int major = 0, minor = 0, build = 0, revision = 0;
            Regex r = new Regex(@"^(\d+)\.(\d+)(?:\.(\d+)T?(\d+)?)?$");
            Match m = r.Match((string)value);
            if (m.Success) {
              if (m.Groups.Count > 1) {
                if (m.Groups[1].Value != "") {
                  // We've matched these values against numeric digits in the
                  // regular expression, so we know that it will always parse
                  // as an integer.  (No try/catch needed!)
                  major = int.Parse(m.Groups[1].Value);
                }
              }
              if (m.Groups.Count > 2) {
                if (m.Groups[2].Value != "") {
                  minor = int.Parse(m.Groups[2].Value);
                }
              }
              if (m.Groups.Count > 3) {
                if (m.Groups[3].Value != "") {
                  build = int.Parse(m.Groups[3].Value);
                }
              }
              if (m.Groups.Count > 4) {
                if (m.Groups[4].Value != "") {
                  revision = int.Parse(m.Groups[4].Value);
                }
              }
              result = new Version(major, minor, build, revision);
            }
            break;
          default:
            // Don't do anything, leave the result as null.
            break;
        }
      }
      return result;
    }

    /// <summary>
    /// Reads a setting as a string and returns it.
    /// </summary>
    /// <param name="Setting">The setting to read from the City of Heroes
    /// registry key.</param>
    /// <returns>A blank string if the setting is invalid, or the value of the
    /// setting otherwise.</returns>
    /// <remarks>"Why have a function that reads a string and returns it
    /// instead of just reading it directly?" you ask?  Two reasons.  First,
    /// GetValue returns an object and this encapsulates it to make it easier
    /// to quickly snag the string without having to cast it.  Second, it can
    /// check for null values (i.e. non-existent settings) and return a blank
    /// string instead.</remarks>
    private string ReadString(string Setting) {
      object value = CohRegistryKey.GetValue(Setting);
      string result = null;
      if (value != null) {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
          case RegistryValueKind.DWord:
          case RegistryValueKind.QWord:
          case RegistryValueKind.ExpandString:
            result = value.ToString();
            break;
          case RegistryValueKind.Binary:
            result = System.Text.Encoding.UTF8.GetString((byte[])value);
            break;
          default:
            // We have something that we can't convert directly into a string.
            break;
        }
      }
      return result;
    }

    /// <summary>
    /// Writes a boolean value to the registry.
    /// </summary>
    /// <param name="Setting">The setting to write to the City of Heroes
    /// registry key.</param>
    /// <param name="Value">The value to write to the setting.</param>
    /// <returns>True if the write was successful, false if it failed.
    /// </returns>
    /// <remarks>Booleans are converted to strings as "0" for false and "1"
    /// for true.  They are converted to integral types (DWord and QWord) as
    /// 0 for false and 1 for true.</remarks>
    private bool WriteBoolean(string Setting, bool Value) {
      object value = CohRegistryKey.GetValue(Setting);
      if (value == null) {
        CohRegistryKey.SetValue(Setting, Value ? "1" : "0");
        return true;
      }
      else {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            CohRegistryKey.SetValue(Setting, Value ? "1" : "0");
            return true;
          case RegistryValueKind.DWord:
            CohRegistryKey.SetValue(Setting, Value ? 1 : 0,
              RegistryValueKind.DWord);
            return true;
          case RegistryValueKind.QWord:
            CohRegistryKey.SetValue(Setting, Value ? 1 : 0,
              RegistryValueKind.QWord);
            return true;
          default:
            return false;
        }
      }
    }

    /// <summary>
    /// Writes an integer to the registry.
    /// </summary>
    /// <param name="Setting">The setting to write to the City of Heroes
    /// registry key.</param>
    /// <param name="Value">The value to write to the setting.</param>
    /// <returns>True if the write was successful, false if it failed.
    /// </returns>
    /// <remarks>Integers can be written to almost any registry setting type.
    /// If needed, it is converted to a string or QWord.  If the type is of a
    /// kind that doesn't support integers, it will fail and return false.
    /// </remarks>
    private bool WriteInteger(string Setting, int Value) {
      object value = CohRegistryKey.GetValue(Setting);
      if (value == null) {
        CohRegistryKey.SetValue(Setting, Value.ToString());
        return true;
      }
      else {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            CohRegistryKey.SetValue(Setting, Value.ToString());
            return true;
          case RegistryValueKind.DWord:
            CohRegistryKey.SetValue(Setting, Value,
              RegistryValueKind.DWord);
            return true;
          case RegistryValueKind.QWord:
            CohRegistryKey.SetValue(Setting, Value,
              RegistryValueKind.QWord);
            return true;
          default:
            return false;
        }
      }
    }

    /// <summary>
    /// Writes an IP address to the registry.
    /// </summary>
    /// <param name="Setting">The setting to write to the City of Heroes
    /// registry key.</param>
    /// <param name="Value">The value to write to the setting.</param>
    /// <returns>True if the write was successful, false if it failed.
    /// </returns>
    /// <remarks>IP addresses are converted to strings or longs as
    /// appropriate.</remarks>
    private bool WriteIPAddress(string Setting, IPAddress Value) {
      object value = CohRegistryKey.GetValue(Setting);
      if (value == null) {
        CohRegistryKey.SetValue(Setting, Value.ToString());
        return true;
      }
      else {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            CohRegistryKey.SetValue(Setting, Value.ToString());
            return true;
          case RegistryValueKind.DWord:
            if (Value.GetAddressBytes().Length <= 4) {
              int address = 0;
              for (int i = 0; i < 4; i++) {
                address = address << 8 + Value.GetAddressBytes()[3 - i];
              }
              CohRegistryKey.SetValue(Setting, address,
                RegistryValueKind.DWord);
              return true;
            }
            else {
              // We can't store addresses longer than four bytes in a DWord.
              return false;
            }
          case RegistryValueKind.QWord:
            if (Value.GetAddressBytes().Length <= 8) {
              long address = 0;
              for (int i = 0; i < 8; i++) {
                address = address << 8 + Value.GetAddressBytes()[7 - i];
              }
              CohRegistryKey.SetValue(Setting, address,
                RegistryValueKind.QWord);
              return true;
            }
            else {
              // We can't store addresses longer than eight bytes in a QWord.
              return false;
            }
          case RegistryValueKind.Binary:
            CohRegistryKey.SetValue(Setting, Value.GetAddressBytes(),
              RegistryValueKind.Binary);
            return true;
          default:
            return false;
        }
      }
    }

    /// <summary>
    /// Writes a Uri to the registry.
    /// </summary>
    /// <param name="Setting">The setting to write to the City of Heroes
    /// registry key.</param>
    /// <param name="Value">The value to write to the setting.</param>
    /// <returns>True if the write was successful, false if it failed.
    /// </returns>
    public bool WriteUri(string Setting, Uri Value) {
      if (Value != null) {
        return WriteString(Setting, Value.AbsoluteUri);
      }
      else { return false; }
    }

    /// <summary>
    /// Writes a double to the registry.
    /// </summary>
    /// <param name="Setting">The setting to write to the City of Heroes
    /// registry key.</param>
    /// <param name="Value">The value to write to the setting.</param>
    /// <returns>True if the write was successful, false if it failed.
    /// </returns>
    /// <remarks>Doubles are written with precision of six decimal places,
    /// which is the City of Heroes registry settings standard.  If we try to
    /// write a double to an existing integral setting (DWord or QWord), we'll
    /// try to convert it.  If it is in range of that type, we'll truncate and
    /// save it.</remarks>
    private bool WriteDouble(string Setting, double Value) {
      object value = CohRegistryKey.GetValue(Setting);
      if (value == null) {
        CohRegistryKey.SetValue(Setting, Value.ToString("F6"));
        return true;
      }
      else {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            CohRegistryKey.SetValue(Setting, Value.ToString("F6"));
            return true;
          case RegistryValueKind.DWord :
            if (Value >= (double)int.MinValue &&
              Value <= (double)int.MaxValue) {
              CohRegistryKey.SetValue(Setting,
                (int)(Decimal.Round((Decimal)Value)),
                RegistryValueKind.DWord);
              return true;
            }
            else { return false; }
          case RegistryValueKind.QWord:
            if (Value >= (double)long.MinValue &&
              Value <= (double)long.MaxValue) {
              CohRegistryKey.SetValue(Setting,
                (long)(Decimal.Round((Decimal)Value)),
                RegistryValueKind.QWord);
              return true;
            }
            else { return false; }
          default:
            return false;
        }
      }
    }

    /// <summary>
    /// Writes a string to the registry.
    /// </summary>
    /// <param name="Setting">The setting to write to the City of Heroes
    /// registry key.</param>
    /// <param name="Value">The value to write to the setting.</param>
    /// <returns>True if the write was successful, false if it failed.
    /// </returns>
    /// <remarks>This function may fail if a string is attempted to be written
    /// to a setting that already exists and is not a string type.  In such a
    /// case, we'll try to convert it, but if we can't, we will not overwrite
    /// a setting and return false.</remarks>
    private bool WriteString(string Setting, string Value) {
      object value = CohRegistryKey.GetValue(Setting);
      if (value == null) {
        CohRegistryKey.SetValue(Setting, Value, RegistryValueKind.String);
        return true;
      }
      else {
        // We're being asked to write a string value to a registry setting
        // that is not a string.  We'll make an effort to convert it, but if
        // we can't we're not going to overwrite it, we'll just return false.
        bool converted = false;
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            // Stright-forward enough.
            CohRegistryKey.SetValue(Setting, Value);
            return true;
          case RegistryValueKind.DWord:
            // We're trying to write a string value to an integer setting.
            // Let's try to convert it!
            int converted_int = 0;
            try {
              converted_int = int.Parse(Value);
              converted = true;  // Success!
            }
            catch (FormatException) {
              // Don't do anything, just let it fall through.
            }
            if (converted) {
              // Now we have a DWord that we can save.
              CohRegistryKey.SetValue(Setting, converted_int,
                RegistryValueKind.DWord);
              return true;
            }
            else {
              // If we couldn't convert the string to an integer, we're not
              // going to be nasty and overwrite stuff in the registry.  Just
              // bail and let the caller know.
              return false;
            }
          case RegistryValueKind.QWord:
            // We're trying to write a string value to a long integer setting.
            // Let's try to convert it!
            long converted_long = 0;
            try {
              converted_long = long.Parse(Value);
              converted = true;  // Success!
            }
            catch (FormatException) {
              // Don't do anything, just let it fall through.
            }
            if (converted) {
              // Now we have a DWord that we can save.
              CohRegistryKey.SetValue(Setting, converted_long,
                RegistryValueKind.QWord);
              return true;
            }
            else {
              // If we couldn't convert the string to an integer, we're not
              // going to be nasty and overwrite stuff in the registry.  Just
              // bail and let the caller know.
              return false;
            }
          case RegistryValueKind.Binary:
            CohRegistryKey.SetValue(Setting,
              System.Text.Encoding.UTF8.GetBytes(Value.ToCharArray()),
              RegistryValueKind.Binary);
            return true;
          default:
            // We don't know how to convert this thing.
            return false;
        }
      }
    }

    /// <summary>
    /// Writes a version structure to the registry.
    /// </summary>
    /// <param name="Setting">The setting to write to the City of Heroes
    /// registry key.</param>
    /// <param name="Value">The value to write to the setting.</param>
    /// <returns>True if the write was successful, false if it failed.
    /// </returns>
    private bool WriteVersion(string Setting, Version Value) {
      object value = CohRegistryKey.GetValue(Setting);
      if (value == null) {
        CohRegistryKey.SetValue(Setting, VersionToString(Value));
        return true;
      }
      else {
        switch (CohRegistryKey.GetValueKind(Setting)) {
          case RegistryValueKind.String:
            string version = VersionToString(Value);
            if (version == "") { return false; }
            else {
              CohRegistryKey.SetValue(Setting, version);
              return true;
            }
          default:
            return false;
        }
      }
    }

    /// <summary>
    /// Returns a DateTime structure that represents the date of a City of
    /// Heroes client version.
    /// </summary>
    /// <param name="Value">The version to parse for a date.</param>
    /// <returns>A DateTime structure representing the date of the version, or
    /// null if the version number is unable to be parsed for a date.
    /// </returns>
    public static DateTime? VersionToDate(Version Value) {
      Match m = Regex.Match(Value.Minor.ToString(),
        @"^(\d{4}){\d{2})(\d{2})$");
      if (m.Success) {
        int year = int.Parse(m.Groups[1].Value);
        int month = int.Parse(m.Groups[2].Value);
        int day = int.Parse(m.Groups[3].Value);
        if (year >= 1 && year <= 9999) {
          if (month >= 1 && month <= 12) {
            if (day >= 1 && day <= DateTime.DaysInMonth(year, month)) {
              return new DateTime(year, month, day);
            }
          }
        }
      }
      return null;
    }

    /// <summary>
    /// Converts a Version structure to a City of Heroes version string.
    /// </summary>
    /// <param name="Value">The Version structure to convert.</param>
    /// <returns>A City of Heroes version string.</returns>
    private string VersionToString(Version Value) {
      // If the version isn't set, return a blank string.
      if (Value == null) { return ""; }
      if (Value.Major == 0 && Value.Minor == 0 && Value.Build == 0 &&
        Value.Revision == 0) { return ""; }

      string result = Value.Major.ToString() + ".";
      result += Value.Minor.ToString() + ".";

      // In actual version numbers without a revision, sometimes the T is left
      // off the end, but in general, it is included, so we'll do so here.
      result += Value.Build.ToString() + "T";

      // When the revision number is zero, it is left off.
      if (Value.Revision > 0) { result += Value.Revision.ToString(); }

      return result;
    }

    /// <summary>
    /// Sets a setting in the registry.
    /// </summary>
    /// <param name="Setting">Setting to set.</param>
    /// <param name="Value">Value to which the setting will be set.</param>
    /// <remarks>In this version of the SetValue function, the setting will
    /// be set to a string value.</remarks>
    private void SetValue(string Setting, string Value) {
      SetValue(Setting, Value.ToString(), RegistryValueKind.String);
    }

    /// <summary>
    /// Sets a setting in the registry.
    /// </summary>
    /// <param name="Setting">Setting to set.</param>
    /// <param name="Value">Value to which the setting will be set.</param>
    /// <param name="Type">Type of the value that will be set.</param>
    private void SetValue(string Setting, object Value,
      RegistryValueKind Type) {
      if (this.CohRegistryKey != null) {
        CohRegistryKey.SetValue(Setting, Value, Type);
      }
    }

    /// <summary>
    /// Converts the CohInfo object to a listing of all game settings.
    /// </summary>
    /// <returns>A human-readable listing of the City of Heroes game settings.
    /// </returns>
    public override string ToString() {
      string msg = "";
      msg += "Client version: ";
      if (CurrentVersion == null) { msg += "(invalid)\r\n"; }
      else {
        msg += CurrentVersion.ToString() + "\r\n";
      }
      msg += "Account name: ";
      if (SaveAccountName == true) {
        if (AccountName == "") { msg += "(blank)\r\n"; }
        else { msg += AccountName + "\r\n"; }
      }
      else {
        msg += "(not saved)\r\n";
      }
      msg += "Authorization server: ";
      if (AuthenticationServer == null) { msg += "(invalid)\r\n"; }
      else { msg += AuthenticationServer.ToString() + "\r\n"; }

      msg += "Database server: ";
      if (DatabaseServer == "") { msg += "(blank)\r\n"; }
      else { msg += DatabaseServer.ToString() + "\r\n"; }

      msg += "\r\nGraphics Settings:\r\n";
      msg += "Ageia: " + (AgeiaEnabled ? "En" : "Dis") + "abled\r\n";
      msg += "Bloom: ";
      if (BloomEnabled) {
        msg += BloomMagnitude.ToString("F6") + "\r\n";
      }
      else { msg += "Disabled\r\n"; }

      msg += "Depth of Field: ";
      if (DepthOfFieldEnabled) {
        msg += DepthOfFieldMagnitude.ToString("F6") + "\r\n";
      }
      else { msg += "Disabled\r\n"; }

      msg += "Gamma: ";
      if (Gamma > 0) { msg += (Gamma * 100).ToString("F2") + "%\r\n"; }
      else { msg += "(invalid)\r\n"; }

      msg += "Character MIP level: " + CharacterMipLevel.ToString() + "\r\n";

      return msg;
    }
    #endregion

    #region Properties
    /// <summary>
    /// Registry key for the installation type.  If installation type does
    /// not exist in the registry, returns null.
    /// </summary>
    private RegistryKey CohRegistryKey {
      get {
        // TODO: To be nice, only open the registry for write mode access if we're actually going to write to it.
        if (m_coh_key == null) {
          switch (this.Installation) {
            case InstallationType.Live:
              m_coh_key = Registry.CurrentUser.OpenSubKey(live_key_path, true);
              break;
            case InstallationType.Test:
              m_coh_key = Registry.CurrentUser.OpenSubKey(test_key_path, true);
              break;
            default:  // This should never fall through, but just in case...
              // Default to the live setting.
              m_coh_key = Registry.CurrentUser.OpenSubKey(live_key_path, true);
              break;
          }
        }
        return m_coh_key;
      }
    }
    /// <summary>
    /// The type of installation pigg info object represents.
    /// </summary>
    public InstallationType Installation {
      get { return m_installation; }
      set { m_installation = value; }
    }

    /// <summary>
    /// The default account name that will be presented to the user upon
    /// launch of the City of Heroes client.
    /// </summary>
    public string AccountName {
      get { return ReadString("accountName"); }
      set { WriteString("accountName", value); }
    }

    /// <summary>
    /// Whether or not the Ageia functionality is enabled.
    /// </summary>
    public bool AgeiaEnabled {
      get { return ReadBoolean("ageiaOn"); }
      set { WriteBoolean("ageiaOn", value); }
    }

    /// <summary>
    /// The anti-aliasing setting.
    /// </summary>
    public int AntiAlias {
      get { return ReadInteger("antiAliasing"); }
      set { WriteInteger("antiAliasing", value); }
    }

    /// <summary>
    /// The IP address of the authentication server.
    /// </summary>
    public IPAddress AuthenticationServer {
      get { return ReadIPAddress("Auth"); }
      set { WriteIPAddress("Auth", value); }
    }

    /// <summary>
    /// The bloom magnitude setting.
    /// </summary>
    public double BloomMagnitude {
      get { return ReadDouble("bloomMagnitude"); }
      set { WriteDouble("bloomMagnitude", value); }
    }

    /// <summary>
    /// The character mip level setting.
    /// </summary>
    public int CharacterMipLevel {
      get { return ReadInteger("characterMipLevel"); }
      set { WriteInteger("characterMipLevel", value); }
    }

    /// <summary>
    /// The current version of the City of Heroes client, or null if the
    /// current version cannot be determined.
    /// </summary>
    /// <remarks>
    ///   The version number is currently given in the format:
    ///     (major).(build_date).(minor)T(revision)
    /// 
    ///   At various times in the past, the minor version number and/or the
    ///   revision have been omitted.  If that is the case, this property
    ///   reports any omitted components of the version number as zero (0).
    /// 
    ///   At the very least in order to return a valid version number the
    ///   format will need to be (major).(minor).
    /// </remarks>
    public Version CurrentVersion {
      get {
        int major = 0, minor = 0, build = 0, revision = 0;
        string ver = (string)GetValue("CurrentVersion",
          RegistryValueKind.String);
        Regex r = new Regex(@"^(\d+)\.(\d+)(?:\.(\d+)T?(\d+)?)?$");
        Match m = r.Match(ver);
        if (m.Success) {
          major = int.Parse(m.Groups[1].Value);
          minor = int.Parse(m.Groups[2].Value);
          if (m.Groups.Count > 3) {
            if (m.Groups[3].Value != "") {
              build = int.Parse(m.Groups[3].Value);
            }
          }
          if (m.Groups.Count > 4) {
            if (m.Groups[4].Value != "") {
              revision = int.Parse(m.Groups[4].Value);
            }
          }
          return new Version(major, minor, build, revision);
        }
        else { return null; }
      }
      set {
        string result = "";

        if (value.Revision > 0) {
          result = value.Revision.ToString();
          if (value.Minor > 0 || value.Major > 0 || value.Build > 0) {
            result = "T" + result;
          }
        }
        if (value.Build > 0) {
          result = value.Build.ToString() + result;
          if (value.Minor > 0 || value.Major > 0) {
            result = "." + result;
          }
        }
        if (value.Minor > 0) {
          result = value.Minor.ToString() + result;
          if (value.Major > 0) {
            result = "." + result;
          }
        }
        if (value.Major > 0) {
          result = value.Major.ToString() + result;
        }
        if (result != "") { WriteString("CurrentVersion", result); }
      }
    }

    /// <summary>
    /// The database server used by the City of Heroes client.  (Currently,
    /// this value is always blank.)
    /// </summary>
    /// <remarks>I am guessing that the devs can set this value manually in
    /// order to run the client against alternate builds of the server.
    /// I suspect (but can't prove) that unless you have your own CoH server
    /// up and running, this value should always be left to a blank string.
    /// I'd also guess that this should be an IP address, but since I can't
    /// tell without it actually being populated, I'm not going to assume
    /// that.</remarks>
    public string DatabaseServer {
      get { return ReadString("DbServer"); }
      set { WriteString("DbServer", value); }
    }

    /// <summary>
    /// Whether or not in-game ads on posters and billboards are disabled.
    /// </summary>
    public bool AdsEnabled {
      get { return !ReadBoolean("DisableAds"); }
      set { WriteBoolean("DisableAds", !value); }
    }

    /// <summary>
    /// The depth of field magnitude setting.
    /// </summary>
    public double DepthOfFieldMagnitude {
      get { return ReadDouble("dofMagnitude"); }
      set { WriteDouble("dofMagnitude", value); }
    }

    /// <summary>
    /// Whether or not the account name is automatically populated when the
    /// City of Heroes client launches.
    /// </summary>
    /// <remarks>Note that the registry entry indicates whether the account
    /// name should NOT be remembered, but this property is an affirmative
    /// one (DO remember it).  Thus the 0 = true, 1 = false logic in this
    /// property.)</remarks>
    public bool SaveAccountName {
      get { return !ReadBoolean("dontSaveName"); }
      set { WriteBoolean("dontSaveName", !value); }
    }

    /// <summary>
    /// Whether 3D sound is enabled in the client.
    /// </summary>
    public bool ThreeDSoundEnabled {
      get { return ReadBoolean("enable3DSound"); }
      set { WriteBoolean("enable3DSound", value); }
    }

    /// <summary>
    /// Whether OpenGL vertex buffer objects are enabled in the client.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Vertex_Buffer_Object</remarks>
    public bool VertexBufferObjectsEnabled {
      get { return ReadBoolean("enableVBOs"); }
      set { WriteBoolean("enableVBOs", value); }
    }

    /// <summary>
    /// The entitiy detail level setting.
    /// </summary>
    public double EntityDetailLevel {
      get { return ReadDouble("entityDetailLevel"); }
      set { WriteDouble("entityDetailLevel", value); }
    }

    /// <summary>
    /// Whether or not compatible cursors are enabled.
    /// </summary>
    /// <remarks>Note that the registry entry indicates whether compatible
    /// cursors are NOT enabled, but this property is an affirmative one
    /// (compatible cursors ARE enabled).  Thus the 0 = true, 1 = false logic
    /// in this property.)</remarks>
    public bool CompatibleCursors {
      get { return !ReadBoolean("fancyMouseCursor"); }
      set { WriteBoolean("fancyMouseCursor", !value); }
    }

    /// <summary>
    /// Whether the client uses software or hardware audio rendering.
    /// </summary>
    public AudioType AudioRender {
      get {
        bool use_software = ReadBoolean("forceSoftwareAudio");
        if (use_software) { return AudioType.Software; }
        else { return AudioType.Hardware; }
      }
      set {
        WriteBoolean("forceSoftwareAudio", value == AudioType.Software);
      }
    }

    /// <summary>
    /// Whether the client uses software audio rendering.
    /// </summary>
    public bool SoftwareAudio {
      get { return ReadBoolean("forceSoftwareAudio"); }
      set { WriteBoolean("forceSoftwareAudio", value); }
    }

    /// <summary>
    /// Whether the client uses hardware audio rendering.
    /// </summary>
    public bool HardwareAudio {
      get { return !SoftwareAudio; }
      set { WriteBoolean("forceSoftwareAudio", !value); }
    }

    /// <summary>
    /// Whether the game is displayed in fullscreen or windowed mode.
    /// </summary>
    public DisplayModeType DisplayMode {
      get {
        bool fullscreen = ReadBoolean("fullScreen");
        if (fullscreen) { return DisplayModeType.Fullscreen; }
        else { return DisplayModeType.Windowed; }
      }
      set {
        WriteBoolean("fullScreen", value == DisplayModeType.Fullscreen);
      }
    }

    /// <summary>
    /// Whether the game is is displayed in fullscreen mode.
    /// </summary>
    public bool Fullscreen {
      get { return ReadBoolean("fullScreen"); }
      set { WriteBoolean("fullScreen", value); }
    }

    /// <summary>
    /// Whether the game is displayed in windowed mode.
    /// </summary>
    public bool Windowed {
      get { return !Fullscreen; }
      set { WriteBoolean("fullScreen", !value); }
    }

    /// <summary>
    /// The sound effects volume setting.
    /// </summary>
    /// <remarks>The value of the sound effects volume setting is between
    /// 0 and 1.  So for example, if the sound effects volume is set to 27%,
    /// this value will be 0.270000.</remarks>
    public double SoundEffectsVolume {
      get { return ReadDouble("fxSoundVolume"); }
      set {
        if (value < 0.0 || value > 1.0) {
          throw new System.ArgumentOutOfRangeException("SoundEffectsVolume");
        }
        WriteDouble("fxSoundVolume", value);
      }
    }

    /// <summary>
    /// The gamma setting.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Gamma_correction</remarks>
    public double Gamma {
      get { return ReadDouble("gamma"); }
      set { WriteDouble("gamma", value);  }
    }

    /// <summary>
    /// The graphics quality setting.
    /// </summary>
    public double GraphicsQuality {
      get { return ReadDouble("graphicsQuality"); }
      set { WriteDouble("graphicsQuality", value);  }
    }

    /// <summary>
    /// The location where City of Heroes is installed.
    /// </summary>
    /// <remarks>This is the installation directory according to the registry
    /// entry.  This directory should be checked for existence before used.
    /// </remarks>
    public string InstallationDirectory {
      get { return ReadString("Installation Directory"); }
      set { WriteString("Installation Directory", value); }
    }

    /// <summary>
    /// The langauge of the installer.
    /// </summary>
    public CultureInfo InstallerLanguage {
      get {
        int value = ReadInteger("Installer Language");
        if (value > 0) { return CultureInfo.GetCultureInfo(value); }
        else { return null; }
      }
      set {
        throw new System.NotImplementedException(@"It's on my to-do list.");
      }
    }

    /// <summary>
    /// The load tip number to show on the next load screen.
    /// </summary>
    public int LoadTip {
      get { return ReadInteger("LoadintTip"); }
      set { WriteInteger("LoadintTip", value); }
    }

    /// <summary>
    /// The locale of the game.
    /// </summary>
    /// <remarks>I have only played the English-language version of the game,
    /// so this value is always zero to me.  I believe it would change based
    /// on other language versions of the game.</remarks>
    public int Locale {
      // TODO: Research alternate locales to see if there's a better return value for this property.
      get { return ReadInteger("Locale"); }
      set { WriteInteger("Locale", value); }
    }

    /// <summary>
    /// URL of page displayed when a downloadable patch is available.
    /// </summary>
    public Uri PatchURL {
      get { return ReadUri("MajorPatchWebPage"); }
      set { WriteUri("MajorPatchWebPage", value); }
    }

    /// <summary>
    /// Whether the window is maximized in windowed mode.
    /// </summary>
    public bool Maximized {
      get { return ReadBoolean("maximized"); }
      set { WriteBoolean("maximized", value); }
    }

    /// <summary>
    /// The maximum particle fill setting.
    /// </summary>
    public double MaxParticleFill {
      get { return ReadDouble("maxParticleFill"); }
      set { WriteDouble("maxParticleFill", value); }
    }

    /// <summary>
    /// The maximum particle count setting.
    /// </summary>
    public int MaxParticleCount {
      get { return ReadInteger("maxParticles"); }
      set { WriteInteger("maxParticles", value); }
    }

    /// <summary>
    /// The mip level setting.
    /// </summary>
    public int MipLevel {
      get { return ReadInteger("mipLevel"); }
      set { WriteInteger("mipLevel", value); }
    }

    /// <summary>
    /// The music volume setting.
    /// </summary>
    /// <remarks>The value of the music volume setting is between 0 and 1.
    /// So for example, if the music volume is set to 27%, this value will be
    /// 0.270000.</remarks>
    public double MusicVolume {
      get { return ReadDouble("musicSoundVolume"); }
      set {
        if (value < 0.0 || value > 1.0) {
          throw new System.ArgumentOutOfRangeException("MusicVolume");
        }
        WriteDouble("musicSoundVolume", value);
      }
    }

    /// <summary>
    /// The physics quality setting.
    /// </summary>
    public int PhysicsQuality {
      get { return ReadInteger("physicsQuality"); }
      set { WriteInteger("physicsQuality", value); }
    }

    /// <summary>
    /// The refresh rate setting.
    /// </summary>
    public int RefreshRate {
      get { return ReadInteger("refreshRate"); }
      set { WriteInteger("refreshRate", value); }
    }

    /// <summary>
    /// The x-axis rendering scale setting.
    /// </summary>
    public double RenderScaleX {
      get { return ReadDouble("renderScaleX"); }
      set { WriteDouble("renderScaleX", value); }
    }

    /// <summary>
    /// The y-axis rendering scale setting.
    /// </summary>
    public double RenderScaleY {
      get { return ReadDouble("renderScaleY"); }
      set { WriteDouble("renderScaleY", value); }
    }

    /// <summary>
    /// The width of the game client window.
    /// </summary>
    public int Width {
      get { return ReadInteger("screenX"); }
      set { WriteInteger("screenX", value); }
    }

    /// <summary>
    /// The height of the game client window.
    /// </summary>
    public int Height {
      get { return ReadInteger("screenY"); }
      set { WriteInteger("screenY", value); }
    }

    /// <summary>
    /// The (height, width) size of the game client.
    /// </summary>
    public Size Size {
      get { return new Size(Width, Height); }
      set { Width = value.Width; Height = value.Height; }
    }

    /// <summary>
    /// The x-coordinate location of the game client when the game is running
    /// in windowed mode.
    /// </summary>
    public int X {
      get { return ReadInteger("screenX_pos"); }
      set { WriteInteger("screenX_pos", value); }
    }

    /// <summary>
    /// The y-coordinate location of the game client when the game is running
    /// in windowed mode.
    /// </summary>
    public int Y {
      get { return ReadInteger("screenY_pos"); }
      set { WriteInteger("screenY_pos", value); }
    }

    /// <summary>
    /// The (x, y) location of the game client when the game is running in
    /// windowed mode.
    /// </summary>
    public Point Location {
      get {
        return new Point(X, Y);
      }
      set { X = value.X; Y = value.Y; }
    }

    /// <summary>
    /// The shader detail setting.
    /// </summary>
    public int ShaderDetail {
      get { return ReadInteger("shaderDetail"); }
      set { WriteInteger("shaderDetail", value); }
    }

    /// <summary>
    /// Whether shadows will be shown or not.
    /// </summary>
    public bool ShadowsEnabled {
      get { return ReadBoolean("shadowsOn"); }
      set { WriteBoolean("shadowsOn", value); }
    }

    /// <summary>
    /// Whether advanced graphics settings are enabled or not.
    /// </summary>
    public bool AdvancedGraphicsEnabled {
      get { return ReadBoolean("showAdvanced"); }
      set { WriteBoolean("showAdvanced", value); }
    }

    /// <summary>
    /// Whether or not one's avatar's graphical power effects will be shown
    /// when the camera is close to the avatar.
    /// </summary>
    /// <remarks>Note that the registry setting is a negative (suppress) while
    /// this property is a positive (enabled), thus the 0 = true, 1 = false
    /// logic.</remarks>
    public bool CloseEffectsEnabled {
      get { return !ReadBoolean("suppressCloseFx"); }
      set { WriteBoolean("suppressCloseFx", !value); }
    }

    /// <summary>
    /// The distance at which the CloseEffectsEnabled setting will be applied.
    /// A distance of zero (0) suppresses the graphical effects only when in
    /// first-person view mode.
    /// </summary>
    public double CloseEffectsDistance {
      get { return ReadDouble("suppressCloseFxDist"); }
      set { WriteDouble("suppressCloseFxDist", value); }
    }

    /// <summary>
    /// Whether or not to suppress graphical effects of third-party avatars.
    /// </summary>
    /// <remarks>Note that the registry setting is a negative (suppress) while
    /// this property is a positive (enabled), thus the 0 = true, 1 = false
    /// logic.</remarks>
    public bool EffectsEnabled {
      get { return !ReadBoolean("suppressFx"); }
      set { WriteBoolean("suppressFx", !value); }
    }

    /// <summary>
    /// The texAniso setting in the registry.
    /// </summary>
    public int TexAniso {
      // TODO: Provide description and better name of texAniso setting.
      get { return ReadInteger("texAniso"); }
      set { WriteInteger("texAniso", value); }
    }

    /// <summary>
    /// The texLodBias setting in the registry.
    /// </summary>
    public int TexLodBias {
      // TODO: Provide description and better name of texLodBias setting.
      get { return ReadInteger("texLodBias"); }
      set { WriteInteger("texLodBias", value); }
    }

    /// <summary>
    /// The TransferRate setting in the registry.
    /// </summary>
    public int TransferRate {
      // TODO: Provide description and better name of TransferRate setting.
      get { return ReadInteger("TransferRate"); }
      set { WriteInteger("TransferRate", value); }
    }

    /// <summary>
    /// Whether or not bloom effects are enabled.
    /// </summary>
    public bool BloomEnabled {
      get { return ReadBoolean("useBloom"); }
      set { WriteBoolean("useBloom", value); }
    }

    /// <summary>
    /// Whether or not desaturation effects are enabled.
    /// </summary>
    /// <remarks>The desaturation effect is the effect seen when first
    /// entering an Ouroboros mission and when it is completed, when the
    /// screen fades in and out of full color.</remarks>
    public bool DesaturateEnabled {
      get { return ReadBoolean("useDesaturate"); }
      set { WriteBoolean("useDesaturate", value); }
    }

    /// <summary>
    /// Whehter or not depth of field effects are enabled.
    /// </summary>
    /// <remarks>Depth of field effects are the effects seen when game objects
    /// at a distance are blurry.  It is intended to simulate the
    /// three-dimensional of objects closer up being in focus.</remarks>
    public bool DepthOfFieldEnabled {
      get { return ReadBoolean("useDOF"); }
      set { WriteBoolean("useDOF", value); }
    }

    /// <summary>
    /// Whether or not the render scale effects are enabled.
    /// </summary>
    /// <remarks>The render scale properties allow the game to render the
    /// graphics on the x-axis and the y-axis at independent resolutions.
    /// These settings are not available to change via the in-game GUI.
    /// </remarks>
    public bool RenderScaleEnabled {
      get { return ReadBoolean("useRenderScale"); }
      set { WriteBoolean("useRenderScale", value); }
    }

    /// <summary>
    /// Whether or not vertical synchronization is enabled.
    /// </summary>
    /// <remarks>http://en.wikipedia.org/wiki/Vertical_sync</remarks>
    public bool VerticalSyncEnabled {
      get { return ReadBoolean("useVSync"); }
      set { WriteBoolean("useVSync", value); }
    }

    /// <summary>
    /// The quality of water effects.  A value of zero indicates that water
    /// effects are disabled.
    /// </summary>
    public int WaterEffects {
      get { return ReadInteger("useWater"); }
      set { WriteInteger("useWater", value); }
    }

    /// <summary>
    /// URL of page displayed inside the launcher.
    /// </summary>
    public Uri LauncherURL {
      get {
        string value = ReadString("WebPage");
        Uri result = null; ;
        if (value != "") {
          string protocol = "http://";
          Match m = Regex.Match(value, "^(?:" + protocol + ")?(.*)$");
          if (m.Success) {
            result = new Uri(protocol + m.Groups[1].Value);
          }
        }
        return result;
      }
      set { WriteString("WebPage", value.AbsoluteUri); }
    }

    /// <summary>
    /// The world detail level setting.
    /// </summary>
    public double WorldDetail {
      get { return ReadDouble("worldDetailLevel"); }
      set { WriteDouble("worldDetailLevel", value); }
    }

    /// <summary>
    /// Delete me before production.  :-)
    /// </summary>
    public Version Test {
      get { return ReadVersion("_Test"); }
      set { WriteVersion("_Test", value); }
    }

    #endregion

  }
}
