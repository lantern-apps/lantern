using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Lantern.Aus;

public partial class AusManifest
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("version")]
    public Version Version { get; set; }

    [JsonPropertyName("r")]
    public Version? RequireVersion { get; set; }

    [JsonPropertyName("files")]
    public IList<AusFile> Files { get; set; } = new List<AusFile>();

    public IReadOnlyList<AusFile> CheckForUpdates(AusManifest package)
    {
        List<AusFile> updates = new();

        if (package.Version <= Version)
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

    public void ClearFileVersion(bool clearAll = false)
    {
        foreach (var file in Files)
        {
            if (clearAll || file.FromVersion == Version)
                file.FromVersion = null;
        }
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
                FromVersion = x.FromVersion ?? Version,
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
                FromVersion = x.FromVersion ?? Version,
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
                    FromVersion = patch.Version,
                });
            }
            else
            {
                original.Hash = file.Hash;
                original.Size = file.Size;
                original.FromVersion = patch.Version;
            }
        }

        return current;
    }
}
