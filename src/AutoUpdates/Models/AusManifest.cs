using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoUpdates;

public partial class AusManifest
{

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("version")]
    public Version Version { get; set; } = new Version(0, 0, 1);

    [JsonPropertyName("files")]
    public List<AusFile> Files { get; set; } = [];

    public IReadOnlyList<AusFile> CheckForUpdates(AusManifest package)
    {
        List<AusFile> updates = [];

        if (package.Version.Equals(Version))
            return updates;

        foreach (var file in package.Files)
        {
            if (!Files.Any(x => string.Equals(x.Name, file.Name, StringComparison.InvariantCultureIgnoreCase) && x.Hash == file.Hash))
            {
                updates.Add(file);
            }
        }

        return updates;
    }

    public void SaveAs(string filename)
    {
        var json = JsonSerializer.Serialize(this, typeof(AusManifest), ManifestJsonSerializerContext.DefaultContext);
        //var json = JsonSerializer.Serialize(this, ManifestJsonSerializerContext.Default);
        File.WriteAllText(filename, json);
    }

    public AusManifest Clone()
    {
        return new AusManifest()
        {
            Name = Name,
            Version = Version,
            Files = Files?.Select(x => new AusFile
            {
                Name = x.Name,
                Hash = x.Hash,
                Size = x.Size,
            })?.ToList() ?? []
        };
    }

    public AusManifest ApplyPatch(AusManifest patch)
    {
        AusManifest current = new()
        {
            Name = patch.Name ?? Name,
            Version = patch.Version,
            Files = Files?.Select(x => new AusFile
            {
                Name = x.Name,
                Hash = x.Hash,
                Size = x.Size,
            })?.ToList() ?? []
        };

        foreach (var file in patch.Files)
        {
            var original = current.Files.FirstOrDefault(x => string.Equals(x.Name, file.Name, StringComparison.CurrentCultureIgnoreCase));
            if (original == null)
            {
                current.Files.Add(new AusFile
                {
                    Name = file.Name,
                    Hash = file.Hash,
                    Size = file.Size,
                });
            }
            else
            {
                original.Hash = file.Hash;
                original.Size = file.Size;
            }
        }

        return current;
    }
}
