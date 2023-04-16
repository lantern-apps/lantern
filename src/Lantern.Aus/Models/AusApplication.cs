using System.Text.Json.Serialization;

namespace Lantern.Aus;

public class AusApplication
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("version")]
    public Version Version { get; set; }

    [JsonPropertyName("mapFileExtensions")]
    public bool MapFileExtensions { get; set; }
}
