# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This command will ask you which statistics to gather and if you want to share them anonymously. When sharing, no information about you or your computer will be sent or stored, only the number of times a command have been used and the operating system where Aaru is run. This information is not, and will never be, sold to any third party, and is publicly available at [https://www.aaru.app](https://www.aaru.app)

## Command usage

```bash
Aaru -d [true/false] -v [true/false] configure -h [true/false]
```

`-d, --debug [true/false]` shows debug output *(default false)*  
`-v, --verbose [true/false]` shows verbose output *(default false)*  
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*  

## Example

```bash
Aaru configure
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
