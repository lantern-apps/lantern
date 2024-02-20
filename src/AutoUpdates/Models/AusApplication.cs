using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoUpdates;

public class AusApplication
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("versions")]
    public AusAppVersion[] Versions { get; set; } = [];

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
        var json = JsonSerializer.Serialize(this, typeof(AusApplication), ManifestJsonSerializerContext.DefaultContext);
        File.WriteAllText(path, json);
    }
}
