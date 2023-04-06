using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lantern.Messaging;

internal class IpcMessage
{
    [JsonPropertyName("id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? InvoicatonId { get; set; }
}

internal sealed class IpcResponse : IpcMessage
{
    //[JsonPropertyName("name")]
    //public required string Name { get; set; }

    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public object? Body { get; set; }

    [JsonPropertyName("error")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool Error { get; set; }
}

internal sealed class IpcEvent : IpcMessage
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public object? Body { get; set; }

    [JsonPropertyName("target")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Target { get; set; }
}

internal sealed class IpcRequest : IpcMessage
{
    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public JsonElement Body { get; set; }

    [JsonPropertyName("target")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Target { get; set; }

    public bool HasBody() => Body.ValueKind != JsonValueKind.Null && Body.ValueKind != JsonValueKind.Undefined;
}