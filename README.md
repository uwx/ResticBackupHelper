# ResticBackupHelper

A C# utility for automating Restic backups with YAML configuration and Ludusavi game save integration.

## Features

- Configure multiple backup jobs in a single YAML file
- Windows VSS (Volume Shadow Copy) support for locked files
- Integration with Ludusavi for automatic game save backups in a format identical to [playnite-ludusavi-restic](https://github.com/sharkusmanch/playnite-ludusavi-restic)
- Environment variable expansion in paths
- Tag-based backup organization

## Requirements

- Restic (backup tool)
- Ludusavi (optional, for game saves)

## Configuration

Create a `backups.yml` file with the following structure:

```yaml
meta:
  password: your-restic-password
  repository: your-restic-repo
  pack-size: 128
  restic-executable: path/to/restic.exe

# Optional for ludusavi integration
ludusavi:
  enable: true
  ludusavi-executable: path/to/ludusavi.exe
  ludusavi-tag: Ludusavi
  use-vss: false

backups:
  'Backup Name': # Will be used as the snapshot tag
    # this path will become the backup's root folder. (optional) Omit it if you want absolute paths in your snapshot.
    path: C:\Path\To\Backup
    # Set to true to use VSS for this backup (optional)
    use-vss: false
    # Equivalent to restic --include, --exclude and --iexclude (optional)
    include: []
    exclude: []
    iexclude: []
    # Additional snapshot tags (optional)
    tags: []
    # Arguments to pass through to restic (optional)
    additional-args: []
```

## Usage

Run the executable from the directory containing `backups.yml`:

```cmd
ResticBackupHelper.exe
```

Options:
- `--dry-run` - Preview what would be backed up without creating snapshots
- `--ignore-errors` - Continue processing even if backups fail

## License

MIT

