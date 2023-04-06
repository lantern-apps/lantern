// See https://aka.ms/new-console-template for more information
using Lantern;
using Lantern.Windows;
using Microsoft.Extensions.Logging;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var builder = LanternAppHostBuilder.Create()
            .ConfigureLogging(logging =>
            {
                logging.AddConsole();
                logging.AddFilter((string? category, LogLevel level)
                    => level >= LogLevel.Information
                    || category!.StartsWith("Lantern.")
                    || category!.StartsWith("Robos.")
                    || category!.StartsWith("UT."));
            })
            .AddWindow(options =>
            {
                options.Name = "splash";
                options.Title = "Splash Screen";
                options.Url = "index.html";
                //options.Url = "http://localhost:5173";
                options.Width = 1280;
                options.Height = 720;
                options.Visible = true;
                options.Center = true;
                options.SkipTaskbar = false;
                options.Resizable = true;
                options.TitleBarStyle = WindowTitleBarStyle.Default;
                options.AreBrowserAcceleratorKeysEnabled = true;
                options.AreDevToolsEnabled = true;
                //options.Decorations = WindowDecorations.BorderOnly;
            })
            //.AddWindow(options =>
            //{
            //    options.Name = "main";
            //    options.NewWindowHandling = WebViewNewWindowHandling.NewWebViewWindow;
            //    options.WindowTitleHandling = WebViewWindowTitleHandling.AlwaysUseUserSetting;
            //    options.Title = "main";
            //    options.IconPath = "favicon.ico";
            //    options.Width = 1024;
            //    options.Height = 720;

            //    options.Url = "https://app.lantern/index.html";
            //    //options.Url = "https://map.google.com";
            //    //options.Url = "http://localhost:5173";
            //    options.AreDevToolsEnabled = true;
            //    options.AreBrowserAcceleratorKeysEnabled = true;
            //    options.AreDefaultContextMenusEnabled = true;

            //    options.AreDevToolsEnabled = true;
            //    options.AreBrowserAcceleratorKeysEnabled = true;
            //    options.AreDefaultContextMenusEnabled = true;

            //    options.Visible = true;

            //    options.Center = true;
            //    options.ShowInTaskbar = true;
            //    options.Resizable = true;
            //    options.Decorations = WindowDecorations.Full;
            //})
            //.AddTray(options =>
            //{
            //    options.Name = "main";
            //    options.ToolTip = "Hi Lantern 1";
            //    options.IconPath = "favicon.ico";
            //    options.Visible = false;
            //    options.Menu = new MenuOptions
            //    {
            //        Items = new List<MenuItemOptions>
            //        {
            //            new MenuItemOptions("test","选中", Commands.Shutdown)
            //            {
            //                Checked = true,
            //            },
            //            new MenuItemOptions("test2","禁用", Commands.Shutdown)
            //            {
            //                Enabled = false,
            //            },
            //            new MenuItemOptions
            //            {
            //                Text = "子菜单",
            //                Type = MenuItemType.SubMenu,
            //                Items = new MenuItemOptions[]
            //                {
            //                    new MenuItemOptions("subItem1","子菜单1"),
            //                    new MenuItemOptions("subItem2","子菜单2"),
            //                }
            //            }
            //        }
            //    };
            //})
            .AddTray(options =>
            {
                options.Name = "secondary";
                options.ToolTip = "Hi Lantern 2";
                options.IconPath = "favicon.ico";
                options.Visible = true;
            });

        builder.Build().Run();
    }
}