using YamlDotNet.Serialization;

namespace ResticBackupHelper;

public record YamlStructure
{
    [YamlMember(Alias = "meta", ApplyNamingConventions = false)] public Meta Meta { get; set; } = null!;
    [YamlMember(Alias = "backups", ApplyNamingConventions = false)] public Dictionary<string, Backup> Backups { get; set; } = new();
}

public record Backup
{
    [YamlMember(Alias = "path", ApplyNamingConventions = false)] public string? Path { get; set; }
    [YamlMember(Alias = "use-vss", ApplyNamingConventions = false)] public bool UseVss { get; set; }
    [YamlMember(Alias = "include", ApplyNamingConventions = false)] public string[] Include { get; set; } = [];
    [YamlMember(Alias = "iexclude", ApplyNamingConventions = false)] public string[] Iexclude { get; set; } = [];
    [YamlMember(Alias = "exclude", ApplyNamingConventions = false)] public string[] Exclude { get; set; } = [];
    [YamlMember(Alias = "tags", ApplyNamingConventions = false)] public string[] Tags { get; set; } = [];
    [YamlMember(Alias = "additional-args", ApplyNamingConventions = false)] public string[] AdditionalArgs { get; set; } = [];
}

public record Meta
{
    [YamlMember(Alias = "password", ApplyNamingConventions = false)] public string Password { get; set; } = null!;
    [YamlMember(Alias = "repository", ApplyNamingConventions = false)] public string Repository { get; set; } = null!;
    [YamlMember(Alias = "pack-size", ApplyNamingConventions = false)] public int PackSize { get; set; }
    [YamlMember(Alias = "restic-executable", ApplyNamingConventions = false)] public string ResticExecutable { get; set; } = "restic";
}

[YamlStaticContext]
[YamlSerializable(typeof(YamlStructure))]
[YamlSerializable(typeof(Backup))]
[YamlSerializable(typeof(Meta))]
public partial class YamlStaticContext;
