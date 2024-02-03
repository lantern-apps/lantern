using Microsoft.Extensions.DependencyInjection;

namespace Lantern.Blazor;

public class PhotinoBlazorApp
{
    /// <summary>
    /// Gets configuration for the service provider.
    /// </summary>
    public IServiceProvider Services { get; private set; }

    /// <summary>
    /// Gets configuration for the root components in the window.
    /// </summary>
    public BlazorWindowRootComponents RootComponents { get; private set; }

    internal void Initialize(IServiceProvider services, RootComponentList rootComponents)
    {
        Services = services;
        RootComponents = Services.GetRequiredService<BlazorWindowRootComponents>();
        MainWindow = Services.GetService<BlazorWebViewWindow>();
        WindowManager = Services.GetRequiredService<LanternWebViewManager>();

        //MainWindow
        //    .SetTitle("Photino.Blazor App")
        //    .SetUseOsDefaultSize(false)
        //    .SetUseOsDefaultLocation(false)
        //    .SetWidth(1000)
        //    .SetHeight(900)
        //    .SetLeft(450)
        //    .SetTop(100);

        foreach (var component in rootComponents)
        {
            RootComponents.Add(component.Item1, component.Item2);
        }
    }

    public BlazorWebViewWindow MainWindow { get; private set; }

    public LanternWebViewManager WindowManager { get; private set; }
}
