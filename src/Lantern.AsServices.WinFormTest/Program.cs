using Microsoft.Extensions.DependencyInjection;

namespace Lantern.AsService.WinFormTest;

internal static class Program
{
    public static IServiceProvider Services { get; private set; } = null!;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    //[STAThread]
    static void Main()
    {
        ServiceCollection serviceCollection = new();
        serviceCollection.AddLanternAsService(options =>
        {
            options.AddVirtualHostMapping("app.lantern", "wwwroot");
        });
        serviceCollection.AddTransient<Form1>();

        Services = serviceCollection.BuildServiceProvider();
        Services.GetRequiredService<LanternService>().Start();

        var form = Services.GetRequiredService<Form1>();
        Application.Run(form);

        Console.ReadLine();
    }

}