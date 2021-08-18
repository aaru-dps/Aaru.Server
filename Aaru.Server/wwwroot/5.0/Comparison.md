In this page you can find a comparison between Aaru and other dump image managers or creators.

## Comparison of optical discs image managers

|                                                 | Aaru                     | DiscImageCreator | CDRWin   | CloneCD  | Alcohol 120% | IsoBuster | WinImage  |
|-------------------------------------------------|--------------------------|------------------|----------|----------|--------------|-----------|-----------|
| Opensource                                      | Yes                      | [Yes](https://github.com/saramibreak/DiscImageCreator)              | No       | No       | No           | No        | No        |
| Supported  platforms                            | Windows,  Linux,  macOS¹ |  Windows         |  Windows |  Windows |  Windows     |  Windows  |  Windows  |
| Supported  formats                              | [12](https://github.com/aaru-dps/Aaru/blob/master/README.md#supported-disk-image-formats-read-and-write)                       | 3²               | 1³       | 1⁴       | 1⁵           | 17⁶       | 1⁷        |
| Can dump  Audio CD                              | Yes⁸                     | Yes              | Yes      | Yes      | Yes          | Yes       | No        |
| Can dump  Mode 0 tracks                         | Yes                      | No               | No       | No       | No           | No        | No        |
| Can dump  Mode 1 tracks                         | Yes                      | Yes              | Yes      | Yes      | Yes          | Yes       | Yes       |
| Can dump  Mode 2 tracks                         | Yes⁸                     | Yes              | Yes      | Yes      | Yes          | Yes       | Yes       |
| Can dump  multisession  discs                   |  Yes                     |  Yes             |  No      |  Yes     |  Yes         |  Yes      |  No       |
| Can dump  discs with  errors                    |  Yes⁹                    |  Yes¹⁰           |  No¹¹    |  Yes¹⁰   |  Yes¹⁰       |  Unknown  |  Unknown  |
| Reads  subchannel                               | Yes                      | Yes              | No¹²     | Yes      | Yes          | Unknown   | No        |
| Reads lead-in  postgap                          | Yes                      | Yes              | No       | No       | No           | No        | No        |
| Supports  illegal TOCs                          | Yes                      | Yes              | No       | Yes      | Yes          | Yes       | No        |
| Supports error  based copy  protections         | Yes                      | Yes              | No       | Yes      | Yes          | Yes       | No        |
| Supports twin  sectors based  copy  protections | Not yet¹³                |   No             |   No     |   No     |   No         |   No      |   No      |
| Supports  position based copy  protections      | Not yet¹³                |   No             |   No     |   No     |   Yes        |   No      |   No      |
| Supports  dumping DDCD                          | Yes                      | No¹⁴             | No¹⁴     | No¹⁴     | No¹⁴         | No¹⁴      | No¹⁴      |
| Supports  dumping GD                            | Not yet¹⁵                | Yes              | No       | No       | No           | No        | No        |
| Supports  dumping  GameCube/Wii                 | Not yet¹⁵                |  Yes             |  No      |  No      |  No          |  No       |  No       |
| Supports  dumping  DVD¹⁶ ¹⁷                     | Yes                      | Partial¹⁸        | No       | No       | Partial¹⁸    | Partial¹⁸ | Partial¹⁸ |
| Supports  dumping HD  DVD¹⁶                     | Yes                      | Partial¹⁹        | No       | No       | Partial¹⁹    | Partial¹⁹ | Partial¹⁹ |
| Supports  dumping Blu- ray¹⁶ ²⁰                 | Yes                      | Yes              | No       | No       | Yes          | Yes       | Yes       |
| Supports  dumping Xbox  Game discs              |  Yes²¹ ²²                |  Yes²²           |  No      |  No      |  No          |  No       |  No       |

1. macOS does not yet support dumping media, only managing existing 
images

2. CDRWin, CloneCD and raw

3. CDRWin

4. CloneCD

5. Alcohol 120%

6. Alcohol 120%, BlindWrite 5, BlindWrite 6, CD-i OptImage, CDRWin, CloneCD, DiscJuggler, Easy CD Creator, IsoBuster, Nero, NTI, PlexTools, Prassi PrimoCD, Prassi PrimoDVD, raw, Virtual CD and WinOnCD

7. Raw

8. It can lose a few seconds from start of audio on some discs

9. Depending on the drive it can recover data from sectors with errors

10. Writes fake data in the place of sectors with errors

11. Ignores errors or stops on error

12. Only if it detects the disc is | No a CD+G

13. Pending format support

14. None of its supported formats support the DDCD media

15. Feature will be added in next release

16. Does not include encrypted video media

17. Includes PlayStation DVD

18. Customized PFI information, like PSN of start LBA in DVD-RAM will be lost

19. Customized PFI information, like PSN of start LBA in HD DVD-RAM will be lost

20. Includes PlayStation Blu-ray

21. XGD2 are untested

22. XGD3 are not supported