using Microsoft.Extensions.FileSystemGlobbing;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace Lantern.Aus;

public class AusManifest
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
    };

    [JsonPropertyName("n")]
    public string Name { get; set; }

    [JsonPropertyName("v")]
    public Version Version { get; set; }

    [JsonPropertyName("r")]
    public Version? RequireVersion { get; set; }

    [JsonPropertyName("f")]
    public IList<AusFile> Files { get; set; } = new List<AusFile>();

    public IList<AusFile> CheckForUpdates(AusManifest package)
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

    public static async Task<IList<AusFile>> LoadAsync(string directory, string[] exclusives, CancellationToken cancellationToken = default)
    {
        Matcher matcher = new();
        matcher.AddIncludePatterns(new string[] { "**/*" });
        matcher.AddExcludePatterns(exclusives);
        var files = matcher.GetResultsInFullPath(directory);

        List<AusFile> packages = new();
        foreach (string filename in files)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            if (exclusives.Contains(filename))
                continue;

            var file = await AusFile.LoadAsync(directory, filename, cancellationToken);
            packages.Add(file);
        }

        return packages;
    }

    public static IList<AusFile> Load(string directory)
    {
        var files = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).ToList();

        List<AusFile> packages = new();
        foreach (string filename in files)
        {
            var file = AusFile.Load(directory, filename);
            packages.Add(file);
        }

        return packages;
    }


    public static AusManifest LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException(filename);

        var json = File.ReadAllText(filename);
        return JsonSerializer.Deserialize<AusManifest>(json, SerializerOptions)!;
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
