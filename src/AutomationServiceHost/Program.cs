// See https://aka.ms/new-console-template for more information
using AutomationServiceHost.Services;
using Lantern;
using Lantern.AsService;
using Microsoft.Extensions.DependencyInjection;


ServiceCollection serviceCollection = new();
serviceCollection.AddLanternAsService(options =>
{
    options.UserDataFolder = null;
});

var services = serviceCollection.BuildServiceProvider();

var host = services.GetRequiredService<LanternService>();
await host.StartAsync();

var manager = new TiktokSessionManager(services.GetRequiredService<IWebBrowserManager>());

await manager.CreateAsync("xiao.xin@outlook.com", "P#ssw0rd!");
await manager.CreateAsync("adam.xx@qq.com", "adaxin()123");

//await manager.CreateAsync("a1");
//await manager.CreateAsync("a2");

await manager.BlukAsync(async (session, ct) =>
{
    await session.LoginAsync(ct);
    await session.FollowAsync("tea520530");
});

//await manager.BlukAsync((session, ct) =>
//{
//    return session.PublishVideoAsync("D:\\我的桌面\\智能文档传作助手.mp4", ct);
//});

Console.ReadLine();

