// See https://aka.ms/new-console-template for more information
using Lantern;
using Lantern.Blazor;
using Lantern.Platform;
using Lantern.Threading;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    [STAThread]
    public static void Main()
    {

        ServiceCollection services = new();
        services.AddBlazorWebView();
        services.AddLanternWin32Platform();
        services.AddSingleton<BlazorWindowManager>();
        services.AddSingleton(new WebViewEnvironmentOptions());

        var app = services.BuildServiceProvider();

        var manager = app.GetRequiredService<BlazorWindowManager>();

        var threading = app.GetRequiredService<IPlatformThreadingInterface>();

        Dispatcher.InitializeUIThread(threading);

        var window = manager.CreateAsync(new Lantern.Windows.WebViewWindowOptions
        {
            Title = "aaa"
        });

        Dispatcher.UIThread.MainLoop(default);

    }
}

