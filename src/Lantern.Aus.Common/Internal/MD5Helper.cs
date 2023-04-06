using System.Security.Cryptography;

namespace Lantern.Aus.Internal;

internal static class MD5Helper
{
    public static async Task<Guid> ComputeMd5Async(Stream stream, CancellationToken cancellationToken = default)
    {
        using var mdhashAlgorithm = MD5.Create();
        var bytes = await mdhashAlgorithm.ComputeHashAsync(stream, cancellationToken);
        return new Guid(bytes);
    }

    public static async Task<byte[]> ComputeMd5Async(string filePath, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(filePath);
        using var mdhashAlgorithm = MD5.Create();
        return  await mdhashAlgorithm.ComputeHashAsync(stream, cancellationToken);
    }

    public static byte[] ComputeMd5(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var mdhashAlgorithm = MD5.Create();
        return mdhashAlgorithm.ComputeHash(stream);
    }

}

