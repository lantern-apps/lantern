namespace Lantern.AsService;

public static class WebViewBrowserExtensions
{
    public static async Task WaitClickAsync(this WebViewBrowser browser, string selector, WaitForSelectorOptions? options = null)
    {
        options ??= new();
        await browser.WaitForSelectorAsync(selector, options);
        await browser.ClickAsync(selector);
    }

    public static async Task WaitClickThenWaitDetachAsync(this WebViewBrowser browser, string selector, WaitForSelectorOptions? options = null)
    {
        options ??= new();

        await browser.WaitForSelectorAsync(selector, options with
        {
            Detach = false,
        });
        await browser.ClickAsync(selector);
        await browser.WaitForSelectorAsync(selector, options with
        {
            Detach = true,
        });
    }
}