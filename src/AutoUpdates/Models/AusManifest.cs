using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace AutoUpdates;

public partial class AusManifest
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("version")]
    public required Version Version { get; set; }

    [JsonPropertyName("files")]
    public required IList<AusFile> Files { get; set; } = new List<AusFile>();

    public IReadOnlyList<AusFile> CheckForUpdates(AusManifest package)
    {
        List<AusFile> updates = new();

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
        var json = JsonSerializer.Serialize(this, SerializerOptions);
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
            })?.ToList() ?? new List<AusFile>()
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
            })?.ToList() ?? new List<AusFile>()
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
