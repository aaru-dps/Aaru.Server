## Command description
This operation will show all known attached devices on your system and if they are supported for device dependent operations.

## Command usage
```aaru -d [true/false] -v [true/false] device list -h [true/false] <aaru-remote-host>```

```-d, --debug [true/false]``` shows debug output *(default false)*

```-v, --verbose [true/false]``` shows verbose output *(default false)*

```-h, --help [true/false]``` shows help screen for the command instead of running it, ignores all other switches *(default false)*

```<aaru-remote-host>``` connects to an Aaru Remote Host with ```aaru://<IP ADDRESS>```

## Example
```aaru device list aaru://192.168.1.25```

## Operating system support

|FreeBSD|macOS|Linux|Windows|
|---|---|---|---|
|Yes|Yes|Yes|Yes|