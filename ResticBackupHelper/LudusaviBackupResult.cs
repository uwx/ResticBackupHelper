using System.Text.Json.Serialization;

namespace ResticBackupHelper;

public class LudusaviBackupResult
{
    [JsonPropertyName("errors")]
    public Errors? Errors { get; set; }

    [JsonPropertyName("overall")]
    public OverallStats Overall { get; set; } = null!;

    [JsonPropertyName("games")]
    public Dictionary<string, GameInfo> Games { get; set; } = null!;
}

public class Errors
{
    [JsonPropertyName("someGamesFailed")]
    public bool? SomeGamesFailed { get; set; }

    [JsonPropertyName("unknownGames")]
    public List<string>? UnknownGames { get; set; }

    [JsonPropertyName("cloudConflict")]
    public Dictionary<string, object>? CloudConflict { get; set; }

    [JsonPropertyName("cloudSyncFailed")]
    public Dictionary<string, object>? CloudSyncFailed { get; set; }
}

public class OverallStats
{
    [JsonPropertyName("totalGames")]
    public int TotalGames { get; set; }

    [JsonPropertyName("totalBytes")]
    public long TotalBytes { get; set; }

    [JsonPropertyName("processedGames")]
    public int ProcessedGames { get; set; }

    [JsonPropertyName("processedBytes")]
    public long ProcessedBytes { get; set; }

    [JsonPropertyName("changedGames")]
    public ChangedGamesCount ChangedGames { get; set; } = null!;
}

public class ChangedGamesCount
{
    [JsonPropertyName("new")]
    public int New { get; set; }

    [JsonPropertyName("same")]
    public int Same { get; set; }

    [JsonPropertyName("different")]
    public int Different { get; set; }
}

public class GameInfo
{
    [JsonPropertyName("decision")]
    public Decision Decision { get; set; } = default!; // Processed, Ignored, Cancelled

    [JsonPropertyName("change")]
    public Change Change { get; set; } = default!; // New, Same, Different

    [JsonPropertyName("files")]
    public Dictionary<string, FileEntry>? Files { get; set; }

    [JsonPropertyName("registry")]
    public Dictionary<string, RegistryEntry>? Registry { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<Change>))]
public enum Change
{
    New, Same, Different
}

[JsonConverter(typeof(JsonStringEnumConverter<Decision>))]
public enum Decision
{
    Processed, Ignored, Cancelled
}

public class FileEntry
{
    [JsonPropertyName("failed")]
    public bool? Failed { get; set; }

    [JsonPropertyName("change")]
    public string Change { get; set; } = null!;

    [JsonPropertyName("ignored")]
    public bool? Ignored { get; set; }

    [JsonPropertyName("bytes")]
    public long Bytes { get; set; }

    [JsonPropertyName("redirectedPath")]
    public string? RedirectedPath { get; set; }

    [JsonPropertyName("originalPath")]
    public string? OriginalPath { get; set; }

    [JsonPropertyName("duplicatedBy")]
    public List<string>? DuplicatedBy { get; set; }
}

public class RegistryEntry
{
    [JsonPropertyName("failed")]
    public bool? Failed { get; set; }

    [JsonPropertyName("change")]
    public string Change { get; set; } = null!;

    [JsonPropertyName("ignored")]
    public bool? Ignored { get; set; }

    [JsonPropertyName("duplicatedBy")]
    public List<string>? DuplicatedBy { get; set; }

    [JsonPropertyName("values")]
    public Dictionary<string, RegistryValue>? Values { get; set; }
}

public class RegistryValue
{
    [JsonPropertyName("change")]
    public string Change { get; set; } = null!;

    [JsonPropertyName("ignored")]
    public bool? Ignored { get; set; }

    [JsonPropertyName("duplicatedBy")]
    public List<string>? DuplicatedBy { get; set; }
}

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(LudusaviBackupResult))]
internal partial class LudusaviJsonContext : JsonSerializerContext;