using Lantern.AsService;
using Lantern.Threading;
using Microsoft.Extensions.Options;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;

namespace AutomationServiceHost.Services;

public class WatchVideoOptions
{
    public TimeSpan Duration { get; set; }
    public CancellationToken CancellationToken { get; set; }
}

public class TiktokSession(WebViewBrowser browser, string username, string password)
{
    private readonly BingtopCaptchaRecognizer _captchaRecognizer = new();
    public string UserName { get; } = username;

    public Task GotoAuthorAsync(string author, CancellationToken cancellationToken = default)
    {
        if (!author.StartsWith('@'))
            author = '@' + author;

        return browser.GotoAsync($"https://www.tiktok.com/{author}", new LoadOptions
        {
            CancellationToken = cancellationToken,
        });
    }

    public async Task VideoLikeAsync(string author, string videoId, CancellationToken cancellationToken)
    {
        if (!author.StartsWith('@'))
            author = '@' + author;

        await browser.GotoAsync($"https://www.tiktok.com/{author}/video/{videoId}", new LoadOptions
        {
            CancellationToken = cancellationToken,
        });

        await browser.WaitForClickAsync("div[data-e2e=search-comment-container] > div > div > div:nth-child(2) button:nth-child(1)", new WaitForSelectorOptions
        {
            CancellationToken = cancellationToken
        });
    }

    public async Task VideoWatchAsync(string author, string videoId, WatchVideoOptions? options = null)
    {
        options ??= new();

        if (!author.StartsWith('@'))
            author = '@' + author;

        await browser.GotoAsync($"https://www.tiktok.com/{author}/video/{videoId}", new LoadOptions
        {
            CancellationToken = options.CancellationToken,
        });

        if (options.Duration > TimeSpan.Zero)
        {
            await Task.Delay(options.Duration);
        }
    }

    public async Task FollowAsync(string author, CancellationToken cancellationToken = default)
    {
        await GotoAuthorAsync(author, cancellationToken);

        await browser.WaitForClickAsync("#main-content-others_homepage > div > div > div > div > div > div > button", new WaitForSelectorOptions
        {
            CancellationToken = cancellationToken
        });
    }

    public async Task LoginAsync(CancellationToken cancellationToken = default)
    {
        await browser.GotoAsync("https://www.tiktok.com", new LoadOptions
        {
            CancellationToken = cancellationToken,
        });

        await Task.Delay(5000, cancellationToken);

        var logined = await browser.WaitForSelectorAsync(["#loginContainer", "#header-more-menu-icon"], new WaitForSelectorOptions
        {
            CancellationToken = cancellationToken,
            Timeout = TimeSpan.FromSeconds(10),
        });

        if (logined == 1)
        {
            return;
        }

        await browser.GotoAsync("https://www.tiktok.com/login", new LoadOptions
        {
            CancellationToken = cancellationToken,
        });

        await Task.Delay(2000, cancellationToken);

        //点击 使用邮件、手机、账号登录
        await browser.WaitForClickAsync("#loginContainer > div > div > div > div:nth-child(3) > div", new WaitForSelectorOptions
        {
            CancellationToken = cancellationToken,
        });

        await Task.Delay(1000, cancellationToken);

        //点击 使用邮件登录
        await browser.WaitForClickAsync("#loginContainer > div > form > div > a", new WaitForSelectorOptions
        {
            CancellationToken = cancellationToken,
        });

        await browser.WaitForSelectorAsync("#loginContainer input[name=username]");

        await browser.FillAsync("#loginContainer input[name=username]", UserName);
        await browser.FillAsync("#loginContainer input[type=password]", password);

        await Task.Delay(500, cancellationToken);

        await browser.ClickAsync("button[type=submit]");

        //等待验证码
        //await browser.WaitForSelectorAsync("#captcha_container");
        await browser.WaitForSelectorAsync("#secsdk-captcha-drag-wrapper");

        await PassCaptchaAsync(cancellationToken);
    }

    private async Task PassCaptchaAsync(CancellationToken cancellationToken)
    {
        if (!await browser.IsVisibleAsync("#secsdk-captcha-drag-wrapper"))
        {
            return;
        }

        await Task.Delay(2000, cancellationToken);

        while (true)
        {
            var inner = await browser.EvaluateAsync<string>("document.querySelector('img[data-testid=whirl-inner-img]').src");
            var outer = await browser.EvaluateAsync<string>("document.querySelector('img[data-testid=whirl-outer-img]').src");

            var result = await _captchaRecognizer.RecognizeAsync(inner, outer, cancellationToken);

            const int x = 560;
            const int y = 505;

            await MouseSimulater.SimulateDragAsync(browser, x, y, (int)(271 * (result.Angle / 180.0)), cancellationToken);

            await Task.Delay(2000, cancellationToken);

            if (!await browser.IsVisibleAsync("#secsdk-captcha-drag-wrapper"))
            {
                break;
            }
        }
    }

    public async Task PublishVideoAsync(string filename, CancellationToken cancellationToken = default)
    {
        await browser.GotoAsync("https://www.tiktok.com/creator-center/upload", new LoadOptions
        {
            CancellationToken = cancellationToken,
        });

        await browser.WaitForSelectorAsync("iframe", "input");

        var dom = browser.Cdp.DOM;

        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            var doc = await dom.GetDocumentAsync(-1, true);
            var frame = Find(doc, "IFRAME") ?? throw new Exception("未找到 IFRAME");
            var input = Find(frame.ContentDocument, "INPUT") ?? throw new Exception("未找到 INPUT");

            await dom.SetFileInputFilesAsync([filename], input.NodeId);
        });
    }

    static DOM.Node? Find(DOM.Node node, string nodeName)
    {
        if (node.NodeName == nodeName)
        {
            return node;
        }

        if (node.Children != null)
        {
            foreach (var child in node.Children)
            {
                var result = Find(child, nodeName);
                if (result != null)
                    return result;
            }
        }

        return null;
    }
}
