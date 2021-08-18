# Quickstart

## How to Dump a Disk Image
It’s very easy to dump any supported media quickly with Aaru! You just have to run `aaru media dump <drive> <output image>` for the most basic options. If you dump to something that’s not Aaru Image Format (images with the extensions .aaruformat, .aaruf, and .aif), you will also have to add the “-f” option at the end of the command.

### Examples (Windows):

`aaru media dump E: Image.aaruf`

`aaru media dump F: Image.iso -f`

### Examples (Linux):

`aaru media dump /dev/sr0 Image.aaruf`

`aaru media dump /dev/sr1 Image.iso -f`

## Comparing Between Two Media Dumps
Comparing between media dumps using Aaru is simple! All you have to do is run the command `aaru image compare image-1 image-2`. Neither of the dumps being compared have to be created by Aaru, they can be any of the supported formats from any software.

### Examples (All OS’):

`aaru image compare Image1.aaruf Image2.cue`

`aaru image compare Image1.iso Image2.ccd`

## Analysing a Media Dump
WIP

## Extracting Content From a Media Dump
Aaru can extract the files from any supported media dump, as long as it uses a supported filesystem to store its files. The command for this is “aaru filesystem extract *Image* *Output folder*”. You can also add “-x” to the end of the command to extract the more technical extended attributes from an image as well.

### Examples:
`aaru filesystem extract Image.aaruf Output`

`aaru filesystem extract Image.ccd Output -x`
