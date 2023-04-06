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

}

