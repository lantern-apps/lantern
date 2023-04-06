using Lantern;
using Lantern.Windows;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

LanternAppHostBuilder.Create()
    .AddFeature<InitService>()
    .AddWindow(options =>
    {
        options.Name = "defualt";
        options.Url = "index.html";
        options.AreDevToolsEnabled = true;
        options.AreBrowserAcceleratorKeysEnabled = true;
    })
    .Build()
    .Run();

public class InitService : ILanternService
{
    private readonly IAppLifetime lifetime;
    private readonly IWindowManager _windowManager;

    public InitService(IAppLifetime lifetime, IWindowManager windowManager)
    {
        this.lifetime = lifetime;
        _windowManager = windowManager;
    }

    public void Initialize()
    {
        lifetime.ApplicationStarted.Register(async () =>
        {
            await Task.Delay(10000);
            var window = (WebViewWindow)_windowManager.GetDefaultWindow()!;
            await window.EnsureWebViewInitializedAsync();

            Dictionary<string, string> a = new()
            {
                {"c", "" },
                {"b", "" },
                {"a", "" },
                {"3", "" },
                {"2", "" },
                {"1", "" },
            };
            var json = JsonSerializer.Serialize(a);
            window.PostMessage(json);
        });
    }
}