Some software contains bugs that can make users think the problem resides with Aaru. This page contains a list of such bugs.

### ImgBurn complains image is smaller than disc
![](https://i.imgur.com/ddAM1YS.png)

This seems to be a common bug, also appearing with images created with other software.

The problem comes because ImgBurn uses the `SCSI READ CAPACITY` command to decide the size of compact discs.
Aaru instead reads and interprets the TOC (Table of Contents), that describes the disc start and end. While that command should indicate the same size as the TOC interpretation, seems to not be so in some conditions (drive firmware bug?).

A CompactDisc data area starts from the Track 1 pregap (MSF 00:00:00, LBA -150) to the last Lead-out start, and Aaru dumps from the Track 1 start (MSF 00:02:00, LBA 0) unless you want it to try to read the pregap (not all drives can read it).
