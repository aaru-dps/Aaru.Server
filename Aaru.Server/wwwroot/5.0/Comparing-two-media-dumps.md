# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will compare two media dumps and print all differences between them. Dumps can be in different formats.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image compare -h [true/false] <image-path1> <image-path2>
```

`-d, --debug [true/false]` shows debug output *(default false)*  
`-v, --verbose [true/false]` shows verbose output *(default false)*  
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*  

## Example

```bash
Aaru image compare mydisc.cue anotherdisc.mds
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
