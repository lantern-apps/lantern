using Lantern.Platform;
using Lantern.Threading;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Drawing;

namespace Lantern.Windows;

[DebuggerDisplay("{Name}")]
public class WebViewWindow : Window, IWebViewWindow
{
    [ThreadStatic]
    private static CoreWebView2Environment? _global_environment;

    private CoreWebView2Environment? _environment;

    private readonly WebViewEnvironmentOptions _environmentOptions;
    private readonly WebViewWindowOptions _windowOptions;
    private readonly IWindowManager? _windowManager;
    private readonly TaskCompletionSource _cts = new();
    private readonly CancellationTokenSource _closedSource = new();

    private readonly string _name;

    protected CoreWebView2Controller? _controller;
    private CoreWebView2? _webview;

    public WebViewWindow(
        WebViewEnvironmentOptions environmentOptions,
        WebViewWindowOptions windowOptions,
        IWindowManager? windowManager,
        IWindowImpl windowImpl)
        : base(windowImpl)
    {
        _environmentOptions = environmentOptions;
        _windowOptions = windowOptions;
        _name = windowOptions.Name;
        _windowManager = windowManager;
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
    public event Action<string, string>? WebViewMessageReceived;

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

    public Task ClearCookieAsync()
    {
        if (_webview == null)
            return Task.CompletedTask;
        if (Dispatcher.UIThread.CheckAccess())
        {
            return _webview.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.Cookies);
        }
        else
        {
            return Dispatcher.UIThread.InvokeAsync(() => _webview.Profile.ClearBrowsingDataAsync(CoreWebView2BrowsingDataKinds.Cookies));
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
        if (_environmentOptions.IsIsolated)
        {
            string? arguments = _environmentOptions.AdditionalBrowserArguments;
            if (_windowOptions.ProxyServer != null)
            {
                if(arguments == null)
                {
                    arguments = $"--proxy-server=\"{_windowOptions.ProxyServer}\"";
                }
                else
                {
                    arguments += $" --proxy-server=\"{_windowOptions.ProxyServer}\"";
                }
            }

            _environment = await CoreWebView2Environment.CreateAsync(
                _environmentOptions.BrowserExecutableFolder,
                _environmentOptions.UserDataFolder,
                new CoreWebView2EnvironmentOptions
                {
                    //#if DEBUG
                    //                AdditionalBrowserArguments = "--disable-web-security --enable-features=\"msWebView2EnableDraggableRegions\" --remote-debugging-port=9222",
                    //#else
                    //                AdditionalBrowserArguments = "--disable-web-security --enable-features=\"msWebView2EnableDraggableRegions\"",
                    //#endif

                    //AdditionalBrowserArguments = "--disable-web-security --proxy-server=\"127.0.0.1:9728\"",
                    AdditionalBrowserArguments = arguments,
                    Language = _windowOptions.Language ?? _environmentOptions.Language,
                    AllowSingleSignOnUsingOSPrimaryAccount = false,
                    IsCustomCrashReportingEnabled = false,
                    ExclusiveUserDataFolderAccess = false,
                });
        }
        else
        {
            _global_environment ??= await CoreWebView2Environment.CreateAsync(
                _environmentOptions.BrowserExecutableFolder,
                _environmentOptions.UserDataFolder,
                new CoreWebView2EnvironmentOptions
                {
                    Language = _environmentOptions.Language,
                    AdditionalBrowserArguments = _environmentOptions.AdditionalBrowserArguments,
                    AllowSingleSignOnUsingOSPrimaryAccount = false,
                    IsCustomCrashReportingEnabled = false,
                    ExclusiveUserDataFolderAccess = false,
                });
            _environment = _global_environment;
        }

        var controllerOptions = _environment.CreateCoreWebView2ControllerOptions();
        controllerOptions.ProfileName = _windowOptions.ProfileName;
        controllerOptions.IsInPrivateModeEnabled = _windowOptions.IsInPrivateModeEnabled;
        controllerOptions.ScriptLocale = _windowOptions.Language;

         _controller = await _environment.CreateCoreWebView2ControllerAsync(WindowImpl.NativeHandle, controllerOptions);

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
        _webview.WebResourceRequested += OnWebResourceRequested;
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
        _webview.Settings.AreHostObjectsAllowed = false;

        if (_environmentOptions.VirtualHosts != null && _environmentOptions.VirtualHosts.Count > 0)
        {
            foreach (var map in _environmentOptions.VirtualHosts)
                _webview.SetVirtualHostNameToFolderMapping(map.HostName, map.FolderName, CoreWebView2HostResourceAccessKind.Allow);
        }

        if (_environmentOptions.JavaScriptsOnDocumentCreated != null)
        {
            foreach (var js in _environmentOptions.JavaScriptsOnDocumentCreated)
                await _webview.AddScriptToExecuteOnDocumentCreatedAsync(js);
        }

        if (_environmentOptions.Filters != null)
        {
            foreach (var filter in _environmentOptions.Filters)
            {
                _webview.AddWebResourceRequestedFilter(filter.UrlOrPredicate, (CoreWebView2WebResourceContext)filter.ResourceType);
            }
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

    private void OnWebResourceRequested(object? sender, CoreWebView2WebResourceRequestedEventArgs e)
    {
        if (_environmentOptions.Filters == null)
        {
            return;
        }

        foreach (var filter in _environmentOptions.Filters)
        {
            if (filter.CanHandle(e.Request.Uri, (WebViewResourceType)e.ResourceContext))
            {
                var deferral = e.GetDeferral();

                filter.HandleAsync(new WebViewRequestContext(_environment!, e)).ContinueWith(_ =>
                {
                    deferral.Complete();
                }).ConfigureAwait(false);

                break;
            }
        }
    }

    private void OnWebViewMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        WebViewMessageReceived?.Invoke(e.Source, e.WebMessageAsJson);
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