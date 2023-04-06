using Lantern.Threading;
using Lantern.Windows;

namespace Lantern.AsService.WinFormTest;

public partial class Form1 : Form
{
    private readonly IWebBrowserManager _windowManager;
    private WebViewBrowser _browser = null!;

    public Form1(IWebBrowserManager windowManager)
    {
        _windowManager = windowManager;
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        CreateOrShow();
    }

    private void BtnCreateWindow_Click(object sender, EventArgs e)
    {
        CreateOrShow();
    }


    private async void BtnEvaluate_Click(object sender, EventArgs e)
    {
        await _browser.EvaluateAsync(TbScript.Text);
    }

    private async void BtnClick_Click(object sender, EventArgs e)
    {
        await _browser.ClickAsync(TbClickSelector.Text);
    }

    private async void BtnFill_Click(object sender, EventArgs e)
    {
        await _browser.FillAsync(TbFillSelector.Text, TbFillContent.Text);
    }

    private async void CreateOrShow()
    {
        if (_browser == null)
        {
            _browser = await _windowManager.CreateAsync(new WebViewWindowOptions
            {
                AreDevToolsEnabled = true,
                AreBrowserAcceleratorKeysEnabled = true,
                Url = "https://app.lantern/index.html"
            });
            _browser.Window.Closed += () => _browser = null!;
        }

        _browser.Window.Show();
        await _browser.Window.EnsureWebViewInitializedAsync();

        //await _window.AsService.RouteAsync("https://assets.msn.cn/**/*", e =>
        //{
        //    if (e.ResourceContext == Microsoft.Web.AsService.Core.CoreWebView2WebResourceContext.Image)
        //    {
        //        e.Response = _window.AsService.CoreWebView.Environment.CreateWebResourceResponse(null, 200, null, null);
        //        _window.AsService.UnrouteAsync("https://assets.msn.cn/**/*");
        //    }
        //});
        //await _window.AsService.RouteAsync("**", e =>
        //{
        //    if (e.ResourceContext == Microsoft.Web.AsService.Core.CoreWebView2WebResourceContext.Image)
        //    {
        //        e.Response = _window.AsService.CoreWebView.Environment.CreateWebResourceResponse(null, 200, null, null);
        //        _window.AsService.UnrouteAsync("**");
        //    }
        //});
    }

    private void BtnHidden_Click(object sender, EventArgs e)
    {
        _browser.Window.Hide();
    }

    private void BtnClose_Click(object sender, EventArgs e)
    {
        _browser.Window.Close();
        _browser = null!;
    }

    private async void BtnGoto_Click(object sender, EventArgs e)
    {
        Log($"Goto Start\t{TbUrl.Text}");
        await _browser.GotoAsync(TbUrl.Text, new AsService.LoadOptions
        {
            WaitUntil = AsService.WaitUntilState.DOMContentLoaded,
        });
        Log($"Goto Complete\t{TbUrl.Text}");
    }

    private async void BtnWaitForSelector_Click(object sender, EventArgs e)
    {
        Log($"WatForSelector Start\t{TbWatForSelector.Text}");
        await _browser.WaitForSelectorAsync(TbWatForSelector.Text);
        Log($"WatForSelector Complete\t{TbWatForSelector.Text}");
    }

    private async void BtnWaitForUrl_Click(object sender, EventArgs e)
    {
        Log($"WaitForUrl Start\t{TbWaitForUrl.Text}");
        await _browser.WaitForUrlAsync(TbWaitForUrl.Text);
        Log($"WaitForUrl Complete\t{TbWaitForUrl.Text}");
    }

    private async void BtnWaitForLoad_Click(object sender, EventArgs e)
    {
        Log($"WaitForLoad Start");
        await _browser.WaitForLoadStateAsync();
        Log($"WaitForLoad Complete");
    }

    private async void BtnFacebookLogin_Click(object sender, EventArgs e)
    {
        FacebookTest test = new(_browser);
        var result = await test.LoginAsync("xiao.xin@outlook.com", "adaxin()", default);
        if (result.Success)
        {
            await test.ExecuteAsync("led");
        }
    }

    private async void BtnGoogleSearch_Click(object sender, EventArgs e)
    {
        GoogleSearchTest test = new(_windowManager);
        await test.ExecuteAsync("led", default);

    }

    private void Log(string message)
    {
        if (InvokeRequired)
        {
            Invoke(() => LbConsoleOutput.Items.Insert(0, message));
        }
        else
        {
            LbConsoleOutput.Items.Insert(0, message);
        }

    }

    private async void BtnThrowException_Click(object sender, EventArgs e)
    {
        try
        {
            await Dispatcher.UIThread.InvokeAsync(() => throw new Exception("throw"));
        }
        catch
        {
            MessageBox.Show("catch exception");
        }
    }
}