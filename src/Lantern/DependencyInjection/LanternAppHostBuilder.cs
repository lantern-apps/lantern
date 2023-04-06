using Calls;
using Lantern.Aus;
using Lantern.Controllers;
using Lantern.Messaging;
using Lantern.Services;
using Lantern.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace Lantern;

public class LanternAppHostBuilder : ILanternHostBuilder
{
    private const string DefaultWindowName = "default";
    private const string DefaultTrayName = "default";
    private const string DefaultVirualHostFolderName = "approot";
    private const string DefaultVirualHostName = "app.lantern";
    private const string DefaultStartUrl = "https://app.lantern/index.html";
    private static readonly string DefaultConfigurationFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

    private readonly IServiceCollection _services;
    private readonly IpcOptions _ipcOptions = new();
    private readonly LanternOptions _lanternOptions;
    private readonly WebViewEnvironmentOptions _envOptions = new();
    private readonly List<Action<WebViewWindowOptions>> _windowActions = new();
    private readonly List<Action<TrayOptions>?> _trayActions = new();
    private readonly List<Action<IpcOptions>> _ipcActions = new();
    private readonly List<Action<WebViewEnvironmentOptions>> _envActions = new();
    private readonly List<Action<AppOptions>> _appActions = new();
    private readonly List<Action<UpdaterOptions>> _updaterActions = new();

    private LanternAppHostBuilder(AppOptions? options, IServiceCollection serviceCollection)
    {
        Configuration = new();

        if (File.Exists(DefaultConfigurationFilePath))
        {
            Configuration.AddJsonFile(DefaultConfigurationFilePath);
        }

        _lanternOptions = new(options);

        _services = serviceCollection;
        _services.AddLogging();
        _services.AddSingleton(_envOptions);
        _services.AddSingleton(_ipcOptions);
        _services.AddSingleton(_lanternOptions);
        _services.AddSingleton<ILanternHost, LanternApp>();
        _services.AddSingleton<ILanternApp>(services => (LanternApp)services.GetRequiredService<ILanternHost>());
        _services.AddSingleton<AppLifetime>();
        _services.AddSingleton<IAppLifetime>(services => services.GetRequiredService<AppLifetime>());
        _services.AddSingleton<IDialogProvider, DialogProvider>();
        _services.AddSingleton<IScreenProvider, ScreenProvider>();
        _services.AddSingleton<ITrayManager, TrayService>();

        _services.TryAddSingleton<ICall, IpcCall>();
        _services.AddSingleton<IEventEmitter, IpcEventEmitter>();

        _services.AddSingleton<WindowControllerBase, WindowController>();
        _services.AddSingleton<AppControllerBase, AppController>();
        _services.AddSingleton<ShellControllerBase, ShellController>();
        _services.AddSingleton<ClipboardControllerBase, ClipboardController>();
        _services.AddSingleton<DialogControllerBase, DialogController>();
        _services.AddSingleton<EventControllerBase, EventController>();
        _services.AddSingleton<NotificationControllerBase, NotificationController>();
        _services.AddSingleton<UpdaterControllerBase, UpdaterController>();
        _services.AddSingleton<ScreenControllerBase, ScreenController>();
        _services.AddSingleton<TrayControllerBase, TrayController>();

        AddBuiltInFeatures();

        RegisterBuiltInIpcRequest();

        LoadCoreJavascripts();
    }

    public ConfigurationManager Configuration { get; }

    public IServiceCollection Services => _services;

    private void AddBuiltInFeatures()
    {
        AddFeature<Ipc, IpcImpl>();
        AddFeature<ITrayManager, TrayService>();
        AddFeature<IWindowManager, WindowService>();
    }

    private void RegisterBuiltInIpcRequest()
    {
        _ipcOptions.Register(KnownMessageNames.AppGetVersion);
        _ipcOptions.Register(KnownMessageNames.AppShutdown);

        _ipcOptions.Register(KnownMessageNames.WindowCreate, typeof(WindowCreateOptions));
        _ipcOptions.Register(KnownMessageNames.WindowGetAll);
        _ipcOptions.Register(KnownMessageNames.WindowActivate);
        _ipcOptions.Register(KnownMessageNames.WindowCenter);
        _ipcOptions.Register(KnownMessageNames.WindowClose);
        _ipcOptions.Register(KnownMessageNames.WindowHide);
        _ipcOptions.Register(KnownMessageNames.WindowShow);

        _ipcOptions.Register(KnownMessageNames.WindowFullScreen);
        _ipcOptions.Register(KnownMessageNames.WindowMinimize);
        _ipcOptions.Register(KnownMessageNames.WindowMaximize);
        _ipcOptions.Register(KnownMessageNames.WindowRestore);

        _ipcOptions.Register(KnownMessageNames.WindowSetAlwaysOnTop, typeof(bool));
        _ipcOptions.Register(KnownMessageNames.WindowSetSize, typeof(LogisticSize));
        _ipcOptions.Register(KnownMessageNames.WindowSetMinSize, typeof(LogisticSize));
        _ipcOptions.Register(KnownMessageNames.WindowSetMaxSize, typeof(LogisticSize));
        _ipcOptions.Register(KnownMessageNames.WindowSetPosition, typeof(LogisticPosition));
        _ipcOptions.Register(KnownMessageNames.WindowSetTitle, typeof(string));
        _ipcOptions.Register(KnownMessageNames.WindowSetUrl, typeof(string));

        _ipcOptions.Register(KnownMessageNames.WindowGetSize);
        _ipcOptions.Register(KnownMessageNames.WindowGetPosition);
        _ipcOptions.Register(KnownMessageNames.WindowGetTitle);
        _ipcOptions.Register(KnownMessageNames.WindowGetMinSize);
        _ipcOptions.Register(KnownMessageNames.WindowGetMaxSize);
        _ipcOptions.Register(KnownMessageNames.WindowGetUrl);

        _ipcOptions.Register(KnownMessageNames.WindowStartDragMove);

        _ipcOptions.Register(KnownMessageNames.WindowIsFullScreen);
        _ipcOptions.Register(KnownMessageNames.WindowIsMinimized);
        _ipcOptions.Register(KnownMessageNames.WindowIsMaximized);
        _ipcOptions.Register(KnownMessageNames.WindowIsAlwaysOnTop);
        _ipcOptions.Register(KnownMessageNames.WindowIsVisible);
        _ipcOptions.Register(KnownMessageNames.WindowIsActive);

        _ipcOptions.Register(KnownMessageNames.WindowIsResizable);
        _ipcOptions.Register(KnownMessageNames.WindowSetResizable, typeof(bool));

        _ipcOptions.Register(KnownMessageNames.WindowIsSkipTaskbar);
        _ipcOptions.Register(KnownMessageNames.WindowSetSkipTaskbar, typeof(bool));

        _ipcOptions.Register(KnownMessageNames.WindowGetTitleBarStyle);
        _ipcOptions.Register(KnownMessageNames.WindowSetTitleBarStyle, typeof(WindowTitleBarStyle));

        _ipcOptions.Register(KnownMessageNames.UpdaterLaunch);
        _ipcOptions.Register(KnownMessageNames.UpdaterCheck);
        _ipcOptions.Register(KnownMessageNames.UpdaterPerform);

        _ipcOptions.Register(KnownMessageNames.EventListen, typeof(EventListenOptions));
        _ipcOptions.Register(KnownMessageNames.EventUnlisten, typeof(EventUnlistenOptions));

        _ipcOptions.Register(KnownMessageNames.ClipboardSetText, typeof(string));
        _ipcOptions.Register(KnownMessageNames.ClipboardGetText);

        _ipcOptions.Register(KnownMessageNames.DialogMessage, typeof(MessageDialogOptions));
        _ipcOptions.Register(KnownMessageNames.DialogAsk, typeof(MessageDialogOptions));
        _ipcOptions.Register(KnownMessageNames.DialogConfirm, typeof(MessageDialogOptions));

        _ipcOptions.Register(KnownMessageNames.DialogOpenFile, typeof(OpenFileDialogOptions));
        _ipcOptions.Register(KnownMessageNames.DialogOpenFolder, typeof(OpenFolderDialogOptions));
        _ipcOptions.Register(KnownMessageNames.DialogSaveFile, typeof(SaveFileDialogOptions));

        _ipcOptions.Register(KnownMessageNames.NotificationSend, typeof(SendNotificationOptions));
        _ipcOptions.Register(KnownMessageNames.ShellOpen, typeof(ShellOpenOptions));

        _ipcOptions.Register(KnownMessageNames.ScreenGetAll);
        _ipcOptions.Register(KnownMessageNames.ScreenGetCurrent);

        _ipcOptions.Register(KnownMessageNames.TraySetTooltip, typeof(string));
        _ipcOptions.Register(KnownMessageNames.TraySetMenu, typeof(MenuOptions));
        _ipcOptions.Register(KnownMessageNames.TrayShow);
        _ipcOptions.Register(KnownMessageNames.TrayHide);
        _ipcOptions.Register(KnownMessageNames.TrayIsVisible);
    }

    public LanternAppHostBuilder AddFeature<TService>()
        where TService : class, ILanternService
    {
        _services.AddSingleton<TService>();
        _services.AddSingleton<ILanternService, TService>(services => services.GetRequiredService<TService>());
        return this;
    }

    public LanternAppHostBuilder AddFeature<TService, TImplementation>()
        where TService : class
        where TImplementation : class, TService, ILanternService
    {
        _services.AddSingleton<TService, TImplementation>();
        _services.AddSingleton<ILanternService, TImplementation>(services => (TImplementation)services.GetRequiredService<TService>());
        return this;
    }

    private void LoadCoreJavascripts()
    {
        using var stream = typeof(LanternAppHostBuilder).Assembly.GetManifestResourceStream("Lantern.lantern.global.js");
        if (stream == null)
            return;

        using var reader = new StreamReader(stream);
        var js = reader.ReadToEnd();
        _envOptions.JavaScriptsOnDocumentCreated = new List<string> { js };
    }

    public LanternAppHostBuilder ConfigureWebView(Action<WebViewEnvironmentOptions> optionsAction)
    {
        if (optionsAction == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(optionsAction));
        }

        _envActions.Add(optionsAction);
        return this;
    }

    public LanternAppHostBuilder ConfigureLogging(Action<ILoggingBuilder> optionsAction)
    {
        _services.AddLogging(optionsAction);
        return this;
    }

    public LanternAppHostBuilder AddWindow(Action<WebViewWindowOptions> optionsAction)
    {
        if (optionsAction == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(optionsAction));
        }

        _windowActions.Add(optionsAction);
        return this;
    }

    public LanternAppHostBuilder AddTray(Action<TrayOptions>? optionsAction = null)
    {
        _trayActions.Add(optionsAction);
        return this;
    }

    public LanternAppHostBuilder ConfigureApp(Action<AppOptions> optionsAction)
    {
        if (optionsAction == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(optionsAction));
        }
        _appActions.Add(optionsAction);
        return this;
    }

    public LanternAppHostBuilder ConfigureUpdater(Action<UpdaterOptions> optionsAction)
    {
        if (optionsAction == null)
        {
            ThrowHelper.ThrowArgumentNullException(nameof(optionsAction));
        }
        _updaterActions.Add(optionsAction);
        return this;
    }

    public LanternAppHostBuilder ConfigureIpc(Action<IpcOptions>? optionsAction = null)
    {
        optionsAction?.Invoke(_ipcOptions);
        return this;
    }

    public LanternAppHostBuilder ConfigureAppConfiguration(Action<IConfigurationBuilder> builderAction)
    {
        builderAction.Invoke(Configuration);
        return this;
    }

    public ILanternHost Build()
    {
        if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            Services.AddLanternWin32Platform();

        Configuration.GetSection("lantern").Bind(_lanternOptions);

        BuildAppOptions();
        BuildWebViewEnvironment();
        BuildUpdaterOptions();
        BuildTrayOptions();
        BuildWindowOptions();

        foreach (var ipcAction in _ipcActions)
        {
            ipcAction(_ipcOptions);
        }

        if (_lanternOptions.Windows.Count == 0)
        {
            _lanternOptions.Windows.Add(new WebViewWindowOptions
            {
                Name = "default",
                Title = "Lantern",
                Url = DefaultStartUrl
            });
        }

        ((ServiceCollection)Services).MakeReadOnly();
        var services = ((ServiceCollection)Services).BuildServiceProvider();
        return services.GetRequiredService<ILanternHost>();
    }

    private void BuildUpdaterOptions()
    {
        if (_updaterActions.Count == 0)
            return;

        foreach (var updaterAction in _updaterActions)
        {
            updaterAction(_lanternOptions.Updater);
        }

        _lanternOptions.Updater.UpdaterName ??= _lanternOptions.AppName + " AutoUpdater";

        if (_lanternOptions.Updater.UpdateDataPath == null)
        {
            _lanternOptions.Updater.UpdateDataPath = Path.Combine(_lanternOptions.UserDataFolder, " UpdateData");
        }
        else
        {
            _lanternOptions.Updater.UpdateDataPath = ValidationHelper.ValidatePathQualified(_lanternOptions.Updater.UpdateDataPath);
        }

        _services.AddSingleton(new AusUpdateOptions
        {
            Exclusives = _lanternOptions.Updater.Exclusives,
            InitVersion = _lanternOptions.Version,
            PackageName = _lanternOptions.AppName,
            UpdaterName = _lanternOptions.Updater.UpdaterName,
            ServerAddress = _lanternOptions.Updater.ServerAddress,
            TempFilePath = _lanternOptions.Updater.UpdateDataPath,
        });
        _services.AddSingleton<ILanternService, UpdateService>();
        _services.AddScoped<IAusUpdateManager, AusUpdateManager>();
    }

    private void BuildWindowOptions()
    {
        if (_windowActions.Count == 0)
            return;

        List<string?> windowNames = new();
        foreach (var windowAction in _windowActions)
        {
            WebViewWindowOptions options = new();
            windowAction(options);

            if (windowNames.Contains(options.Name))
            {
                throw new ArgumentException($"Duplicate window name '{options.Name}'", "options.Name");
            }

            windowNames.Add(options.Name);

            if (options.Url != null && !Uri.IsWellFormedUriString(options.Url, UriKind.Absolute))
            {
                if (!Uri.IsWellFormedUriString(options.Url, UriKind.Relative))
                {
                    throw new ArgumentException("Invaid url.", "Url");
                }

                options.Url = ParseUrl(options.Url);
            }


            options.Title ??= _lanternOptions.AppName;
            options.IconPath ??= _lanternOptions.DefaultIconPath;

            _lanternOptions.Windows.Add(options);
        }

        var nullName = _lanternOptions.Windows.FirstOrDefault(x => x.Name is null);
        if (nullName != null)
        {
            nullName.Name = DefaultWindowName;
        }

        foreach(var options in _lanternOptions.Windows)
        {
            ValidationHelper.Validate(options);
        }

        string ParseUrl(string url)
        {
            if (url.StartsWith('/'))
                url = url.Substring(1);

            foreach (var host in _lanternOptions.WebViewEnvironment.VirtualHosts)
            {
                var path = Path.Combine(host.FolderName, url);

                if (Path.IsPathFullyQualified(path) && File.Exists(path))
                {
                    return "https://" + host.HostName + '/' + url;
                }
            }

            return "https://" + DefaultVirualHostName + '/' + url;
        }
    }

    private void BuildWebViewEnvironment()
    {
        foreach (var envAction in _envActions)
        {
            envAction(_envOptions);
        }

        bool hasDefault = false;
        foreach (var host in _envOptions.VirtualHosts)
        {
            ValidationHelper.ValidateDirectoryExists(host.FolderName);

            if (host.HostName == DefaultVirualHostName)
                hasDefault = true;
        }

        if (_envOptions.UserDataFolder == null)
        {
            _envOptions.UserDataFolder = _lanternOptions.UserDataFolder;
        }
        else
        {
            _envOptions.UserDataFolder = ValidationHelper.ValidatePathQualified(_envOptions.UserDataFolder);
        }

        if (!hasDefault && Directory.Exists(DefaultVirualHostFolderName))
        {
            _envOptions.AddVirtualHostMapping(DefaultVirualHostName, DefaultVirualHostFolderName);
        }
    }

    private void BuildTrayOptions()
    {

        TrayOptions options = new();

        foreach (var taryAction in _trayActions)
        {
            taryAction?.Invoke(options);
        }

        options.Name ??= DefaultTrayName;
        options.ToolTip ??= _lanternOptions.AppName;

        if (options.IconPath == null)
        {
            options.IconPath = _lanternOptions.DefaultIconPath;
        }
        else
        {
            options.IconPath = ValidationHelper.ValidateFileExists(options.IconPath);
        }

        if (_trayActions.Count == 0)
        {
            options.Visible = false;
        }

        _lanternOptions.Trays.Add(options);
    }

    private void BuildAppOptions()
    {
        foreach (var appAction in _appActions)
        {
            appAction(_lanternOptions);
        }

        if (string.IsNullOrEmpty(_lanternOptions.AppName))
        {
            _lanternOptions.AppName = "Lantern";
        }

        if (_lanternOptions.DefaultIconPath != null)
        {
            _lanternOptions.DefaultIconPath = ValidationHelper.ValidateFileExists(_lanternOptions.DefaultIconPath);
        }

        if (_lanternOptions.UserDataFolder == null)
        {
            _lanternOptions.UserDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), _lanternOptions.AppName);
        }
        else
        {
            _lanternOptions.UserDataFolder = ValidationHelper.ValidatePathQualified(_lanternOptions.UserDataFolder);
        }

    }

    public static LanternAppHostBuilder Create() => Create(new ServiceCollection());
    public static LanternAppHostBuilder Create(IServiceCollection services) => new(null, services);
}

