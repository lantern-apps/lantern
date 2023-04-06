using Lantern.Messaging;
using Lantern.Windows;
using System.Diagnostics;

namespace Lantern.Controllers;

internal sealed class WindowController : WindowControllerBase
{
    private readonly IWindowManager _windowManager;

    public WindowController(IWindowManager windowManager)
    {
        _windowManager = windowManager;
    }

    public override void Close(IpcContext context) => context.Window.Close(true);
    public override void Hide(IpcContext context) => context.Window.Hide();
    public override void Show(IpcContext context) => context.Window.Show();
    public override void Center(IpcContext context) => context.Window.Center();
    public override void Restore(IpcContext context) => context.Window.Restore();

    public override bool IsMaximized(IpcContext context) => context.Window.IsMaximized;
    public override void Maximize(IpcContext context) => context.Window.Maximize();

    public override bool IsMinimized(IpcContext context) => context.Window.IsMinimized;
    public override void Minimize(IpcContext context) => context.Window.Minimize();

    public override bool IsFullScreen(IpcContext context) => context.Window.IsFullScreen;
    public override void FullScreen(IpcContext context) => context.Window.FullScreen();

    public override bool IsVisible(IpcContext context) => context.Window.IsVisible;

    public override bool IsActive(IpcContext context) => context.Window.IsActive;
    public override void Activate(IpcContext context) => context.Window.Activate();

    public override bool IsAlwaysOnTop(IpcContext context) => context.Window.AlwaysOnTop;
    public override void SetAlwaysOnTop(IpcContext<bool> context) => context.Window.AlwaysOnTop = context.Body;

    public override LogisticSize GetSize(IpcContext context) => context.Window.Size;
    public override void SetSize(IpcContext<LogisticSize> context) => context.Window.Size = context.Body;

    public override LogisticPosition GetPosition(IpcContext context) => context.Window.Position;
    public override void SetPosition(IpcContext<LogisticPosition> context) => context.Window.Position = context.Body;

    public override string[] GetAll(IpcContext context) => _windowManager.GetAllWindows().Select(x => x.Name!).ToArray();

    public override string? GetTitle(IpcContext context) => context.Window.Title;
    public override void SetTitle(IpcContext<string> context) => context.Window.Title = context.Body;

    public override Task<string?> GetUrl(IpcContext context) => context.Window.GetUrlAsync();
    public override void SetUrl(IpcContext<string> context) => context.Window.SetUrlAsync(context.Body);

    public override LogisticSize GetMinSize(IpcContext context) => context.Window.MinSize;
    public override void SetMinSize(IpcContext<LogisticSize> context) => context.Window.MinSize = context.Body;

    public override LogisticSize GetMaxSize(IpcContext context) => context.Window.MaxSize;
    public override void SetMaxSize(IpcContext<LogisticSize> context) => context.Window.MaxSize = context.Body;

    public override bool IsSkipTaskbar(IpcContext context) => context.Window.SkipTaskbar;
    public override void SetSkipTaskbar(IpcContext<bool> context) => context.Window.SkipTaskbar = context.Body;

    public override bool IsResizable(IpcContext context) => context.Window.Resizable;
    public override void SetResizable(IpcContext<bool> context) => context.Window.Resizable = context.Body;

    public override WindowTitleBarStyle GetTitleBarStyle(IpcContext context) => context.Window.TitleBarStyle;
    public override void SetTitleBarStyle(IpcContext<WindowTitleBarStyle> context) => context.Window.TitleBarStyle = context.Body;

    public override void StartDragMove(IpcContext context) => context.Window.StartDragMove();

    public override async Task Create(IpcContext<WindowCreateOptions> context)
    {
        var options = context.Body;

        string? url;
        if (options.Url != null && !Uri.IsWellFormedUriString(options.Url, UriKind.Absolute))
        {
            if (!Uri.IsWellFormedUriString(options.Url, UriKind.Relative))
            {
                throw new ArgumentException($"Invaild Url '{options.Url}'");
            }

            var currentUrl = await context.Window.GetUrlAsync();

            Debug.Assert(!string.IsNullOrEmpty(currentUrl));

            var uri = new Uri(currentUrl);
            url = $"{uri.Scheme}://{uri.Host}/{options.Url}";
        }
        else
        {
            url = options.Url;
        }

        await _windowManager.CreateWindowAsync(new WebViewWindowOptions
        {
            Name = options.Name,
            Title = options.Title,
            Url = url,
            Visible = options.Visible,
            Width = options.Width,
            Height = options.Height,
            X = options.X,
            Y = options.Y,
            MinWidth = options.MinWidth,
            MinHeight = options.MinHeight,
            MaxWidth = options.MaxWidth,
            MaxHeight = options.MaxHeight,
            Center = options.Center,
            AwaysOnTop = options.AlwaysOnTop,
            SkipTaskbar = options.SkipTaskbar,
            Resizable = options.Resizeable,
            TitleBarStyle = options.TitleBarStyle,
        });
    }

}
