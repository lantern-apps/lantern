using System.Text.Json.Serialization;

namespace AutoUpdates;

public class AusAppVersion
{
    [JsonPropertyName("version")]
    public Version Version { get; set; } = new Version(0, 0, 1);

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("mapFileExtensions")]
    public bool MapFileExtensions { get; set; }
}
