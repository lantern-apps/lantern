using Lantern.AsService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Lantern;

public static class LanternAsServiceCollectionExtensions
{
    public static IServiceCollection AddLanternAsService(this IServiceCollection services, Action<WebViewEnvironmentOptions>? optionsAction = null)
    {
        WebViewEnvironmentOptions options = new();
        optionsAction?.Invoke(options);

        services.TryAddSingleton(options);
        services.AddSingleton<IWebBrowserManager, WebBrowserManager>();
        services.AddSingleton<LanternService>();
        services.AddSingleton<ILanternHost>(x => x.GetRequiredService<LanternService>());
        services.AddLogging();

        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            services.AddLanternWin32Platform();

        return services;
    }
}