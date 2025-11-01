using YamlDotNet.Serialization;

namespace ResticBackupHelper;

public record YamlStructure
{
    [YamlMember(Alias = "meta", ApplyNamingConventions = false)] public Meta Meta { get; set; } = null!;
    [YamlMember(Alias = "backups", ApplyNamingConventions = false)] public Dictionary<string, Backup> Backups { get; set; } = new();
    [YamlMember(Alias = "ludusavi", ApplyNamingConventions = false)] public Ludusavi? Ludusavi { get; set; }
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

public record Ludusavi
{
    [YamlMember(Alias = "enable", ApplyNamingConventions = false)] public bool Enable { get; set; }
    [YamlMember(Alias = "ludusavi-executable", ApplyNamingConventions = false)] public string LudusaviExecutable { get; set; } = "ludusavi";
    [YamlMember(Alias = "ludusavi-tag", ApplyNamingConventions = false)] public string LudusaviTag { get; set; } = "Ludusavi";
    [YamlMember(Alias = "additional-args", ApplyNamingConventions = false)] public string[] AdditionalArgs { get; set; } = [];
    [YamlMember(Alias = "additional-restic-args", ApplyNamingConventions = false)] public string[] AdditionalResticArgs { get; set; } = [];
    [YamlMember(Alias = "use-vss", ApplyNamingConventions = false)] public bool UseVss { get; set; } = false;
}

[YamlStaticContext]
[YamlSerializable(typeof(YamlStructure))]
[YamlSerializable(typeof(Backup))]
[YamlSerializable(typeof(Meta))]
[YamlSerializable(typeof(Ludusavi))]
public partial class YamlStaticContext;
