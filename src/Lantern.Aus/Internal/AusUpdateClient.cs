using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Lantern.Aus.Internal;

internal class AusUpdateClient : IDisposable
{
    private readonly string _url;
    private readonly string _package;
    private readonly HttpClient _httpClient;

    public AusUpdateClient(string url, string package)
    {
        if (url == null)
            throw new ArgumentNullException(nameof(url));

        if (url.EndsWith('/'))
            _url = url.Substring(0, url.Length - 1);
        else
            _url = url;

        _package = package;
        _httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (a, b, c, d) => true
        });
    }

    public async Task<AusManifest?> GetUpdateAsync(Version version, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{_url}/packages/{_package}/update?version={version}", cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
            response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var error = await response.Content.ReadFromJsonAsync<AusErrorResponse>(cancellationToken: cancellationToken);
            if (error != null)
                throw new AusException(error.Title, error.Detail);
        }
        response.EnsureSuccessStatusCode();

        var manifest = await response.Content.ReadFromJsonAsync<AusManifest>(cancellationToken: cancellationToken);
        return manifest!;
    }

    public async Task<AusManifest?> GetLatestManifestAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{_url}/packages/{_package}/manifest/latest", cancellationToken);
        response.EnsureSuccessStatusCode();
        if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            return null;

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
            response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var error = await response.Content.ReadFromJsonAsync<AusErrorResponse>(cancellationToken: cancellationToken);
            if (error != null)
                throw new AusException(error.Title, error.Detail);
        }
        response.EnsureSuccessStatusCode();

        var manifest = await response.Content.ReadFromJsonAsync<AusManifest>(cancellationToken: cancellationToken);
        return manifest!;
    }

    public async Task DownloadAsync(
        Version version,
        string name,
        string destFilePath,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var fileUrl = $"{_url}/packages/{_package}/file?version={version}&name={name}";

        using var response = await _httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest ||
            response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var error = await response.Content.ReadFromJsonAsync<AusErrorResponse>(cancellationToken: cancellationToken);
            if (error != null)
                throw new AusException(error.Title, error.Detail);
        }
        response.EnsureSuccessStatusCode();

        var dir = Path.GetDirectoryName(destFilePath)!;
        Directory.CreateDirectory(dir);
        using var output = File.Create(destFilePath);
        await response.Content.CopyToStreamAsync(output, progress, cancellationToken);
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }

    private class AusErrorResponse
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("detail")]
        public string? Detail { get; set; }
    }
}
