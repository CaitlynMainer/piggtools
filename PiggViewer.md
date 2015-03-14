# Introduction #

**Pigg Viewer** (aka **Hero Viewer**) is an application that is commonly used by players to extract and modify files embedded in pigg archives.


# Pigg Viewer #

The original Pigg Viewer application was developed by Keith DeGrace ("Dracos").  The latest version available was 1.62.1641, although it is no longer publicly downloadable.  A 1.61 version is still available from [Coldfront](http://coh.coldfront.net/index.php/content/view/339/58/).

The original Pigg Viewer application included the ability to not only extract files embedded in pigg archives, but also to view textures, hear audio files, and recreate texture files from external images.

The original Pigg Viewer application used the SandBar and SandDock user interface libraries, the BASS sound library, and #ZipLib (SharpZipLib) compression/decompression library.  While the original Pigg Viewer application is powerful, it did have some limitations, such as non-Direct Draw Surface graphics formats not being recognized.  With the source code to the original Pigg Viewer application seemingly not available, direct modification to the original application is unlikely to be undertaken.

# Hero Viewer #

When DeGrace abandoned the Pigg Viewer project, John Toebes rewrote the application as Viewer of Heroes.  The latest version of the application is 5.7, and it is available as a project named as the original, [PiggViewer](http://sourceforge.net/projects/piggviewer/), on SourceForge.  This project appears to have been abandoned for approximately three years as of the writing of this article.  Although a respectable first effort, the rewrite omitted many features of the original Pigg Viewer application, such as the ability to listen to audio files and the ability to create texture files from images.

# Pigg Tools #

The Pigg Tools project is an effort to recreate and expand on the user-friendliness of the original Pigg Viewer application.  Since the source code is seemingly unavailable to the original Pigg Viewer application, the file format and some concepts are being derived from John Toebes's Hero Viewer application.