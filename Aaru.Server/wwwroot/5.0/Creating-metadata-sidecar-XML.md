# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will analyze a media dump, and if the format is recognized, create a CICM XML metadata sidecar with all information that can be automatically gotten about it, as well as all [supported checksum algorithms.](https://github.com/aaru-dps/Aaru.Documentation/blob/master/5.0/Supported-checksums.md)

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image create-sidecar -h [true/false] -b [block size] -e [encoding] -t tape [true/false] <image-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*  
`-v, --verbose [true/false]` shows verbose output *(default false)*  
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*    
`-b, --block-size [block size]` used only for tapes, indicates fixed block size in bytes. File in dump folder not multiple of this value will be ignored *(default 512)*           
`-e, --encoding [encoding]` sets which encoding is used by the contents of the media dump *(default varies by filesystem)*            
`-t, --tape [true/false]` indicates that dump points to a folder containing alphabetically sorted files extracted from a linear block-based tape with fixed block size (e.g. a SCSI streaming device) *(default false)*          

## Example

```bash
Aaru image create-sidecar mydisc.cue
Aaru image create-sidecar -t -b 1024 mytapedir
Aaru image create-sidecar -e shift_jis "My japanese software.img"
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
