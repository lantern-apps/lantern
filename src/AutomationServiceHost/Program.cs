// See https://aka.ms/new-console-template for more information
using Lantern;
using Lantern.AsService;
using Microsoft.Extensions.DependencyInjection;


ServiceCollection serviceCollection = new();
serviceCollection.AddLanternAsService(options =>
{
    options.UserDataFolder = null;
});

var services = serviceCollection.BuildServiceProvider();
var manager = services.GetRequiredService<IWebBrowserManager>();
var host = services.GetRequiredService<LanternService>();
await host.StartAsync();

var window1 = await manager.CreateAsync(new Lantern.Windows.WebViewWindowOptions
{
    Url = "https://www.tiktok.com/",
    ProfileName = "a2",
    Title = "a2",
    WindowTitleHandling = Lantern.Windows.WebViewWindowTitleHandling.AlwaysUseUserSetting,
    NewWindowHandling = Lantern.Windows.WebViewNewWindowHandling.WebViewDefault,
    AreDevToolsEnabled = true,
    AreBrowserAcceleratorKeysEnabled = true,
});

var window2 = await manager.CreateAsync(new Lantern.Windows.WebViewWindowOptions
{
    Url = "https://www.tiktok.com/",
    ProfileName = "a1",
    Title = "a1",
    WindowTitleHandling = Lantern.Windows.WebViewWindowTitleHandling.AlwaysUseUserSetting,
    NewWindowHandling = Lantern.Windows.WebViewNewWindowHandling.WebViewDefault,
    AreDevToolsEnabled = true,
    AreBrowserAcceleratorKeysEnabled = true,
});

Console.ReadLine();