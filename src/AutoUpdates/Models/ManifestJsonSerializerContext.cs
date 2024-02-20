using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutoUpdates;

[JsonSerializable(typeof(AusApplication))]
[JsonSerializable(typeof(AusAppVersion))]
[JsonSerializable(typeof(AusManifest))]
[JsonSerializable(typeof(AusFile))]
[JsonSourceGenerationOptions(DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, WriteIndented = true)]
internal partial class ManifestJsonSerializerContext : JsonSerializerContext
{
    public static readonly ManifestJsonSerializerContext DefaultContext = new(new JsonSerializerOptions
    {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    });

}