using System.CommandLine;
using System.Diagnostics;

namespace Lantern.Cli;

class Program
{
    static int Main(string[] args)
    {
        var fileOption = new Option<string>(
            name: "--msg",
            description: "The file to read and display on the console.");


        var rootCommand = new RootCommand("Sample app for System.CommandLine");

        var newCommand = new Command("new", "Read and display the file.")
            {
                fileOption,
            };
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
}