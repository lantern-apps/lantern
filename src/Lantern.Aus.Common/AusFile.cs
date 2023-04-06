using Lantern.Aus.Internal;
using System.Text.Json.Serialization;

namespace Lantern.Aus;

public class AusFile
{
    [JsonPropertyName("n")]
    public string Name { get; set; }

    [JsonPropertyName("h")]
    public string Hash { get; set; }

    [JsonPropertyName("s")]
    public long Size { get; set; }

    [JsonPropertyName("v")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Version? FromVersion { get; set; }

    public static async Task<AusFile> LoadAsync(string baseDir, string fileName, CancellationToken cancellationToken = default)
    {
        var file = new FileInfo(fileName);
        var hash = await MD5Helper.ComputeMd5Async(fileName, cancellationToken);
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
        var hash = MD5Helper.ComputeMd5(fileName);
        return new AusFile
        {
            Size = file.Length,
            Name = Path.GetRelativePath(baseDir, fileName),
            Hash = Convert.ToBase64String(hash)
        };
    }

}