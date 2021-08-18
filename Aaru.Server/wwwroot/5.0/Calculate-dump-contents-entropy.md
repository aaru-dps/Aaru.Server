# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will calculate uniqueness and entropy of the media represented by a media dump image. It's not affected by the image format compression if applicable.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image entropy -h [true/false] -p [true/false] -t [true/false] -w [true/false] <image-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*  
`-v, --verbose [true/false]` shows verbose output *(default false)*  
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*  
`-p, --duplicated-sectors [true/false]` besides entropy also calculates how many sectors have the exact same data in their user area *(default true)*  
`-t, --separated-tracks true/false]` separately calculates the entropy for each track dividing the media. Only applicable to certain kind of media (optical discs and digital tapes mostly) *(default true)*  
`-w, --whole-disc [true/false]` calculates the entropy for the whole media *(default true)*

## Example

```bash
Aaru image entropy mydisc.cue
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
