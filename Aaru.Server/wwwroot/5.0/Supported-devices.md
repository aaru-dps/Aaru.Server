The following physical devices are supported:

## Operating system support

| Device Type  | FreeBSD  | MacOS  | Linux  | Windows  |
|--------------|----------|--------|--------|----------|
| SCSI Block device  | Yes  | No¹  | Yes  | Yes  |
| SCSI MultiMedia device  | Yes  | Not yet² | Yes  | Yes  |
| SCSI Streaming device  | Yes  | No¹  | Yes  | Yes  |
| Parallel ATA  | No³ | No¹  | Yes  | Yes  |
| Serial ATA  | Yes  | No¹  | Yes  | Yes  |
| USB  | Partial⁴ | Partial⁵ | Yes  | Yes  |
| FireWire  | Partial⁶ | Partial⁵ | Yes  | Partial⁶ |
| PCMCIA  | Partial⁷ | Partial⁵ | Yes  | Partial⁷ |
| SecureDigital / MultiMediaCard  | Not yet⁸ | No¹  | Yes  | Untested⁹ |

1. macOS only allows talking with MultiMedia devices.
2. Support for MultiMedia devices in macOS will be added if users require it
3. Not supported due to upstream bug
4. USB descriptors are not retrieved
5. Only MultiMedia devices can be supported and descriptors will not be retrieved
6. FireWire descriptors are not retrieved
7. PCMCIA CIS is not retrieved
8. Support will come with FreeBSD 12-RELEASE
9. Should work, untested due to not available hardware