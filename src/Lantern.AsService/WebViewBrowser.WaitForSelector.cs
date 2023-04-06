using System.Threading;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public async Task WaitForSelectorAsync(string selector, WaitForSelectorOptions? options = null)
    {
        options ??= WaitForSelectorOptions.Default;

        if(options.Timeout > TimeSpan.Zero)
        {
            await WaitForSelectorAsync(selector, options.Timeout, options.CancellationToken);
        }
        else
        {
            await WaitForSelectorAsync(selector, options.CancellationToken);
        }
    }

    private async Task WaitForSelectorAsync(string selector, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var cts = new CancellationTokenSource(timeout);
        var timeoutToken = cts.Token;
        while (true)
        {
            var value = await InvokeAsync(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(cancellationToken);
                if (timeoutToken.IsCancellationRequested)
                    throw new TimeoutException();

                return _webview.ExecuteScriptAsync($"document.querySelector('{selector}') != null");
            });

            if (value == "true")
                break;

            if (cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException(cancellationToken);

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();

            await Task.Delay(100, cts.Token);

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();
        }
    }

    private async Task WaitForSelectorAsync(string selector, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var value = await InvokeAsync(() =>
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new OperationCanceledException(cancellationToken);

                return _webview.ExecuteScriptAsync($"document.querySelector('{selector}') != null");
            });
            if (value == "true")
                break;
            await Task.Delay(100, cancellationToken);
        }
    }
}

public class WaitForSelectorOptions
{
    internal static readonly WaitForSelectorOptions Default = new();
    public TimeSpan Timeout { get; set; }
    public CancellationToken CancellationToken { get; set; }
}
