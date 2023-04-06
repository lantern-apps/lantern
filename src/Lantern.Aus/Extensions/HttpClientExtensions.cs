using Lantern.Aus.Internal;

namespace Lantern.Aus;

internal static class HttpClientExtensions
{
    public static async Task DownloadFileAsync(
        this HttpClient httpClient,
        string fileUrl,
        string destFilePath,
        IProgress<double>? progress = null,
        CancellationToken cancellationToken = default)
    {
        using var response = await httpClient.GetAsync(
            fileUrl,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken
        );

        response.EnsureSuccessStatusCode();
        using var output = File.Create(destFilePath);
        await response.Content.CopyToStreamAsync(output, progress, cancellationToken);
    }

    public static async Task CopyToStreamAsync(
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

