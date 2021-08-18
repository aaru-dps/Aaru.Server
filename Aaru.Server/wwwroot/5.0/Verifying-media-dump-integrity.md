## Command description
This operation will verify a media dump.
If the media dump format includes a hash or checksum, it will calculate and compare it.
If the media sectors/blocks format include a hash, checksum or error recovery system, it will calculate and compare them.

## Command usage
```aaru -d [true/false] -v [true/false] image verify -h [true/false] -s [true/false] -w [true/false] <image-path>```

```-d, --debug [true/false]``` shows debug output *(default false)*

```-v, --verbose [true/false]``` shows verbose output *(default false)*

```-h, --help [true/false]``` shows help screen for the command instead of running it, ignores all other switches *(default false)*

```-s, --verify-sectors [true/false]``` calculates and verifies the hash/checksum/ecc of every sector/block in the media dump *(default true)*

```-w, --verify-disc [true/false]``` calculates and verifies a media dump format checksum/hash *(default true)*


## Example
```aaru image verify mydisc.cue```


## Operating system support

| FreeBSD | macOS | Linux | Windows |
|---|---|---|---|
| Yes | Yes | Yes | Yes |