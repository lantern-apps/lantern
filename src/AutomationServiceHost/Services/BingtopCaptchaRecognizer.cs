using System.Text.Json;
using System.Text.Json.Serialization;

namespace AutomationServiceHost.Services;

public class RecognizeResult
{
    public RecognizeResult(string error)
    {
        Success = false;
        Error = error;
    }

    public RecognizeResult(int angle)
    {
        Success = true;
        Angle = angle;
    }

    public bool Success { get; }
    public string? Error { get; }
    public int Angle { get; }
}

public interface ITiktokCaptchaRecognizer
{
    Task<RecognizeResult> RecognizeAsync(string innerUrl, string outnerUrl, CancellationToken cancellationToken = default);
}

internal class BingtopCaptchaRecognizer : ITiktokCaptchaRecognizer
{

    private static async Task<string> GetImageBase64(HttpClient client, string imageUrl, CancellationToken cancellationToken)
    {
        using var stream = await client.GetStreamAsync(imageUrl, cancellationToken);
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        ms.Seek(0, SeekOrigin.Begin);
        return Convert.ToBase64String(ms.ToArray());
    }
    public async Task<RecognizeResult> RecognizeAsync(string innerUrl, string outnerUrl, CancellationToken cancellationToken = default)
    {
        HttpClient client = new();

        string inner = await GetImageBase64(client, innerUrl, cancellationToken);
        string outer = await GetImageBase64(client, outnerUrl, cancellationToken);

        FormUrlEncodedContent form = new(new Dictionary<string, string>
        {
            { "username", "adamxx" },
            { "password", "adaxin()" },
            { "captchaType", "1122" },
            { "captchaData", outer },
            { "subCaptchaData", inner },
        });

        HttpRequestMessage request = new()
        {
            Content = form,
            Method = HttpMethod.Post,
            RequestUri = new Uri("https://www.bingtop.com/ocr/upload2/"),
        };

        var response = await client.SendAsync(request, cancellationToken);

        var body = await response.Content.ReadAsStringAsync(cancellationToken);

        var result = JsonSerializer.Deserialize<CaptchaResult>(body);

        if (result == null)
        {
            return new RecognizeResult("");
        }

        if (result.Code == 0)
        {
            return new RecognizeResult(result.Data!.Recognition);
        }

        return new RecognizeResult(result.Message!);
    }

    private class CaptchaResult
    {
        [JsonPropertyName("code")]
        public int Code { get; set; }

        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("data")]
        public CaptchaResultData? Data { get; set; }
    }

    private class CaptchaResultData
    {
        [JsonPropertyName("recognition")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public int Recognition { get; set; }
    }
}
