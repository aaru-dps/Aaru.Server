# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)
- [Operating system support](#operating-system-support)


## Command Description

This operation will calculate the checksums for the media represented by a media dump image.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image checksum -h [true/false] -a [true/false] --crc16 [true/false] -c [true/false] --crc64 [true/false] -f [true/false] --fletcher16 [true/false] --fletcher32 [true/false] -m [true/false] --ripemd160 [true/false] -s [true/false] --sha256 [true/false] --sha384 [true/false] --sha512 [true/false] -t [true/false] -w [true/false] <image-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*                            
`-v, --verbose [true/false]` shows verbose output *(default false)*                           
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*                                      
`-a, --adler32 [true/false]` calculates the Adler-32 checksum *(default true)*       
`--crc16 [true/false]` calculates the CRC16 checksum *(default true)*      
`-c, --crc32 [true/false]` calculates the CRC32 checksum *(default true)*        
`--crc64 [true/false]` calculates the ECMA CRC64 checksum *(default false)*              
`-f, --spamsum [true/false]` calculates the SpamSum fuzzy hash *(default true)*            
`--fletcher16 [true/false]` calculates the Fletcher-16 checksum *(default false)*              
`--fletcher32 [true/false]` calculates the Fletcher-32 checksum *(default false)*            
`-m, --md5 [true/false]` calculates the MD5 hash *(default true)*         
`--ripemd160 [true/false]` calculates the RIPEMD160 hash *(default false)*         
`-s, --sha1 [true/false]` calculates the SHA1 hash *(default true)*     
`--sha256 [true/false]` calculates the SHA2 hash with 256-bit *(default false)*       
`--sha384 [†rue/false]` calculates the SHA2 hash with 384-bit *(default false)*         
`--sha512 [true/false]` calculates the SHA2 hash with 512-bit *(default false)*         
`-t, --separated-tracks [true/false]` calculates each track checksum separately *(default true)*          
`-w, --whole-discs [true/false]` calculates the whole media checksum *(default true)*        

## Example

```bash
Aaru image checksum -a false --sha512 true mydisc.cue
```

## Operating system support

| OS | Supported |
|----|-----------|
| FreeBSD | Yes  |
| macOS   | Yes  |
| Linux   | Yes  |
| Windows | Yes  |
