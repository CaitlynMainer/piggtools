using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;

namespace ParagonForge.PiggInterface {
  #region Enumerations
  /// <summary>
  /// Return values used to indicate the status of trying to add a file to
  /// the collection
  /// </summary>
  public enum AddFileResult {
    /// <summary>File was successfully added.</summary>
    OK = 0,
    /// <summary>File was already in the collection and did not need to be
    /// added again.</summary>
    FileExists = 1,
    /// <summary>File was unable to be added.</summary>
    InvalidFile = 2
  };
  #endregion

  /// <summary>
  /// Represents a collection of pigg files.
  /// </summary>
  public class PiggFileCollection {
    private PiggNode m_root;
    private Dictionary<string, PiggFile> m_filelist;
    private CohInfo coh_info;

    #region Constructors
    /// <summary>
    /// Creates a new PiggCollection object with all properties set to null or
    /// default values
    /// </summary>
    public PiggFileCollection() {
      coh_info = new CohInfo(InstallationType.Live);
      string install_directory = coh_info.InstallationDirectory;
      if (install_directory != "") {
        // TODO: Add logic to initialize if no filenames are passed in.
      }
      else { Initialize(); }
    }

    /// <summary>
    /// Creates a new PiggCollection object based on the DirectoryName top
    /// level directory
    /// </summary>
    /// <param name="Filenames">Files or directories to add to the pigg file
    /// collection.</param>
    public PiggFileCollection(string[] Filenames) {
      Initialize();
      if (Filenames != null) {
        foreach (string file in Filenames) {
          if (Directory.Exists(file)) {
            AddDirectory(file, SearchOption.TopDirectoryOnly);
          }
          else if (File.Exists(file)) {
            AddFile(file);
          }
        }
      }
    }

    /// <summary>
    /// Initializes a pigg file collection.
    /// </summary>
    private void Initialize() {
      m_filelist = new Dictionary<string, PiggFile>();
      m_root = new PiggNode();
    }
    #endregion

    #region Member Functions
    /// <summary>
    /// Adds a file to the pigg file collection.
    /// </summary>
    /// <param name="FileName">The name of the pigg file to add to the
    /// collection.</param>
    /// <returns>An AddFileResult that indicates the result of trying to add
    /// the file to the pigg file collection.</returns>
    /// <remarks>The collection of pigg files is contained in a Dictionary
    /// object.  This is so that there will be no duplicate files in the
    /// collection.  Using this version of the AddFile function, if one tries
    /// to add a file that already exists to the collection, it will be
    /// overwritten by default.  If this is not desired, use the version of
    /// AddFile that takes the second Overwrite argument.</remarks>
    public AddFileResult AddFile(string FileName) {
      return AddFile(FileName, true);
    }

    /// <summary>
    /// Adds a file to the pigg file collection.
    /// </summary>
    /// <param name="FileName">The name of the pigg file to add to the
    /// collection.</param>
    /// <param name="Overwrite">Whether to overwrite the file if it already
    /// exists within the collection.</param>
    /// <returns>And AddFileResult that indicates the result of trying to add
    /// the file to the pigg file collection.</returns>
    /// <remarks>The collection of pigg files is contained in a Dictionary
    /// object.  This is so that there will be no duplicate files in the
    /// collection.  Using this version of the AddFile function, one can
    /// specify whether or not it is desired to overwrite the file if it is
    /// already in the collection.  Either way, if the file exists,
    /// AddFileResult.FileExists will be returned.</remarks>
    public AddFileResult AddFile(string FileName, bool Overwrite) {
      if (m_root == null) { m_root = new PiggNode(); }
      if (!File.Exists(FileName)) {
        return AddFileResult.InvalidFile;
      }
      else {
        FileInfo info = new FileInfo(FileName);
        if (m_filelist.ContainsKey(info.FullName)) {
          if (Overwrite == true) {
            m_filelist.Remove(info.FullName);
            m_filelist[info.FullName] = new PiggFile(info.FullName, m_root);
          }
          return AddFileResult.FileExists;
        }
        else {
          m_filelist[info.FullName] = new PiggFile(info.FullName, m_root);
          return AddFileResult.OK;
        }
      }
    }

    /// <summary>
    /// Adds an entire directory containing pigg files to the pigg file
    /// collection.
    /// </summary>
    /// <param name="PathName">Path name of the directory to add.</param>
    /// <param name="Include">How the directory is to be searched.</param>
    /// <returns>The number of pigg files found and added.</returns>
    public int AddDirectory(string PathName, SearchOption Include) {
      if (!Directory.Exists(PathName)) { return 0; }
      else {
        string[] files = Directory.GetFiles(PathName, "*.pigg", Include);
        int file_count = 0;
        foreach (string file in files) {
          if (AddFile(file, true) == AddFileResult.OK) {
            file_count++;
          }
        }
        return file_count;
      }
    }

    /// <summary>
    /// Returns whether the given filename exists within the pigg file
    /// collection.
    /// </summary>
    /// <param name="FileName">File name of which the existence is tested.
    /// </param>
    /// <returns>True of the file exists within the pigg file collection,
    /// false if it does not.</returns>
    public bool FileExists(string FileName) {
      if (!File.Exists(FileName)) { return false; }
      FileInfo fi = new FileInfo(FileName);
      if (m_filelist.ContainsKey(fi.FullName)) { return true; }
      else { return false; }
    }
    #endregion

    #region Properties
    /// <summary>
    /// List of pigg file objects contained in the pigg file collection.
    /// </summary>
    public PiggFile[] Files {
      get {
        PiggFile[] filelist = new PiggFile[m_filelist.Count];
        m_filelist.Values.CopyTo(filelist, 0);
        return filelist;
      }
    }
    #endregion
  }
}
