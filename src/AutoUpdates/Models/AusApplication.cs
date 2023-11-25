using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoUpdates;

public class AusApplication
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("versions")]
    public required AusAppVersion[] Versions { get; set; }

    public AusAppVersion? GetLatestVersion()
    {
        AusAppVersion? version = null;
        foreach (var v in Versions)
        {
            if (version == null)
            {
                version = v;
            }
            else if (v.Version > version.Version)
            {
                version = v;
            }
        }

        return version;
    }

    public void SaveAs(string path)
    {
        var json = JsonSerializer.Serialize(this, JsonSerializerOptionsHelper.SerializerOptions);
        File.WriteAllText(path, json);
    }
}

public class AusAppVersion
{
    [JsonPropertyName("version")]
    public required Version Version { get; set; }

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("mapFileExtensions")]
    public bool MapFileExtensions { get; set; }
}
