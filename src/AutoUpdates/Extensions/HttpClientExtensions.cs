using Lantern.Aus.Internal;
using System.Net.Http.Json;

namespace AutoUpdates;

internal static class HttpClientExtensions
{
    public static async Task<AusManifest?> GetManifestAsync(this HttpClient httpClient, Version version, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"{version}/.manifest", cancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AusManifest?>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            else
                throw new UpdateException(response.StatusCode.ToString(), response.ReasonPhrase);
        }
    }

    public static async Task<AusApplication?> GetApplicationInfoAsync(this HttpClient httpClient, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"application.json", cancellationToken).ConfigureAwait(false);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AusApplication?>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }
        else
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
            else
                throw new UpdateException(response.StatusCode.ToString(), response.ReasonPhrase);
        }
    }

    public static async Task DownloadAsync(
        this HttpClient httpClient,
        Version version,
        string name,
        string destFilePath,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var fileUrl = $"{version}/{name.Replace('\\', '/')}";

        using var response = await httpClient.GetAsync(fileUrl, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        var dir = Path.GetDirectoryName(destFilePath)!;
        Directory.CreateDirectory(dir);
        using var output = File.Create(destFilePath);
        await response.Content.CopyToStreamAsync(output, progress, cancellationToken);
    }

    private static async Task CopyToStreamAsync(
        this HttpContent content,
        Stream destination,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        var length = content.Headers.ContentLength;
        using var source = await content.ReadAsStreamAsync();
        using var buffer = PooledBuffer.ForStream();

        var totalBytesCopied = 0L;
        int bytesCopied;
        do
        {
            bytesCopied = await source.CopyBufferedToAsync(destination, buffer.Array, cancellationToken);
            totalBytesCopied += bytesCopied;

            if (length != null)
                progress?.Report(1.0 * totalBytesCopied / length.Value);
        } while (bytesCopied > 0);
    }

}
