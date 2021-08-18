# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will request and show all information about the inserted media in a physical device. For information about supported physical devices check [the list of supported physical devices.](https://github.com/Senn1/Aaru.Documentation/blob/master/5.0/Supported-devices.md)

## Command usage

```bash
Aaru -d [true/false] -v [true/false] media info -h [true/false] -w [prefix] <device-path/aaru-remote-host>
```

`-d, --debug [true/false]` shows debug output *(default false)*                            
`-v, --verbose [true/false]` shows verbose output *(default false)*                           
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*           
`-w, --output-prefix [prefix]` writes binary responses from device to that prefix       
`<aaru-remote-host>` connects to an Aaru Remote Host with aaru:///       


## Example

FreeBSD: `Aaru media info /dev/cd0`   
Linux: `Aaru media info /dev/sdb`    
Windows: `Aaru media info \\.\PhysicalDrive3`       

## Operating system support

| Device Type  | FreeBSD  | MacOS  | Linux  | Windows  |
|--------------|----------|--------|--------|----------|
| SCSI Block device  | Yes  | No¹  | Yes  | Yes  |
| SCSI MultiMedia device  | Yes  | Not yet² | Yes  | Yes  |
| SCSI Streaming device  | Yes  | No¹  | Yes  | Yes  |
| Parallel ATA  | No³ | No³  | Yes³  | Yes³  |
| Serial ATA  | Yes³  | No³  | Yes³  | Yes³  |
| USB  | Partial | Partial | Yes  | Yes  |
| FireWire  | Partial | Partial | Yes  | Partial |
| PCMCIA  | Partial | Partial | Yes  | Partial |
| SecureDigital / MultiMediaCard  | Not yet³ | No³  | Yes³  | Untested³ |

1. macOS only allows talking with MultiMedia devices.
2. Support for MultiMedia devices in macOS will be added if users require it
3. Use device-info command