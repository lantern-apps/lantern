﻿using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

internal class Program
{
    private static readonly TextWriter _logger = File.CreateText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"update_{DateTime.Now:yyyy_MM_dd_HH_mm_ss}.log"));
    private static string _entryFile = null!;
    private static string _targetDir = null!;
    private static string _sourceDir = null!;
    private static bool _restart = false;
    private static string? _args = null;

    //"C:\Dev\gitee\staiia\robos-khaos\Robos.Khaos\UT\bin\Debug\net7.0\UT.dll" "C:\Dev\gitee\staiia\robos-khaos\Robos.Khaos\UT\bin\Debug\net7.0" "C:\Users\xiaox\AppData\Local\UT\UpdateData\4.0.1" "True" ""
    private static void Main(string[] args)
    {
        //args = new string[5];
        //args[0] = @"C:\Dev\gitee\staiia\robos-khaos\Robos.Khaos\UT\bin\Debug\net7.0\UT.dll";
        //args[1] = @"C:\Dev\gitee\staiia\robos-khaos\Robos.Khaos\UT\bin\Debug\net7.0";
        //args[2] = @"C:\Users\xiaox\AppData\Local\UT\UpdateData\4.0.1";
        //args[3] = @"True";
        //args[4] = @"";

        if (args.Length < 3)
        {
            WriteLog("miss argument");
            Exit();
        }

        _entryFile = args[0];
        _targetDir = args[1];
        _sourceDir = args[2];

        if (args.Length >= 4)
        {
            if (!bool.TryParse(args[3], out _restart))
            {
                WriteLog("argument 'restart' cannot be parse to bool value");
                Exit();
            }
        }

        if (args.Length >= 5)
        {
            try
            {
                _args = Encoding.UTF8.GetString(Convert.FromBase64String(args[4]));
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
        // Wait until updatee is writable to ensure all running instances have exited
        WriteLog("wait for caller exit");
        int i = 0;
        while (!CheckWriteAccess(_entryFile))
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
        WriteLog($"source directory {_sourceDir}");
        WriteLog($"target directory {_targetDir}");

        CopyDirectory(_sourceDir, _targetDir);

        if (_restart)
        {
            WriteLog($"restart {_entryFile}");
            Restart(_targetDir);
        }

        WriteLog("delete temp files");
        Directory.Delete(_sourceDir, true);
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
            Arguments = _args,
            UseShellExecute = true // avoid sharing console window with updatee
        };

        // If updatee is an .exe file - start it directly
        if (string.Equals(Path.GetExtension(_entryFile), ".exe", StringComparison.OrdinalIgnoreCase))
        {
            startInfo.FileName = _entryFile;
        }
        // If not - figure out what to do with it
        else
        {
            // If there's an .exe file with same name - start it instead
            // Security vulnerability?
            var exe = Path.ChangeExtension(_entryFile, ".exe");
            if (File.Exists(exe))
            {
                startInfo.FileName = exe;
            }
            // Otherwise - start the updatee using dotnet SDK
            else
            {
                startInfo.FileName = "dotnet";
                startInfo.Arguments = $"{_entryFile} {_args}";
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