# Introduction #

This page describes the file format of .texture files.

# Details #

A texture file consists of the following structures, contained in order:

| _Texture header_ |
|:-----------------|
| _Texture data_   |

## Texture header ##

The texture header consists of the following entries:

| **Offset**     | **Size**   | **Description**          |
|:---------------|:-----------|:-------------------------|
| `0x00000000` | `0x04`   | Size of texture header |
| `0x00000004` | `0x04`   | Size of the image data |
| `0x00000008` | `0x04`   | Width of the image     |
| `0x0000000c` | `0x04`   | Height of the image    |
| `0x00000010` | `0x04`   | _Unknown 1_            |
| `0x00000014` | `0x04`   | _Unknown 2_            |
| `0x00000018` | `0x04`   | _Unknown 3_            |
| `0x0000001c` | `0x04`   | _Marker_               |
| `0x00000020` | varies   | _Filename_             |
| _varies_     | _varies_ | _Unknown 4_            |

### Marker ###

The marker is usually a zero byte (`0x00`) followed by the ASCII characters TX2 (`0x54`, `0x58`, `0x32`).  In some textures, however, the first byte is not zero.  It might be some kind of flag field.

### Filename ###

The filename is a zero-terminated string.  There is no length field preceding the string.

### Unknown fields ###

_Unknown 1_ seems to actually contain information or could be a set of flags.  Currently, _Unknown 2_ and _Unknown 3_ are always set to 0x00000000.

_Unknown 4_ is not always present.  In many of the texture files, the zero-terminator for the filename is the last byte of the header.  Whether or not the _Unknown 4_ data is present can be determined by examining the header size and the position in the stream after reading the filename string.  When it does contain data, it appears to contain information (i.e. much of the data is non-zero).

## Texture Data ##

Following the _Texture header_ fields is the raw texture data, in whatever format the file is stored in.  Currently, the two formats used to store images are Microsoft's [DirectDraw Surface](http://en.wikipedia.org/wiki/DirectDraw_Surface) (.dds) format and [JPEG](http://en.wikipedia.org/wiki/JPEG) (.jpg) format.  The format that is used can be determined by examining the filename extension or examining the image data itself.