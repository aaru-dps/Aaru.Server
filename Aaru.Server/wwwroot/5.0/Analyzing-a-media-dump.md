# Analyzing a media dump

This operation will analyze a media dump, and if the format is recognized and you choose so, it will search for [supported partitioning schemes](Partitioning-schemes-recognized-by-Aaru.md) and [supported filesystems](Filesystems-recognized-by-Aaru.md) on the dump, showing information about them.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image analyze -h [true/false] -e [encoding] -f [true/false] -p [true/false] <image-path>
```

`-d, --debug [true/false]` shows debug output ''(default false)''  
`-v, --verbose [true/false]` shows verbose output (default false)  
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches (default false)  
`-e, --encoding [encoding]` sets which encoding is used by the contents of the media dump (default varies by filesystem)  
`-f, --filesystems [true/false]` searches and interprets filesystems (default true)  
`-p, --partitions [true/false]` searches and interprets partitions (default true)  

## Example

```bash
Aaru image analyze mydisc.cue
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
