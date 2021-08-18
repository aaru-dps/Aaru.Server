# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will decode all [sector tags](https://github.com/aaru-dps/Aaru.Documentation/blob/master/5.0/Sector-tags.md) and [media tags](https://github.com/aaru-dps/Aaru.Documentation/blob/master/5.0/Media-tags.md) on a media dump image.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image decode -h [true/false] -f [true/false] -l [sectors] -p [true/false] -s [start sector] <image-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*          
`-v, --verbose [true/false]` shows verbose output *(default false)*           
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*              
`-f, --disk-tags [true/false]` decodes all media tags *(default true)*          
`-l, --length [sectors]` how many sectors to decode or all to decode all *(default all)*          
`-p, --sector-tags [true/false]` decodes all sector tags *(default true)*           
`-s, --start [start-sector]` starting sector *(default 0)*         

## Example

```bash
Aaru image decode -s 1000 -l 15 -p false mydisc.cue
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
