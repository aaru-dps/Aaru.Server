# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will request and show all information about a physical device. For information about supported physical devices check [the list of supported physical devices.](https://github.com/aaru-dps/Aaru.Documentation/blob/master/5.0/Supported-devices.md)

## Command usage

```bash
Aaru -d [true/false] -v [true/false] device info -h [true/false] -w [prefix] <device-path/aaru-remote-host>
```

`-d, --debug [true/false]` shows debug output *(default false)*                            
`-v, --verbose [true/false]` shows verbose output *(default false)*                           
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*           
`-w, --output-prefix [prefix]` writes binary responses from device to that prefix       
`<aaru-remote-host>` connects to an Aaru Remote Host with aaru:///       


## Example

FreeBSD: `Aaru device info /dev/cd0`   
Linux: `Aaru device info /dev/sda`    
Windows: `Aaru device info D:`       

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
