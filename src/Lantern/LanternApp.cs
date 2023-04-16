using Lantern.Platform;
using Lantern.Resources;
using Lantern.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Web.WebView2.Core;
using System.Diagnostics;

namespace Lantern;

public partial class LanternApp : ILanternHost, ILanternApp
{
    private readonly LanternOptions _options;

    private readonly IPlatformThreadingInterface _threadingPlatform;
    private readonly ISingleProcessInstanceManager _singleInstance;
    private readonly IDialogPlatform _dialogPlatform;
    private readonly IServiceProvider _services;
    private readonly ILogger _logger;
    private readonly AppLifetime _lifetime;
    private readonly IEnumerable<ILanternService> _features;

    private Thread? _thread;

    public LanternApp(
        LanternOptions options,
        AppLifetime lifetime,
        IEnumerable<ILanternService> features,
        ISingleProcessInstanceManager singleInstance,
        IDialogPlatform dialogPlatform,
        IPlatformThreadingInterface threading,
        ILogger<LanternApp> logger,
        IServiceProvider services)
    {
        _options = options;
        _lifetime = lifetime;
        _features = features;
        _singleInstance = singleInstance;
        _threadingPlatform = threading;
        _dialogPlatform = dialogPlatform;
        _services = services;
        _logger = logger;
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        if (e.ExceptionObject is Exception exception)
        {
            if (e.IsTerminating)
                _logger.LogCritical(exception, "Unhandled exception.");
            else
                _logger.LogError(exception, "Unhandled exception.");
        }
        else
        {
            if (e.IsTerminating)
                _logger.LogCritical($"Unhandled exception. {e.ExceptionObject}");
            else
                _logger.LogError($"Unhandled exception. {e.ExceptionObject}");
        }
    }

    public IServiceProvider Services => _services;

    public void Stop() => _lifetime.StopApplication();

    public void Run()
    {
        if (!PrepareToRun())
        {
            return;
        }

        RunThread(true);
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (!PrepareToRun())
            return;

        cancellationToken.ThrowIfCancellationRequested();
        cancellationToken.Register(Stop);

        var completion = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        _lifetime.ApplicationStopped.Register(() => completion.SetResult());

        RunThread(false);
        await completion.Task.ConfigureAwait(false);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!PrepareToRun())
            return;

        cancellationToken.ThrowIfCancellationRequested();
        var completion = new TaskCompletionSource();
        _lifetime.ApplicationStarted.Register(() => completion.SetResult());
        RunThread(false);
        await completion.Task.ConfigureAwait(false);
    }

    private bool PrepareToRun()
    {
        if (!_options.IsSingleInstance)
            return true;

        if (!_singleInstance.Lock())
        {
            _singleInstance.ActivateOtherProcessMainWindow();
            return false;
        }

        return CheckWebView2Environment();
    }

    private bool CheckWebView2Environment()
    {
        const string WebView2DownloadLink = "https://go.microsoft.com/fwlink/p/?LinkId=2124703";
        string? version = null;
        try
        {
            version = CoreWebView2Environment.GetAvailableBrowserVersionString(_options.WebViewEnvironment.BrowserExecutableFolder);
        }
        catch (WebView2RuntimeNotFoundException) { }

        if (!string.IsNullOrWhiteSpace(version))
            return true;

        if (_options.WebViewEnvironment.UninstallHanding == WebView2EnvironmentUninstallHanding.Exit)
        {
            _dialogPlatform.Alert(null, _options.AppName, SR.MissWebView2RuntimeManualInstallAlert);
            return false;
        }

        if (!_dialogPlatform.Confirm(null, _options.AppName, SR.MissWebView2RuntimeAutoInstallAlert))
        {
            return false;
        }

        var dir = Path.Combine(_options.UserDataFolder, "Setup");
        var path = Path.Combine(dir, "WebView2.exe");

        if (File.Exists(path))
            File.Delete(path);

        try
        {
            Directory.CreateDirectory(dir);

            using HttpClient httpClient = new();
            using var stream = httpClient.GetStreamAsync(WebView2DownloadLink).GetAwaiter().GetResult();
            using var fs = File.OpenWrite(path);
            stream.CopyTo(fs);
        }
        catch
        {
            if (_dialogPlatform.Confirm(null, _options.AppName, SR.MissWebView2RuntimeManualInstallAlert))
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = WebView2DownloadLink,
                    UseShellExecute = true
                });
            }
            return false;
        }

        Process.Start(new ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        })!.WaitForExit();

        try
        {
            version = CoreWebView2Environment.GetAvailableBrowserVersionString(_options.WebViewEnvironment.BrowserExecutableFolder);
        }
        catch (WebView2RuntimeNotFoundException) { }

        if (string.IsNullOrWhiteSpace(version))
        {
            _dialogPlatform.Alert(null, _options.AppName, SR.MissWebView2RuntimeManualInstallAlert);
            return false;
        }
        return true;
    }

    private void RunThread(bool waitForShudown)
    {
        if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
        {
            RunCore();
        }
        else
        {
            _thread = new(RunCore)
            {
                IsBackground = false
            };

            if (OperatingSystem.IsWindows())
            {
                _thread.SetApartmentState(ApartmentState.STA);
            }

            _thread.Start();

            if (waitForShudown)
            {
                _thread.Join();
            }
        }
    }

    private void RunCore()
    {
        Dispatcher.InitializeUIThread(_threadingPlatform);

        InitializeUI();

        _lifetime.NotifyStarted();

        Dispatcher.UIThread.MainLoop(_lifetime.ApplicationStopping);

        _lifetime.NotifyStopped();
    }

    private void InitializeUI()
    {
        foreach (var feature in _features)
        {
            feature.Initialize();
        }
    }

    public void Dispose()
    {
        _lifetime.StopApplication();

        try
        {
            _thread?.Join(1000);
            _thread = null;
        }
        catch (ThreadStateException) { }
    }
}
