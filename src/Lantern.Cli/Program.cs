using System.CommandLine;
using System.Diagnostics;

namespace Lantern.Cli;

class Program
{
    static int Main(string[] args)
    {
        Option<string> fileOption = new("--msg", "The file to read and display on the console.");
        RootCommand rootCommand = new("Sample app for System.CommandLine");
        Command newCommand = new("new", "Read and display the file.") { fileOption };
        rootCommand.AddCommand(newCommand);

        newCommand.SetHandler(msg => New(msg), fileOption);

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