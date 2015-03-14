# Version 1.0 Goals #

Create a back-end assembly to facilitate extracting files and information from pigg archive files.

Create a functional replacement for the [Pigg Viewer](http://sourceforge.net/projects/piggviewer/) application, complete with the ability to view and extract files and/or directories and textures, and the ability to create .texture files from source image files.

## Details ##

There are a few differences between the planned release of Pigg Viewer Pro and the original Pigg Viewer application:

  * Pigg Viewer Pro is written in C#, allowing the project to be developed much more rapidly and taking advantage of the latest .NET features released by Microsoft.
  * Pigg Viewer Pro will have the ability to load an entire directory of pigg files, adding all files embedded across all pigg files into one integrated view.
  * The original Pigg Viewer application ignored the texture file header that specified the real size of images, which might be smaller than the image embedded within a texture file due to the dimensions of a DDS file necessarily being powers of two.  The Pigg Viewer Pro application will use the texture header to determine the real dimensions of DDS images and extract or render them accordingly.
  * The original Pigg Viewer application assumed that all images embedded within texture files are in the DDS format.  While usually true, some images might be JPEG images.  Pigg Viewer Pro will render these images as well.
  * Several other quality of life features are planned to be added, and will be discussed as they get closer to implementation.