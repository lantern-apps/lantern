using Lantern.AsService;

namespace AutomationServiceHost.Services;

public class TiktokSessionManager(IWebBrowserManager browserManager)
{
    private readonly List<TiktokSession> _sessions = [];


    public async Task<TiktokSession> CreateAsync(string username, string password)
    {
        var browser = await browserManager.CreateAsync(new Lantern.Windows.WebViewWindowOptions
        {
            Name = username,
            ProfileName = username,
            Title = username,
            Width = 1440,
            Url = "https://www.tiktok.com/",
            WindowTitleHandling = Lantern.Windows.WebViewWindowTitleHandling.AlwaysUseUserSetting,
            NewWindowHandling = Lantern.Windows.WebViewNewWindowHandling.WebViewDefault,
            AreDevToolsEnabled = true,
            AreBrowserAcceleratorKeysEnabled = true,
            Visible = true,
        });

        var session = new TiktokSession(browser, username, password);
        browser.Window.Closed += () => _sessions.Remove(session);
        _sessions.Add(session);

        return session;
    }

    public Task BlukAsync(Func<TiktokSession, CancellationToken, Task> op, CancellationToken cancellationToken = default)
    {
        Task[] tasks = new Task[_sessions.Count];

        for (int i = 0; i < _sessions.Count; i++)
        {
            TiktokSession session = _sessions[i];
            tasks[i] = op(session, cancellationToken);
        }

        return Task.WhenAll(tasks);
    }
}
