using System.IO;

namespace Lantern.Aus.Internal;

internal partial class LockFile : IDisposable
{
    private readonly FileStream _fileStream;

    public LockFile(FileStream fileStream) => _fileStream = fileStream;

    public void Dispose()
    {
        var path = _fileStream.Name;
        _fileStream.Dispose();

        try
        {
            if (File.Exists(path))
                File.Delete(path);
        }
        catch { }
    }

    public static LockFile? TryAcquire(string filePath)
    {
        try
        {
            var fileStream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            return new LockFile(fileStream);
        }
        // This is the most specific exception for "access denied"
        catch (IOException)
        {
            return null;
        }
    }

}