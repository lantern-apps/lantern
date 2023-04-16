using Lantern.Aus.Internal;
using System.Text.Json.Serialization;

namespace Lantern.Aus;

public class AusFile
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("hash")]
    public string Hash { get; set; }

    [JsonPropertyName("size")]
    public long Size { get; set; }

    public bool ValidateFile(string baseDir)
    {
        var fileInfo = new FileInfo(Path.Combine(baseDir, Name));

        if (!fileInfo.Exists || fileInfo.Length != Size)
        {
            return false;
        }
        return true;
    }

    public static async Task<AusFile> LoadAsync(string baseDir, string fileName, CancellationToken cancellationToken = default)
    {
        var file = new FileInfo(fileName);
        var hash = await FileSystemHelper.ComputeSha256Async(fileName, cancellationToken);
        return new AusFile
        {
            Size = file.Length,
            Name = Path.GetRelativePath(baseDir, fileName),
            Hash = Convert.ToBase64String(hash)
        };
    }

    public static AusFile Load(string baseDir, string fileName)
    {
        var file = new FileInfo(fileName);
        var hash = FileSystemHelper.ComputeSha256(fileName);
        return new AusFile
        {
            Size = file.Length,
            Name = Path.GetRelativePath(baseDir, fileName),
            Hash = Convert.ToBase64String(hash)
        };
    }

}