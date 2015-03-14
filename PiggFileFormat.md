# Introduction #

This page describes the Pigg file format.


# Details #

A pigg file consists of the following structures, contained in order:

|_Pigg file header_|
|:-----------------|
|_Directory entry table_|
|_String table_|
|_Secondary directory entry table_|
|_Data_|

## Pigg file header ##

The pigg file header consists of the following entries:

| **Offset** | **Size** | **Description** |
|:-----------|:---------|:----------------|
| `0x00000000` | `0x04` | Header marker |
| `0x00000004` | `0x04` | Unknown 1 |
| `0x00000008` | `0x04` | Unknown  2 |
| `0x0000000c` | `0x04` | Directory entry count |

### Header marker ###

Always set to 0x00000123.  Use this to verify file integrity.

### Unknown values ###

These two 32-bit values might also be markers, or it could be some kind of flags.  Currently, the values are always set to 0x00020002 and 0x00300010, respectively.

### Directory entry count ###

The number of directory entries in the directory entry table.  Note that since the size of directory entries is constant (0x30 bytes), the total size of the directory entry table can be calculated by multiplying this value by the size of a directory entry structure.  So, for example, if this value is 0x000008a7, the size of the directory entry table will be 0x000008a7 `*` 0x30, or 0x00019f50.

## Directory entry table ##

The directory entry table consists of the following entries:

| **Offset**     | **Size** | **Description**       |
|:---------------|:---------|:----------------------|
| `0x00000000` | `0x30` | _Directory entry 1_ |
| `0x00000030` | `0x30` | _Directory entry 2_ |
| `0x00000060` | `0x30` | _Directory entry 3_ |
| ... | | |

The number of directory entries in the directory entry table is specified by the _Directory entry count_ value in the _Pigg file header_ structure.  Each directory entry is composed of the following fields:

| **Offset**     | **Size** | **Description**                   |
|:---------------|:---------|:----------------------------------|
| `0x00000000` | `0x04` | Directory entry marker          |
| `0x00000004` | `0x04` | String index                    |
| `0x00000008` | `0x04` | Uncompressed file size          |
| `0x0000000c` | `0x04` | Datestamp                       |
| `0x00000010` | `0x08` | File offset                     |
| `0x00000018` | `0x04` | Secondary directory entry index |
| `0x0000001c` | `0x10` | MD5 checksum                    |
| `0x0000002c` | `0x04` | Compressed file size            |

### Directory entry marker ###

Always set to 0x00003456.  Use this to verify file integrity.

### String index ###

Index of filename in the string table.  Currently, there are always exactly as many strings in the string table as directory entries.

### Uncompressed file size ###

Total size of the embedded file, in bytes.

### Datestamp ###

The date and time the file was last modified, expressed as a Unix epoch timestamp (seconds since midnight, January 1, 1970 UTC).

### File offset ###

The offset of the embedded file within the pigg file, in bytes, from the beginning of the file.

### Secondary directory entry index ###

Index of the secondary directory entry, or zero if there is no secondary directory entry associated with this file.

### MD5 checksum ###

16-byte MD5 checksum of the uncompressed file.

### Compressed file size ###

Size of the compressed embedded file, in bytes.  If this value is zero, the embedded file is not compressed and can be extracted directly from the pigg file.  Compressed files use the [zlib compressed data format](http://www.ietf.org/rfc/rfc1950.txt).

## String Table ##

The string table consists of the following entries:

| **Offset**     | **Size**   | **Description**       |
|:---------------|:-----------|:----------------------|
| `0x00000000` | `0x04`   | String table marker |
| `0x00000004` | `0x04`   | String count        |
| `0x00000008` | `0x30`   | String table size   |
| `0x0000000c` | _varies_ | _String 1_          |
| _varies_     | _varies_ | _String 2_          |
| _varies_     | _varies_ | _String 3_          |
| ... | | |

The number of strings in the string table is specified by the _String count_ value.  The total size of the string table, in bytes, is given by the _String table size_ value.  Each string is composed of the following fields:

| **Offset**     | **Size**   | **Description**          |
|:---------------|:-----------|:-------------------------|
| `0x00000000` | `0x04`   | String Length          |
| `0x00000004` | _varies_ | Zero-terminated string |