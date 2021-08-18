# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will print a hexadecimal dump in the console of the chosen sector/block of the indicated media dump image.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image print -h [true/false] -l [sectors] -r [true/false] -s <starting sector> -w [width] <image-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*                      
`-v, --verbose [true/false]` shows verbose output *(default false)*                      
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*                               
`-l, --length [sectors]` specifies how many sectors to print *(default 1)*            
`-r, --long-sectors [true/false]` specifies if hex print should include all sector tags stored in the media dump *(default false)*         
`-s, --start <starting sector>` starts the hexadecimal printing from this sector            
`-w, --width [width]` specifies how long the width, in characters, should the print be before creating a new line *(default 32)*         

## Example

```bash
Aaru image print -s 15 -l 30 -r -w 64 mydisc.cue
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
