## Command description
This operation will analyze and find all filesystems in a media dump and list all the files that are contained in [[Filesystems-recognized-by-Aaru.md|supported filesystems]].

## Command usage
```aaru -d [true/false] -v [true/false] filesystem list -h [true/false] -e [encoding] -l [true/false]``` 

```-d, --debug [true/false]```shows debug output *(default false)*

```-v, --verbose [true/false]``` shows verbose output *(default false)*

```-h, --help [true/false]``` shows help screen for the command instead of running it, ignores all other switches '*(default false)*

```-e, --encoding [encoding]``` sets which encoding is used by the contents of the media dump *(default varies by filesystem)*

```-l, --long [true/false]``` uses a long listing format, showing sizes and extended attributes *(default false)*

## Example
```aaru filesystem list -l -e x-mac-icelandic mydisc.cue```

## Operating system support
|FreeBSD|macOS|Linux|Windows|
|---|---|---|---|
|Yes|Yes|Yes|Yes|
