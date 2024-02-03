using System.Text.Json.Serialization;

namespace Lantern.Serialization;

internal class JsonStringEnumConverterAttribute : JsonConverterAttribute
{
    private static JsonConverter? _camelCaseConverter;
    private static JsonConverter? _converter;

    public JsonStringEnumConverterAttribute(bool camelCase = true)
    {
        CamelCase = camelCase;
    }

    public bool CamelCase { get; }

    public override JsonConverter? CreateConverter(Type typeToConvert) => CamelCase ?
        _camelCaseConverter ??= new JsonStringEnumConverter(System.Text.Json.JsonNamingPolicy.CamelCase) :
        _converter ??= new JsonStringEnumConverter();
}