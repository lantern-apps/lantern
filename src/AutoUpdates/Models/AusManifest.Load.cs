using Microsoft.Extensions.FileSystemGlobbing;
using System.Text.Json;

namespace AutoUpdates;

public partial class AusManifest
{
    public static async Task<AusManifest> LoadAsync(
        string name,
        Version version,
        string directory,
        string[] exclusives,
        CancellationToken cancellationToken = default)
    {
        Matcher matcher = new();
        matcher.AddIncludePatterns(new string[] { "**/*" });
        matcher.AddExcludePatterns(exclusives);
        var filePaths = matcher.GetResultsInFullPath(directory);

        List<AusFile> files = new();
        foreach (string filename in filePaths)
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            if (exclusives.Contains(filename))
                continue;

            var file = await AusFile.LoadAsync(directory, filename, cancellationToken);
            files.Add(file);
        }

        return new()
        {
            Name = name,
            Version = version,
            Files = files
        };
    }

    public static AusManifest Load(
        string name,
        Version version,
        string directory,
        string[] exclusives)
    {
        Matcher matcher = new();
        matcher.AddIncludePatterns(new string[] { "**/*" });
        matcher.AddExcludePatterns(exclusives);
        var filePaths = matcher.GetResultsInFullPath(directory);

        List<AusFile> files = new();
        foreach (string filename in filePaths)
        {
            if (exclusives.Contains(filename))
                continue;

            var file = AusFile.Load(directory, filename);
            files.Add(file);
        }

        return new()
        {
            Name = name,
            Version = version,
            Files = files
        };
    }

    public static AusManifest ParseFile(string filename)
    {
        if (!File.Exists(filename))
            throw new FileNotFoundException(filename);

        var json = File.ReadAllText(filename);
        return JsonSerializer.Deserialize<AusManifest>(json,  JsonSerializerOptionsHelper.SerializerOptions)!;
    }
}