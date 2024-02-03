namespace Lantern.AsService;

public partial class WebViewBrowser
{
    public Task<int> WaitForSelectorAsync(string[] selectors, WaitForSelectorOptions? options = null)
    {
        options ??= WaitForSelectorOptions.Default;

        if (options.Timeout > TimeSpan.Zero)
        {
            return WaitForSelectorAsync(selectors, options.Timeout, options.CancellationToken);
        }
        else
        {
            return WaitForSelectorAsync(selectors, options.CancellationToken);
        }
    }

    public Task WaitForSelectorAsync(string selector, WaitForSelectorOptions? options = null)
    {
        options ??= WaitForSelectorOptions.Default;

        if (options.Timeout > TimeSpan.Zero)
        {
            return WaitForSelectorAsync(selector, options.Detach, options.Timeout, options.CancellationToken);
        }
        else
        {
            return WaitForSelectorAsync(selector, options.Detach, options.CancellationToken);
        }
    }

    public async Task WaitForSelectorAsync(string frameSelector, string selector, WaitForSelectorOptions? options = null)
    {
        options ??= WaitForSelectorOptions.Default;

        if (options.Timeout > TimeSpan.Zero)
        {
            await WaitForSelectorAsync(frameSelector, selector, options.Timeout, options.CancellationToken);
        }
        else
        {
            await WaitForSelectorAsync(frameSelector, selector, options.CancellationToken);
        }
    }

    private async Task WaitForSelectorAsync(string selector, bool detach, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var cts = new CancellationTokenSource(timeout);
        var timeoutToken = cts.Token;
        while (true)
        {
            var value = await InvokeAsync(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (timeoutToken.IsCancellationRequested)
                    throw new TimeoutException();

                if (detach)
                {
                    return _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\") == null");
                }
                else
                {
                    return _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\") != null");
                }
            });

            if (value == "true")
                break;
            cancellationToken.ThrowIfCancellationRequested();

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();

            try
            {
                await Task.Delay(100, timeoutToken);
            }
            catch
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new TimeoutException();
            }

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();
        }
    }

    private async Task<int> WaitForSelectorAsync(string[] selectors, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var value = await InvokeAsync(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                for (int i = 0; i < selectors.Length; i++)
                {
                    var value = await _webview.ExecuteScriptAsync($"document.querySelector(\"{selectors[i]}\") != null");
                    if (value == "true")
                        return i;
                }

                return -1;
            });

            if (value >= 0)
            {
                return value;
            }
            await Task.Delay(100, cancellationToken);
        }

        return -1;
    }

    private async Task<int> WaitForSelectorAsync(string[] selectors, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var cts = new CancellationTokenSource(timeout);
        var timeoutToken = cts.Token;
        while (true)
        {
            var value = await InvokeAsync(async () =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (timeoutToken.IsCancellationRequested)
                    throw new TimeoutException();

                for (int i = 0; i < selectors.Length; i++)
                {
                    var value = await _webview.ExecuteScriptAsync($"document.querySelector(\"{selectors[i]}\") != null");
                    if (value == "true")
                        return i;
                }

                return -1;
            });

            if (value >= 0)
            {
                return value;
            }
            cancellationToken.ThrowIfCancellationRequested();

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();

            try
            {
                await Task.Delay(100, timeoutToken);
            }
            catch
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new TimeoutException();
            }

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();
        }
    }

    private async Task WaitForSelectorAsync(string selector, bool detach, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var value = await InvokeAsync(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (detach)
                {
                    return _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\") == null");
                }
                else
                {
                    return _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\") != null");
                }
            });
            if (value == "true")
                break;
            await Task.Delay(100, cancellationToken);
        }
    }

    private async Task WaitForSelectorAsync(string frameSelector, string selector, TimeSpan timeout, CancellationToken cancellationToken)
    {
        using var cts = new CancellationTokenSource(timeout);
        var timeoutToken = cts.Token;
        while (true)
        {
            var value = await InvokeAsync(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (timeoutToken.IsCancellationRequested)
                    throw new TimeoutException();

                return _webview.ExecuteScriptAsync($"document.querySelector(\"{frameSelector}\")?.contentWindow?.document?.querySelector(\"{selector}\") != null");
            });

            if (value == "true")
                break;
            cancellationToken.ThrowIfCancellationRequested();

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();

            try
            {
                await Task.Delay(100, timeoutToken);
            }
            catch
            {
                cancellationToken.ThrowIfCancellationRequested();
                throw new TimeoutException();
            }

            if (timeoutToken.IsCancellationRequested)
                throw new TimeoutException();
        }
    }



    private async Task WaitForSelectorAsync(string frameSelector, string selector, CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var value = await InvokeAsync(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();

                return _webview.ExecuteScriptAsync($"document.querySelector(\"{frameSelector}\")?.contentWindow?.document?.querySelector(\"{selector}\") != null");
            });
            if (value == "true")
                break;
            await Task.Delay(100, cancellationToken);
        }
    }

}

public record WaitForSelectorOptions
{
    internal static readonly WaitForSelectorOptions Default = new();

    public bool Detach { get; set; }
    public TimeSpan Timeout { get; set; }
    public CancellationToken CancellationToken { get; set; }
}
