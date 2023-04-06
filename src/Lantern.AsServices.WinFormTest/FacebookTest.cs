using System.Net;

namespace Lantern.AsService.WinFormTest;

internal class FacebookTest
{
    private readonly WebViewBrowser _page;

    public FacebookTest(WebViewBrowser page)
    {
        _page = page;
    }

    public async Task<FacebookLoginResult> LoginAsync(string username, string password, CancellationToken cancellationToken)
    {
        await _page.GotoAsync("https://www.facebook.com/login");

        if (cancellationToken.IsCancellationRequested)
            return new FacebookLoginResult(false);

        if (await _page.IsVisibleAsync("a[href='/me/'],div[role=\'button\'] mask[id]"))
            return new FacebookLoginResult(true);

        await _page.FillAsync("#email", username/*, new PageFillOptions { Force = true }*/);
        await _page.FillAsync("#pass", password/*, new PageFillOptions { Force = true }*/);
        await Task.Delay(100);

        if (cancellationToken.IsCancellationRequested)
            return new FacebookLoginResult(false);

        //他有ID，但每次刷新都会变
        //await page.RunAndWaitForResponseAsync(async () => await page.ClickAsync("button._42ft._4jy0._6lth._4jy6._4jy1.selected._51sy"), WaitForLogin).WithCancellation(cancellationToken);

        await _page.RunAndWaitForResponseAsync(
            () => _page.ClickAsync("#loginbutton"),
            e =>
            {
                if (e.Request.Method == "GET" && e.Request.Url == "https://www.facebook.com/?sk=welcome")
                    return true;

                if (e.Request.Method != "POST")
                    return false;

                if (e.Request.Url.StartsWith("https://www.facebook.com/login/?privacy_mutation_token=") ||
                    e.Request.Url.StartsWith("https://www.facebook.com/login/device-based/regular/login/?"))
                {
                    if (e.StatusCode != 302 || e.Headers == null)
                        return false;

                    if (!e.Headers.ContainsKey("location"))
                        return false;

                    var location = e.Headers["location"];
                    if (string.IsNullOrWhiteSpace(location))
                        return false;

                    if (location == "https://www.facebook.com/" ||
                        location == "https://www.facebook.com")
                        return true;
                }

                return false;
            });

        //await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        await Task.Delay(1000, cancellationToken);

        var url = await _page.UrlAsync();
        if (url.StartsWith("https://www.facebook.com/checkpoint", StringComparison.CurrentCultureIgnoreCase))
        {
            return new FacebookLoginResult(false, "");
        }

        string location;
        try
        {
            location = await _page.EvaluateAsync<string>("window.location.href");
        }
        catch (Exception ex)
        {
            return new FacebookLoginResult(false, "");
        }

        if (location.StartsWith("https://www.facebook.com/login", StringComparison.CurrentCultureIgnoreCase))
        {
            //这种情况是账号密码错
            var error = await _page.EvaluateAsync<string?>("document.querySelector('#error_box div:not([class])')?.innerText");
            if (string.IsNullOrEmpty(error))
            {
                //这种情况是被限制登录
                error = await _page.EvaluateAsync<string?>("document.querySelector('#content div.phl')?.innerText");
            }

            return new FacebookLoginResult(false, error);
        }
        else if (location.StartsWith("https://www.facebook.com/checkpoint", StringComparison.CurrentCultureIgnoreCase))
        {
            return new FacebookLoginResult(false, "");
        }
        else
        {
            return new FacebookLoginResult(true);
        }

    }

    public virtual async Task ExecuteAsync(string keyword, CancellationToken cancellationToken = default)
    {
        int count = 10;
        try
        {
            await _page.RunAndWaitForResponseAsync(
                async () => await _page.GotoAsync($"https://www.facebook.com/search/posts/?q={WebUtility.UrlEncode(keyword.Trim())}"),
                response => response.Request.Method == "POST" && response.Request.Url.StartsWith("https://www.facebook.com/ajax/bulk-route-definitions"));
        }
        catch (OperationCanceledException)
        {
            return;
        }

        //这里似乎还需要在等待下,因为请求过来了，但是有可能DOM还没加载，然没找到合适检查DOM的方式，就这样了，算是比较稳定
        //await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        //await _page.WaitForTimeoutAsync(100);

        if (cancellationToken.IsCancellationRequested)
            return;

        int pageIndex = 0;

        if (await FacebookSearchIsScrollToBottomAsync())
            return;

        int total = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                try
                {
                    await FacebookSearchResultWaitForScrollLoadAsync();
                }
                catch (TimeoutException)
                {
                    break;
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (cancellationToken.IsCancellationRequested)
                    break;

                var posts = await FacebookPostSearchResultQueryAsync();
                if (posts == null)
                    return;

                var results = posts.Skip(total).ToList();

                foreach (var post in results)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    post.Keyword = keyword;

                    try
                    {
                        //var data = await CreateDataAsync(post, cancellationToken);
                        //Context.Data(data);
                        //Context.LogInformation(SRLogMessages.Getting(post.Author));
                    }
                    catch (Exception)
                    {
                        //Context.LogDebug(ex, "处理数据时发生异常");
                    }

                    //if (_options.DataInterval > 0)
                    //{
                    //    try
                    //    {
                    //        await Task.Delay(_options.DataInterval, cancellationToken);
                    //    }
                    //    catch { break; }
                    //}

                }

                total += results.Count;

                if (++pageIndex >= count && count != 0)
                    break;

                if (await FacebookSearchIsScrollToBottomAsync())
                    break;

                await Task.Delay(1000, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;

                if (await FacebookSearchIsScrollToBottomAsync())
                    break;
            }
            catch (Exception)
            {
                throw;
                //throw new Exception(Resources.ListLoadingError, ex);
            }
        }
    }

    public Task<bool> FacebookSearchIsScrollToBottomAsync() => _page.EvaluateAsync<bool>(@"()=>{ 
if (document.querySelector('.sjgh65i0[role=\'article\'] div[role=\'progressbar\']')) return false;
if(!!document.querySelector('div.w0hvl6rk.qjjbsfad>span[dir=\'auto\']')) return true;
return !!document.querySelector('.l9j0dhe7.du4w35lb.rq0escxv.j83agx80.cbu4d94t.pfnyh3mw.d2edcug0.pybr56ya,.sjgh65i0[role=\'article\']');}");

    public async Task FacebookSearchResultWaitForScrollLoadAsync()
    {
        //https://www.facebook.com/ajax/bulk-route-definitions
        //https://www.facebook.com/api/graphql
        //await page.RunAndWaitForResponseAsync(
        //    async () => await page.EvaluateAsync("()=> window.scrollBy(0, 99999)"),
        //    response => response.Request.ResourceType == "xhr" && response.Request.Method == "POST" && response.Request.Url.StartsWith("https://www.facebook.com/ajax/bulk-route-definitions"));
        //await _page.ScrollToBottomAsync();
        await _page.EvaluateAsync("()=> window.scrollBy(0, 99999999)");

        var waitTask = _page.WaitForResponseAsync(response => 
            response.Request.Method == "POST" && 
            response.Request.Url.StartsWith("https://www.facebook.com/ajax/bulk-route-definitions"));

        if (await FacebookSearchIsScrollToBottomAsync())
        {
            _ = waitTask.ConfigureAwait(false);
            return;
        }

        await waitTask;
        //这里似乎还需要在等待下,因为请求过来了，但是有可能DOM还没加载，然没找到合适检查DOM的方式，就这样了，算是比较稳定
        //await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        //await _page.WaitForTimeoutAsync(200);
    }

    public Task<FacebookPostSearchRobotData[]> FacebookPostSearchResultQueryAsync() => _page.EvaluateAsync<FacebookPostSearchRobotData[]>(
@"Array.from(document.querySelectorAll('div[role=\'feed\'] div[role=\'article\']')).map(x=>{
	let author = x.querySelector('h3,h2')?.textContent
	let content = x.querySelector('div[id],a[href] span[dir=\'auto\']')?.textContent
    let dateTime = x.querySelector('span[id] a>span')?.textContent
    let link = x.querySelector('span[id] a[href],a[href]')?.href
	return {author:author,content:content,dateTime:dateTime,link:link}
}).filter(x=>x.content)");

}

public class FacebookPostSearchRobotData
{
    public string Keyword { get; set; }
    public string? Author { get; set; }
    public string? Content { get; set; }
    public string? DateTime { get; set; }
    public string? Link { get; set; }
}

public class FacebookLoginResult
{
    public FacebookLoginResult(bool success, string? error = null)
    {
        Success = success;
        Error = error;
    }

    public bool Success { get; set; }
    public bool NewSession { get; set; }
    public string? Error { get; set; }
}

