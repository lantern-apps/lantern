using Lantern;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRazorPages();
        builder.Services.AddLanternApp()
            .AddWindow(options =>
            {
                options.Name = "main";
                options.AreDevToolsEnabled = true;
                options.AreBrowserAcceleratorKeysEnabled = true;
                options.Title = "Robos";
                options.Url = "http://localhost:5298/";
                options.Visible = true;
            })
            .AddTray(options =>
            {
                options.IconPath = "favicon.ico";
            });


        var app = builder.Build();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();
        app.MapRazorPages();

        app.RunLanternApp();
    }
}