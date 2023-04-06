using Lantern.AsService;
using Lantern.Windows;
using System.Net;

namespace Lantern.AsService.WinFormTest;

internal class GoogleSearchTest
{
    private readonly IWebBrowserManager _windowManager;
    private WebViewBrowser _browser;

    public GoogleSearchTest(IWebBrowserManager windowManager)
    {
        _windowManager = windowManager;
    }

    public virtual async Task ExecuteAsync(string keyword, CancellationToken cancellationToken)
    {
        _browser = await _windowManager.CreateAsync(new WebViewWindowOptions
        {
            WindowTitleHandling = WebViewWindowTitleHandling.AlwaysUseUserSetting,
            NewWindowHandling = WebViewNewWindowHandling.NavigationCurrentWebView,
            Visible = true,
        });

        int pageIndex = 0;

        while (!cancellationToken.IsCancellationRequested)
        {
            if (_browser == null)
                break;

            if (!await WaitForCaptchaAsync(keyword, pageIndex, cancellationToken))
                break;

            if (cancellationToken.IsCancellationRequested)
                break;

            try
            {
                await _browser.WaitForLoadStateAsync(new LoadOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded,
                    //Timeout = _options.SearchTimeout
                });
            }
            catch (TimeoutException)
            {
                //_logger.LogDebug($"操作超时 WaitForLoadStateAsync");
            }

            //if (cancellationToken.IsCancellationRequested)
            //    break;

            //await page.WaitForTimeoutAsync(1000);

            var results = await _browser.GoogleSearchResultQueryAsync();

            if (results != null)
            {
                foreach (var item in results)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    item.Keyword = keyword;

                    //try
                    //{
                    //    var data = await CreateDataAsync(item, cancellationToken);
                    //    if (data != null)
                    //    {
                    //        Context.Data(data);
                    //        Context.LogInformation(SRLogMessages.Getting(item.Title));
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    Context.LogDebug(ex, "处理数据时发生异常");
                    //}

                    //if (!interval.HasValue || interval <= 0)
                    //    interval = _options.DataInterval;

                    //if (interval > 0)
                    //{
                    //    try
                    //    {
                    //        await Task.Delay(interval.Value, cancellationToken);
                    //    }
                    //    catch { break; }
                    //}
                }
            }

            if (cancellationToken.IsCancellationRequested)
                break;

            ++pageIndex;

            if (!await _browser.GoogleSearchHasNextPageAsync())
                break;

            if (cancellationToken.IsCancellationRequested)
                break;

            await Task.Delay(1000, cancellationToken);
        }
    }

    private async Task<bool> WaitForCaptchaAsync(string keyword, int pageIndex, CancellationToken cancellationToken)
    {
        if (_browser == null)
            return true;

        var captchaRequired = false;

        var response = await _browser.GoogleGotoSearchAsync(keyword, pageIndex);

        if (response != null && response.HttpStatusCode == 429)
            captchaRequired = true;

        if (!captchaRequired)
            return true;

        while (captchaRequired)
        {
            if (cancellationToken.IsCancellationRequested)
                return false;

            //如果浏览器隐藏了，则显示出来
            _browser.Window.Activate();

            try
            {
                ////_logger.LogWarning($"检查到人机验证，等待人机验证完成", Context?.ExectionId);
                //Context.LogWarning(SRLogMessages.ReCaptchaWaiting);

                ////_logger.LogWarning($"如果没有自动弹出人机验证窗口，请查看您的操作系统任务栏窗口", Context?.ExectionId);
                //Context.LogWarning(SRLogMessages.ReCaptchaWarning);

                await _browser.GoogleSearchWaitForCaptchaAsync(1000 * 60 * 5, cancellationToken);

                //Context.LogInformation(SRLogMessages.ReCaptchaCompleted);
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (TimeoutException)
            {
                //Context.LogWarning(SRLogMessages.ReCaptchaTimeout);
                return false;
            }
            finally
            {
                //如果浏览器没有隐藏，则隐藏浏览器
                //_window.Hide();
            }

            try
            {
                response = await _browser.WaitForUrlAsync(url =>
                {
                    var path = url.GetAbsolutePath();
                    return path == "/search" || path == "/sorry/index";
                }, new LoadOptions
                {
                    WaitUntil = WaitUntilState.DOMContentLoaded,
                    CancellationToken = cancellationToken,
                    Timeout = TimeSpan.FromMinutes(5)
                });

                if (response != null && response.Url.GetAbsolutePath() == "/sorry/index")
                    captchaRequired = true;
                else
                    captchaRequired = false;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (TimeoutException)
            {
                //_logger.LogDebug($"操作超时 PageRunAndWaitForNavigationOptions");
                return false;
            }
        }

        return true;
    }

}

internal static class PlaywrightPageGoogleExtensions
{
    public static async Task GoogleSearchWaitForCaptchaAsync(this WebViewBrowser page, float? timeout = 1000 * 60 * 5, CancellationToken cancellationToken = default)
    {
        var uri = new Uri(await page.UrlAsync());
        var callback = System.Web.HttpUtility.ParseQueryString(uri.Query).Get("continue");
        if (callback == null)
            callback = "https://www.google.com/search";

        var path = callback.GetAbsolutePath();

        await page.WaitForRequestAsync(request =>
        {
            if (cancellationToken.IsCancellationRequested)
                return true;

            return new Uri(request.Url).AbsolutePath == path;// && request.ResourceType == "document";
        },
        new WaitForRequestOptions
        {
            CancellationToken = cancellationToken
            //Timeout = timeout,
        });
    }

    /// <summary>
    /// 检查当前页是否有下一页
    /// </summary>
    public static Task<bool> GoogleSearchHasNextPageAsync(this WebViewBrowser page) => page.IsVisibleAsync("#pnnext");

    /// <summary>
    /// 检查当前页面是否需要人机验证
    /// </summary>
    public static Task<bool> GoogleCaptchaRequiredAsync(this WebViewBrowser page) => page.IsVisibleAsync("#captcha-form,hr[noshade]");

    /// <summary>
    /// 将当前页面跳转到谷歌并进行搜索
    /// </summary>
    /// <param name="searchText">搜索关键词</param>
    /// <param name="pageIndex">页码（从0开始）</param>
    /// <returns></returns>
    public static Task<LoadResponse?> GoogleGotoSearchAsync(this WebViewBrowser page, string searchText, int pageIndex = 0, bool filter = true, int timeout = 30000)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            throw new Exception();

        var url = $"https://www.google.com/search?q={WebUtility.UrlEncode(searchText.Trim())}&start={pageIndex * 10}";
        if (!filter)
            url += "&filter=0";

        return page.GotoAsync(url, new LoadOptions
        {
            WaitUntil = WaitUntilState.Completed,
            //Timeout = timeout,
        });
    }

    /// <summary>
    /// 查询当前页的谷歌搜索结果（前置要求是 需要page goto 到响应搜索页面)
    /// </summary>
    public static async Task<GoogleSearchData[]?> GoogleSearchResultQueryAsync(this WebViewBrowser page)
    {
        return await page.EvaluateAsync<GoogleSearchData[]>(@"Array.from(document.querySelectorAll('#rso div.g'))
.map(result => {
	const header = result.querySelector('a[data-jsarwt=\'1\'],div.yuRUbf a,div.dXiKIc a,div.ct3b9e a')
    if(!header) return null;
    const title = header.querySelector('h3').innerText
    const link = header.href
    const tags = Array.from(result.querySelectorAll('div[data-content-feature=\'1\'] div.MUxGbd.wuQ4Ob.WZ8Tjf span:not([aria-hidden])')).map(x=>x.innerText)
	const text = result.querySelector('div[data-content-feature=\'1\'] div.VwiC3b,div.Uroaid')?.innerText
	return { Title: title, Link: link, Summary: text, Tags:tags }
}).filter(x=>x != null)");
    }
}
public class GoogleSearchData
{
    public string? Title { get; set; }

    public string? Summary { get; set; }

    public string? Link { get; set; }

    public string[]? Tags { get; set; }

    public string? Keyword { get; set; }
}

public static class StringUrlExtensions
{
    public static string? GetQueryString(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        int num = url.IndexOf('?');
        if (num >= 0)
        {
            return url.Substring(num);
        }

        return null;
    }

    public static string? GetAbsolutePath(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            return null;
        }

        int num = url.IndexOf("://");
        if (num >= 0)
        {
            if (url.Length <= num + 3)
            {
                return string.Empty;
            }

            num = url.IndexOf('/', num + 3);
            if (num < 0)
            {
                return string.Empty;
            }
        }
        else
        {
            num = 0;
        }

        int num2 = url.IndexOf('?');
        string text = ((num2 < 0) ? url.Substring(num) : url.Substring(num, num2 - num));
        if (text.EndsWith("/"))
        {
            return text.Substring(0, text.Length - 1);
        }

        if (!text.StartsWith("/"))
        {
            text = "/" + text;
        }

        return text;
    }
}
