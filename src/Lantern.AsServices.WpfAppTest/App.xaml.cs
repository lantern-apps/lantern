using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Lantern.AsServices.WpfAppTest
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; private set; }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddLanternWin32Platform();
                    services.AddLanternService();
                })
                .Build();

            Services = host.Services;

            var app = host.Services.GetRequiredService<LanternService>();
            //var task = host.RunAsync(app.OnShutdown).ContinueWith(t => app.Shutdown());
            //await Task.Yield();
            //app.Run();
            //await task;
            app.Run();
        }
    }
}
