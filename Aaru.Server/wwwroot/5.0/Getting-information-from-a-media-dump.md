# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will request and show all information about the selected media dump.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image info -h [true/false] <image-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*                            
`-v, --verbose [true/false]` shows verbose output *(default false)*                           
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*           

## Example

```bash
Aaru image info mydisc.cue
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
