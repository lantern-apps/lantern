using AutoUpdates;
using System.CommandLine;
using System.Diagnostics;

namespace Lantern.Cli;

class Program
{
    //mage --name UT --targetDir ./publish --version 4.0.1 --mapFileExtension
    static int Main(string[] args)
    {
        RootCommand rootCommand = new("Lantern Cli");

        CreateMageCommand(rootCommand);
        CreatePublishCommand(rootCommand);

        return rootCommand.InvokeAsync(args).Result;
    }

    private static void CreateMageCommand(RootCommand rootCommand)
    {
        Option<string> nameOption = new("--name", "The application name");
        Option<string> targetDirOption = new(name: "--targetDir", description: "The target directory.", getDefaultValue: () => ".\\publish");
        Option<bool> mapFileExtensionOptions = new("--mapFileExtension", "Map file extension.");
        Option<Version> versionOption = new(name: "--version", description: "The application version.", parseArgument: result =>
        {
            if (result.Tokens.Count != 1)
            {
                result.ErrorMessage = "--version requires one arguments";
                return null!;
            }

            var value = result.Tokens[0].Value;
            if (!Version.TryParse(value, out var v))
            {
                result.ErrorMessage = $"--version cannot parse '{value}' to version";
                return null!;
            }

            return v;
        })
        {
            IsRequired = true,
        };

        Command mageCommand = new("mage", "Manifest Generation and Editing Tool.") { nameOption, targetDirOption, versionOption, mapFileExtensionOptions };

        rootCommand.AddCommand(mageCommand);
        mageCommand.SetHandler(Mage,
                               nameOption,
                               versionOption,
                               targetDirOption,
                               mapFileExtensionOptions);
    }

    private static void CreatePublishCommand(RootCommand rootCommand)
    {
        Command command = new("publish", "Manifest Generation and Editing Tool.");

        rootCommand.AddCommand(command);
        command.SetHandler(Publish);
    }

    internal static void New(string projectName)
    {
        Console.WriteLine(projectName);
    }

    internal static async Task Mage(string name,
                                    Version version,
                                    string targetDir,
                                    bool mapFileExtension)
    {
        targetDir = Path.GetFullPath(targetDir);
        var path = Path.Combine(targetDir, version.ToString());
        Directory.CreateDirectory(path);

        AusManifest manifest = await AusManifest.LoadAsync(name,
                                                   version,
                                                   Environment.CurrentDirectory,
                                                   Array.Empty<string>());

        AusApplication app = new()
        {
            Name = name,
            Versions = new AusAppVersion[]
            {
                new AusAppVersion
                {
                    Version = version,
                    MapFileExtensions = mapFileExtension,
                }
            }
        };

        CopyDirectory(Environment.CurrentDirectory, path, mapFileExtension);

        app.SaveAs(Path.Combine(targetDir, "application.json"));
        manifest.SaveAs(Path.Combine(path, ".manifest"));

        Console.WriteLine($"Generate application manifest");
        Console.WriteLine("Generate completed.");
        Console.WriteLine();
    }

    internal static Task Publish()
    {
        return Process.Start(new ProcessStartInfo
        {
            WorkingDirectory = Environment.CurrentDirectory,
            FileName = "dotnet",
            Arguments = "publish --self-contained -c release -p:PublishSingleFile=true -p:TrimmerRemoveSymbols=true -p:DebuggerSupport=false -p:TrimMode=partial -p:PublishTrimmed=true -p:RuntimeIdentifier=win-x64",
            UseShellExecute = false,
            CreateNoWindow = false,

        }).WaitForExitAsync();
    }

    static void CopyDirectory(string sourceDirPath, string destDirPath, bool mapFileExtension)
    {
        Directory.CreateDirectory(destDirPath);

        // Copy files
        foreach (var sourceFilePath in Directory.GetFiles(sourceDirPath))
        {
            string destFileName = Path.GetFileName(sourceFilePath);

            string destFilePath;
            if (mapFileExtension)
            {
                destFilePath = Path.Combine(destDirPath, destFileName + ".deploy");
            }
            else
            {
                destFilePath = Path.Combine(destDirPath, destFileName);
            }

            File.Copy(sourceFilePath, destFilePath, true);
            Console.WriteLine($"Copy file {sourceFilePath}");
        }

        // Copy subdirectories recursively
        foreach (var sourceSubDirPath in Directory.GetDirectories(sourceDirPath))
        {
            if (destDirPath.StartsWith(sourceSubDirPath))
            {
                continue;
            }
            var destSubDirName = Path.GetFileName(sourceSubDirPath);
            var destSubDirPath = Path.Combine(destDirPath, destSubDirName);
            CopyDirectory(sourceSubDirPath, destSubDirPath, mapFileExtension);
        }
    }
}
