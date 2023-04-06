using Lantern.Platform;
using Lantern.Win32;
using Microsoft.Extensions.DependencyInjection;

namespace Lantern;

public static class Win32PlatformServiceCollectionExtensions
{
    public static IServiceCollection AddLanternWin32Platform(this IServiceCollection services)
    {
        services.AddSingleton<IClipboard, ClipboardImpl>();
        services.AddSingleton<IDialogPlatform, Win32DialogPlatform>();
        services.AddSingleton<IWindowingPlatform>(Win32Platform.Instance);
        services.AddSingleton<INotifyingPlatform>(Win32Platform.Instance);
        services.AddSingleton<IPlatformThreadingInterface>(Win32Platform.Instance);
        services.AddSingleton<ISingleProcessInstanceManager, Win32SingleProcessInstanceManager>();
        return services;
    }
}