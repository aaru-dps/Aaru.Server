## Command description
This operation will update the master database, and can optionalyl clear the master and local database.

## Command usage
```aaru -d [true/false] -v [true/false] database update -h [true/false]``` 

```-d, --debug [true/false]``` shows debug output *(default false)*

```-v, --verbose [true/false]``` shows verbose output *(default false)*

```-h, --help [true/false]``` shows help screen for the command instead of running it, ignores all other switches *(default false)*

```--clear [true/false]``` clears existing master database before updating *(default false)*

```--clear-all [true/false]``` clears existing master and local database before updating *(default false)*

## Example
```aaru database update --clear-all```

## Operating system support

| FreeBSD | macOS | Linux | Windows |
|---|---|---|---|
| Yes | Yes | Yes | Yes |