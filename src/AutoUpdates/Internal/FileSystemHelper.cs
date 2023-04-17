using System.Security.Cryptography;

namespace Lantern.Aus.Internal;

internal static class FileSystemHelper
{
    public static bool CheckDirectoryWriteAccess(string dirPath)
    {
        var testFilePath = Path.Combine(dirPath, $"{Guid.NewGuid()}");

        try
        {
            File.WriteAllText(testFilePath, "");
            File.Delete(testFilePath);

            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }

    public static bool CheckFileWriteAccess(string filePath)
    {
        try
        {
            File.Open(filePath, FileMode.Open, FileAccess.Write).Dispose();
            return true;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
        catch (IOException)
        {
            return false;
        }
    }


    public static async Task<byte[]> ComputeSha256Async(string filePath, CancellationToken cancellationToken = default)
    {
        using var stream = File.OpenRead(filePath);
        using var mdhashAlgorithm = SHA256.Create();
        return await mdhashAlgorithm.ComputeHashAsync(stream, cancellationToken);
    }

    public static byte[] ComputeSha256(string filePath)
    {
        using var stream = File.OpenRead(filePath);
        using var mdhashAlgorithm = SHA256.Create();
        return mdhashAlgorithm.ComputeHash(stream);
    }
}

