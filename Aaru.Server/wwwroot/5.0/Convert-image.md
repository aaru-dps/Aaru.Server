# Table of Contents

- [Command Description](#command-description)
- [Command usage](#command-usage)
- [Example](#example)


## Command Description

This operation will convert a dump from one image format to another.

## Command usage

```bash
Aaru -d [true/false] -v [true/false] image convert -h [true/false] -c <count> --comments <comments> --creator <creator> --drive-manufacturer manufacturer> --drive-model <model> --drive-revision <revision> --drive-serial <serial> -f [true/false] --media-barcode <barcode> --media-lastsequence <number> --media-manufacturer <manufacturer> --media-model <model> --media-partnumber <partnumber> --media-sequence <sequence> --media-serial <serial> --media-title <title> -O <options> -p <format> -r <resume file> -x <xml sidecar> <input-path> <output-path>
```

`-d, --debug [true/false]` shows debug output *(default false)*  
`-v, --verbose [true/false]` shows verbose output *(default false)*  
`-h, --help [true/false]` shows help screen for the command instead of running it, ignores all other switches *(default false)*  
`-c, --count <count>` specifies how many sectors to convert at once *(default 64)*    
`--comments <comments>` specifies image comments    
`--creator <creator>` specifies who (person) created the image?    
`--drive-manufacturer manufacturer>` specifies manufacturer of the drive used to read the media represented by the image    
`--drive-model <model>` specifies model of the drive used to read the media represented by the image    
`--drive-revision <revision>` specifies firmware revision of the drive used to read the media represented by the image   
`--drive-serial serial>` specifies serial number of the drive used to read the media represented by the image    
`-f, --force true/false]` continues conversion even if sector or media tags will be lost in the process *(default false)*    
`--media-barcode <barcode>` specifies barcode of the media represented by the image     
`--media-lastsequence <number>` specifies last media of the sequence the media represented by the image corresponds to *(default 0)*    
`--media-manufacturer manufacturer>` specifies manufacturer of the media represented by the image    
`--media-model <model>` specifies model of the media represented by the image     
`--media-partnumber <partnumber>` specifies part number of the media represented by the image    
`--media-sequence <sequence>` specifies number in sequence for the media represented by the image *(default 0)*     
`--media-serial <serial>` specifies serial number of the media represented by the image     
`--media-title <title>` specifies title of the media represented by the image     
`-O, --options <options>` specifies comma separated name=value pairs of options to pass to output image plugin      
`-p, --format <format>`  specifies format of the output image, as plugin name or plugin id. If not present, will try to detect it from output image extension     
`-r, --resume-file <resume file>` takes list of dump hardware from existing resume file      
`-x, --cicm-xml <xml sidecar>` takes metadata from existing CICM XML sidecar      

## Example

```bash
Aaru image convert -c 32 --comments "My converted image" --creator "Jane Doe" --drive-manufacturer "LG" --drive-model "CD-RW 1234" --drive-revision "1.0" --drive-serial "AABBCCDDEEFF01" --media-lastsequence 2 --media-sequence 1 --media-title "Important software" -O "deduplicate=true,nocompress=false" -r dd_dump.resume.xml -x dd_dump.cicm.xml dd_dump.iso dump.dicf
```

