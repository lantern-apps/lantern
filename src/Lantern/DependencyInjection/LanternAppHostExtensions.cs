using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Lantern;

public static class LanternAppHostExtensions
{
    public static async Task RunLanternAppAsync(this IHost host)
    {
        var app = host.Services.GetRequiredService<ILanternHost>();
        var lifetime = host.Services.GetRequiredService<IAppLifetime>();
        lifetime.ApplicationStarted.Register(() => host.StartAsync());
        lifetime.ApplicationStopping.Register(() => host.StopAsync());
        await app.StartAsync();
        await host.WaitForShutdownAsync();
    }

    public static void RunLanternApp(this IHost host)
    {
        var app = host.Services.GetRequiredService<ILanternHost>();
        var lifetime = host.Services.GetRequiredService<IAppLifetime>();
        lifetime.ApplicationStarted.Register(() => host.StartAsync());
        lifetime.ApplicationStopping.Register(() => host.StopAsync());
        app.Run();
    }
}