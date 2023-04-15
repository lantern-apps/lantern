using System.Text.Json.Serialization;

namespace Lantern.Aus;

public class AusApplication
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("latestVersion")]
    public Version LatestVersion { get; set; }
}
