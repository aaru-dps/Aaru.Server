# Aaru Data Preservation Suite

Hello! **Aaru Data Preservation Suite** is a fully-featured software package to preserve all storage media from the very old to the cutting edge, as well as to give detailed information about any supported image file (whether from Aaru or not) and to extract the files from those images.

## Aaru DPS includes:
- **Aaru**
  - The main software that does all the heavy lifting, including dumping and extraction.
- **Aaru Remote**
  - A slim application that allows Aaru to dump media or run other commands on an unsupported system via a network connection.
- **Libaaruformat**
  - C implementation of the Aaru Image Format.
- And more!

## Key features of Aaru:

- Aaru Image Format, the format that stores the most amount of data for any given format, and supports media of almost any type.
- Dumps almost any kind of storage media, with more types of media being added whenever possible.
- Able to read information and extract files from many different formats, as well as easily convert from any format to many other compatible formats.
- Automatically stores metadata, including checksums, of the image files and their tracks (if applicable) in an easily accessible XML, and can generate this data for any image.

## Key features of the Aaru Image Format:
- Stores the most amount of data compared to any other image format for supported media.
- Uses compression to take up the least space possible compared to any other format.
- Can be used to store data from any type of media supported by Aaru.
- Can be converted to many formats supported by Aaru, so you can easily create an ISO (or other image types) for software that doesn’t support the Aaru Image Format.

## Installation

Download the latest release from https://github.com/aaru-dps/Aaru/releases

For a practical UI for Aaru, we recommend using the [Media Preservation Frontend](https://github.com/SabreTools/MPF).

## Quickstart

See the [Quickstart](5.3/Quickstart.md) section of the docs!