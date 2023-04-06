using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

public class IpcOptions
{
    public static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    protected readonly Dictionary<string, Type?> _messages = new();

    public JsonSerializerOptions JsonSerializerOptions { get; set; } = DefaultSerializerOptions;

    public bool TryGetBodyType(string name, out Type? bodyType) => _messages.TryGetValue(name, out bodyType);

    public IpcOptions Register(string name, Type? bodyType = null)
    {
        _messages.Add(name, bodyType);
        return this;
    }

}
