using System.CommandLine;
using System.Diagnostics;

namespace Lantern.Cli;

class Program
{
    static int Main(string[] args)
    {
        RootCommand rootCommand = new("Sample app for System.CommandLine");

        Option<string> fileOption = new("--msg", "The file to read and display on the console.");
        Command newCommand = new("new", "Read and display the file.") { fileOption };

        Option<string> updateOption = new("--update", "The file to read and display on the console.");
        Command publishCommand = new("publish", "Read and display the file.") { fileOption };

        rootCommand.AddCommand(newCommand);
        newCommand.SetHandler(msg => New(msg), fileOption);
        publishCommand.SetHandler(update => Publish(), updateOption);

        return rootCommand.InvokeAsync(args).Result;
    }

    internal static void New(string projectName)
    {
        Console.WriteLine(projectName);
    }

    internal static async Task Build()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            ArgumentList =
            {
                "build"
            }
        });
    }

    internal static async Task Publish()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "dotnet",
            ArgumentList =
            {
                "build"
            }
        });
    }

}