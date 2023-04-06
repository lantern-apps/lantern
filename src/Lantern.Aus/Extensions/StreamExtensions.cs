namespace Lantern.Aus;

internal static class StreamExtensions
{
    public static async Task<int> CopyBufferedToAsync(
        this Stream source,
        Stream destination,
        byte[] buffer,
        CancellationToken cancellationToken = default)
    {
        var bytesCopied = await source.ReadAsync(buffer, cancellationToken);
        await destination.WriteAsync(buffer, 0, bytesCopied, cancellationToken);
        return bytesCopied;
    }
}