using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

//args = new string[]
//{
//    @"C:\Dev\Robos\robos-khaos\Robos.Khaos\UT.Robotics.Server\bin\Debug\net7.0\UTService.dll",
//    @"C:\Users\xiaox\AppData\Local\Robos.UpdateService\UTService\1.0.3" ,
//    "True" ,
//    "",
//};


internal class Program
{
    private static readonly TextWriter _logger = File.CreateText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"update_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
    private static string _updateeFilePath = null!;
    private static string _packageContentDirPath = null!;
    private static bool _restartUpdatee = false;
    private static string? _routedArgs = null;

    private static void Main(string[] args)
    {
        if (args.Length < 2)
        {
            WriteLog("miss argument");
            Exit();
        }

        _updateeFilePath = args[0];
        _packageContentDirPath = args[1];

        if (args.Length >= 3)
        {
            if (!bool.TryParse(args[2], out _restartUpdatee))
            {
                WriteLog("argument 'restart' cannot be parse to bool value");
                Exit();
            }
        }

        if (args.Length >= 4)
        {
            try
            {
                _routedArgs = Encoding.UTF8.GetString(Convert.FromBase64String(args[3]));
            }
            catch
            {
                WriteLog("argument 'routedArgs' cannot be parse from base64");
                Exit();
            }
        }

        try
        {
            RunCore();
        }
        catch (Exception ex)
        {
            WriteLog(ex.ToString());
        }

        Exit();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void RunCore()
    {
        var updateeDirPath = Path.GetDirectoryName(_updateeFilePath)!;

        // Wait until updatee is writable to ensure all running instances have exited
        WriteLog("wait for caller exit");
        int i = 0;
        while (!CheckWriteAccess(_updateeFilePath))
        {
            if (i > 100)
            {
                WriteLog("wait for caller exit timeout");
                return;
            }

            Thread.Sleep(100);
            i++;
        }

        // Copy over the package contents
        WriteLog($"source directory {_packageContentDirPath}");
        WriteLog($"target directory {updateeDirPath}");

        CopyDirectory(_packageContentDirPath, updateeDirPath);

        if (_restartUpdatee)
        {
            Restart(updateeDirPath);
        }

        WriteLog("delete temp files");
        Directory.Delete(_packageContentDirPath, true);
    }

    [DoesNotReturn]
    static void Exit()
    {
        _logger.Dispose();
        Environment.Exit(0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static void Restart(string? updateeDirPath)
    {
        var startInfo = new ProcessStartInfo
        {
            WorkingDirectory = updateeDirPath,
            Arguments = _routedArgs,
            UseShellExecute = true // avoid sharing console window with updatee
        };

        // If updatee is an .exe file - start it directly
        if (string.Equals(Path.GetExtension(_updateeFilePath), ".exe", StringComparison.OrdinalIgnoreCase))
        {
            startInfo.FileName = _updateeFilePath;
        }
        // If not - figure out what to do with it
        else
        {
            // If there's an .exe file with same name - start it instead
            // Security vulnerability?
            var exe = Path.ChangeExtension(_updateeFilePath, ".exe");
            if (File.Exists(exe))
            {
                startInfo.FileName = exe;
            }
            // Otherwise - start the updatee using dotnet SDK
            else
            {
                startInfo.FileName = "dotnet";
                startInfo.Arguments = $"{_updateeFilePath} {_routedArgs}";
            }
        }

        WriteLog($"start {startInfo.FileName} {startInfo.Arguments}");
        using var _ = Process.Start(startInfo);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static bool CheckWriteAccess(string filePath)
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

    static void CopyDirectory(string sourceDirPath, string destDirPath, bool root = true)
    {
        Directory.CreateDirectory(destDirPath);

        // Copy files
        foreach (var sourceFilePath in Directory.GetFiles(sourceDirPath))
        {
            var destFileName = Path.GetFileName(sourceFilePath);
            if (root && destFileName == ".patch")
                continue;

            var destFilePath = Path.Combine(destDirPath, destFileName);
            File.Copy(sourceFilePath, destFilePath, true);
        }

        // Copy subdirectories recursively
        foreach (var sourceSubDirPath in Directory.GetDirectories(sourceDirPath))
        {
            var destSubDirName = Path.GetFileName(sourceSubDirPath);
            var destSubDirPath = Path.Combine(destDirPath, destSubDirName);
            CopyDirectory(sourceSubDirPath, destSubDirPath, false);
        }
    }

    static void WriteLog(string content)
    {
        _logger.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {content}");
        _logger.Flush();
    }

}