# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will create a media dump from real media using a physical device. It will retry errors and when finished create an XML metadata sidecar. The dumping operation can be interrupted and continued later, even with a different device.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] media dump -h [true/false] -e <encoding> -f [true/false] -k <sectors> --first-pregap [true/false] --fix-offset [true/false] -m [true/false] --metadata [true/false] --trim [true/false] -O <options> --persistent [true/false] -p <passes> -s [true/false] -t <plugin> -x <xml sidecar> --subchannel <subchannel> --speed <speed> <device-path/aaru-remote-host> <output-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*  
`-v, --verbose [true/false]` shows verbose output *(default false)*  
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*  
`-e, --encoding <encoding>` specifies character encoding to use when creating dump sidecar    
`-f, --force [true/false]` continues dumping whatever happens *(default false)*     
`-k, --skip <sectors>` skips this many sectors when an unreadable sector is found *(default 512)*      
`--first-pregap [true/false]` tries to dump first track pregap. Only applicable to CD, DDCD or GD media *(default false)*          
`--fix-offset [true/false]` fixes audio tracks offset. Only applicable to CD or GD media. *(default false)*        
`-m, --resume [true/false]` creates and/or use resume mapfile *(default true)*         
`--metadata [true/false]` enables creating CICM XML sidecar *(default true)*       
`--trim [true/false]` enables trimming errores from skipped sectors *(default true)*     
`-O, --options <options>` specifies comma separated name=value pairs of options to pass to output image plugin       
`--persistent [true/false]` tries to recover partial or incorrect data *(default false)*       
`-p, --retry-passes <passes>` specifies how many times to retry reading a sector *(default 5)*         
`-s, --stop-on-error [true/false]` stops dumping on first error *(default false)*    
`-t, --format <plugin>` specifies format for the output image, as plugin name or plugin id. If not present, will try to detect it from output image extension       
`-x, --cicm-xml <xml sidecar>` takes metadata from existing CICM XML sidecar          
`--subchannel <subchannel>` specifies which subchannel to dump. Only applicable to CD/GD. Values: any, rw, rw-or-pq, pq, none *(default any)*     
`--speed <speed>` specifies at what speed to dump. Only applicable to optical drives, 0 for maximum *(default 0)*        
`<aaru-remote-host>` connects to an Aaru Remote Host with aaru:///       

## Example

FreeBSD: `Aaru media dump -f --persistent true --separate-subchannel /dev/cd0 mydisc.cue`     
Linux: `Aaru media dump -r -f -p 15 /dev/sdb myusbfloppy.img`     
Windows: `Aaru media dump -f -p 0 --resume false \\.\PhysicalDrive3 mydisk.dicf`     

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