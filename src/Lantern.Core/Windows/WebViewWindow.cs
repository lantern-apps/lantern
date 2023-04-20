using Lantern.Platform;
using Lantern.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Drawing;

namespace Lantern.Windows;

[DebuggerDisplay("{Name}")]
public class WebViewWindow : Window, IWebViewWindow
{
    [ThreadStatic]
    private static CoreWebView2Environment? _environment;

    private readonly WebViewEnvironmentOptions _environmentOptions;
    private readonly WebViewWindowOptions _windowOptions;
    private readonly IServiceProvider _serviceProvider;
    private readonly IWindowManager? _windowManager;
    private readonly TaskCompletionSource _cts = new();
    private readonly CancellationTokenSource _closedSource = new();

    private readonly string _name;

    private CoreWebView2Controller? _controller;
    private CoreWebView2? _webview;

    public WebViewWindow(
        WebViewEnvironmentOptions environmentOptions,
        WebViewWindowOptions windowOptions,
        IWindowManager? windowManager,
        IServiceProvider serviceProvider,
        IWindowImpl windowImpl)
        : base(windowImpl)
    {
        _environmentOptions = environmentOptions;
        _windowOptions = windowOptions;
        _name = windowOptions.Name;
        _windowManager = windowManager;
        _serviceProvider = serviceProvider;
        Title = windowOptions.Title;

        MaxSize = new LogisticSize(windowOptions.MaxWidth ?? 0, windowOptions.MaxHeight ?? 0);
        MinSize = new LogisticSize(windowOptions.MinWidth ?? 0, windowOptions.MinHeight ?? 0);
        Size = new LogisticSize(windowOptions.Width ?? Size.Width, windowOptions.Height ?? Size.Height);
        Position = new LogisticPosition(windowOptions.X ?? Position.X, windowOptions.Y ?? Position.Y);

        Icon = windowOptions.IconPath;
        Resizable = windowOptions.Resizable;
        SkipTaskbar = windowOptions.SkipTaskbar;
        TitleBarStyle = windowOptions.TitleBarStyle;
        AlwaysOnTop = windowOptions.AwaysOnTop;

        if (windowOptions.Center)
            Center();

        WindowClosed = _closedSource.Token;

        CreateWebView2Controller();
    }

    public event Action? WebViewInitialized;
    public event Action<string>? WebViewMessageReceived;

    protected internal CoreWebView2? CoreWebView => _webview;

    public string Name => _name;
    public WebViewWindowOptions WindowOptions => _windowOptions;
    public IWindowManager? WindowManager => _windowManager;
    public CancellationToken WindowClosed { get; }

    public async Task<string?> GetUrlAsync()
    {
        if (_webview == null)
            return _windowOptions.Url;

        if (Dispatcher.UIThread.CheckAccess())
        {
            return _webview.Source;
        }
        else
        {
            return await Dispatcher.UIThread.InvokeAsync(() => _webview.Source);
        }
    }

    public async Task SetUrlAsync(string url)
    {
        if (_webview == null)
            return;

        if (Dispatcher.UIThread.CheckAccess())
        {
            _webview.Navigate(url);
        }
        else
        {
            await Dispatcher.UIThread.InvokeAsync(() => _webview.Navigate(url));
        }

    }

    public Task EnsureWebViewInitializedAsync() => _cts.Task;

    public void PostMessage(string messageJson)
    {
        Debug.Assert(_webview != null);

        if (_webview != null)
        {
            if (Dispatcher.UIThread.CheckAccess())
            {
                _webview.PostWebMessageAsJson(messageJson);
            }
            else
            {
                Dispatcher.UIThread.Post(() => _webview.PostWebMessageAsJson(messageJson), DispatcherPriority.Send);
            }
        }
    }


    protected override void OnResized() => ResizeWebViewBounds();

    protected override void OnClosed()
    {
        if (!_closedSource.IsCancellationRequested)
        {
            _closedSource.Cancel(false);
        }

        if (_webview != null)
        {
            _webview.WebMessageReceived -= OnWebViewMessageReceived;
            _webview.NewWindowRequested -= OnWebViewNewWindowRequested;
            _webview.DocumentTitleChanged -= OnWebViewDocumentTitleChanged;
        }

        if (_controller != null)
        {
            _controller.AcceleratorKeyPressed -= OnWebViewAcceleratorKeyPressed;
            _controller.Close();
        }

    }

    protected virtual void OnWebViewInitialized() => WebViewInitialized?.Invoke();

    private void OnWebViewDocumentTitleChanged(object? sender, object e)
    {
        if (_webview != null && _windowOptions.WindowTitleHandling == WebViewWindowTitleHandling.BindWebViewDocumentTitle)
            Title = _webview.DocumentTitle;
    }

    private void OnWebViewAcceleratorKeyPressed(object? sender, CoreWebView2AcceleratorKeyPressedEventArgs e) { }

    protected virtual async void OnWebViewNewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        var handling = _windowOptions.NewWindowHandling;

        if (handling == WebViewNewWindowHandling.WebViewDefault)
        {
            return;
        }

        var url = e.Uri;

        e.Handled = true;

        if (handling == WebViewNewWindowHandling.OpenSystemDefaultBrowser)
        {
            using var _ = Process.Start(new ProcessStartInfo
            {
                FileName = url,
                UseShellExecute = true,
            });
        }
        else if (handling == WebViewNewWindowHandling.NavigationCurrentWebView)
        {
            if (_webview != null)
            {
                _webview?.Navigate(url);
            }
        }
        else if (handling == WebViewNewWindowHandling.NewWebViewWindow)
        {
            if (_windowManager != null)
            {
                await _windowManager.CreateWindowAsync(new WebViewWindowOptions
                {
                    Name = Name + $"_{Guid.NewGuid():N}",
                    Url = url,
                    Visible = true
                });
            }
        }
    }

    private void ResizeWebViewBounds()
    {
        if (_controller != null)
        {
            _controller.Bounds = new Rectangle(0, 0,
                (int)Math.Ceiling(Size.Width * ScaleFactor),
                (int)Math.Ceiling(Size.Height * ScaleFactor));
        }
    }

    private async void CreateWebView2Controller()
    {
        _environment ??= await CoreWebView2Environment.CreateAsync(
            _environmentOptions.BrowserExecutableFolder,
            _environmentOptions.UserDataFolder,
            new CoreWebView2EnvironmentOptions
            {
#if DEBUG
                AdditionalBrowserArguments = "--disable-web-security --remote-debugging-port=9222",
#else
                AdditionalBrowserArguments = "--disable-web-security",
#endif
                Language = _environmentOptions.Language,
                AllowSingleSignOnUsingOSPrimaryAccount = false,
                IsCustomCrashReportingEnabled = false,
                ExclusiveUserDataFolderAccess = false,
                //CustomSchemeRegistrations = 
                //{
                //    new CoreWebView2CustomSchemeRegistration("lantern")
                //    {

                //    }
                //}
            });

        //var controllerOptions = _environment.CreateCoreWebView2ControllerOptions();
        //controllerOptions.ProfileName = "UT";
        //controllerOptions.ScriptLocale = Thread.CurrentThread.CurrentUICulture.Name;
        _controller = await _environment.CreateCoreWebView2ControllerAsync(WindowImpl.NativeHandle/*, controllerOptions*/); ;
        ResizeWebViewBounds();
        _controller.IsVisible = true;

        _controller.AllowExternalDrop = false;
        _controller.BoundsMode = CoreWebView2BoundsMode.UseRawPixels;
        _controller.DefaultBackgroundColor = Color.White;
        _controller.AcceleratorKeyPressed += OnWebViewAcceleratorKeyPressed;

        _webview = _controller.CoreWebView2;
        _webview.NewWindowRequested += OnWebViewNewWindowRequested;
        _webview.DocumentTitleChanged += OnWebViewDocumentTitleChanged;
        _webview.PermissionRequested += OnWebViewPermissionRequested;
        _webview.WindowCloseRequested += (o, e) => Close(true);
        _webview.ProcessFailed += (o, e) => Close(true);

        _webview.Settings.UserAgent = _windowOptions.UserAgent;
        _webview.Settings.AreDefaultContextMenusEnabled = _windowOptions.AreDefaultContextMenusEnabled;
        _webview.Settings.IsZoomControlEnabled = _windowOptions.IsZoomControlEnabled;
        _webview.Settings.AreBrowserAcceleratorKeysEnabled = _windowOptions.AreBrowserAcceleratorKeysEnabled;
        _webview.Settings.AreDevToolsEnabled = _windowOptions.AreDevToolsEnabled;
        _webview.Settings.IsStatusBarEnabled = _windowOptions.IsStatusBarEnabled;
        _webview.Settings.IsPinchZoomEnabled = _windowOptions.IsPinchZoomEnabled;
        _webview.Settings.IsPasswordAutosaveEnabled = _windowOptions.IsPasswordAutosaveEnabled;
        _webview.Settings.IsGeneralAutofillEnabled = _windowOptions.IsGeneralAutofillEnabled;
        _webview.Settings.IsSwipeNavigationEnabled = _windowOptions.IsSwipeNavigationEnabled;
        _webview.Settings.IsScriptEnabled = _windowOptions.IsScriptEnabled;
        _webview.Settings.IsWebMessageEnabled = _windowOptions.IsWebMessageEnabled;
        _webview.Settings.AreHostObjectsAllowed = _windowOptions.AreHostObjectsAllowed;
        _webview.Settings.IsBuiltInErrorPageEnabled = _windowOptions.IsBuiltInErrorPageEnabled;

        if (_environmentOptions.HostObjects != null)
        {
            foreach (var hostObject in _environmentOptions.HostObjects)
            {
                var obj = _serviceProvider.GetRequiredService(hostObject.Value);
                _webview.AddHostObjectToScript(hostObject.Key, obj);
            }
        }

        if (_environmentOptions.VirtualHosts != null && _environmentOptions.VirtualHosts.Count > 0)
        {
            foreach (var map in _environmentOptions.VirtualHosts)
                _webview.SetVirtualHostNameToFolderMapping(map.HostName, map.FolderName, CoreWebView2HostResourceAccessKind.Allow);
        }
        else
        {
            _webview.Settings.AreHostObjectsAllowed = false;
        }

        if (_environmentOptions.JavaScriptsOnDocumentCreated != null)
        {
            foreach (var js in _environmentOptions.JavaScriptsOnDocumentCreated)
                await _webview.AddScriptToExecuteOnDocumentCreatedAsync(js);
        }

        if (_windowOptions.IsWebMessageEnabled)
        {
            _webview.WebMessageReceived += OnWebViewMessageReceived;
        }

        if (!string.IsNullOrEmpty(_windowOptions.Url) && Uri.IsWellFormedUriString(_windowOptions.Url, UriKind.Absolute))
        {
            _webview.Navigate(_windowOptions.Url);
        }

        OnWebViewInitialized();

        if (_windowOptions.Visible)
        {
            Show();
        }

        _cts.SetResult();
    }

    private void OnWebViewMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        WebViewMessageReceived?.Invoke(e.WebMessageAsJson);
    }

    private void OnWebViewPermissionRequested(object? sender, CoreWebView2PermissionRequestedEventArgs e)
    {
        var kind = (WebViewPermissionKind)e.PermissionKind;
        if (_environmentOptions.AllowedPermissions != null &&
            _environmentOptions.AllowedPermissions.Contains(kind))
        {
            e.State = CoreWebView2PermissionState.Allow;
        }
        else
        {
            e.State = CoreWebView2PermissionState.Deny;
        }
    }
}