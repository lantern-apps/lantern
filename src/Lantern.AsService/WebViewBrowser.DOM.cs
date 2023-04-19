using System.Text.Json;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public Task<string> UrlAsync() => InvokeAsync(() => _webview.Source);

    public Task<string> TitleAsync() => InvokeAsync(() => _webview.DocumentTitle);

    public Task EvaluateAsync(string script)
    {
        script = script.Trim();
        if (script.IsExpressionScript())
            script = '(' + script + ")()";

        script = script.Trim();
        return InvokeAsync(() => _webview.ExecuteScriptAsync(script));
    }

    public Task<T> EvaluateAsync<T>(string script, JsonSerializerOptions? options = null)
    {
        options ??= DefaultSerializerOptions;

        script = script.Trim();
        if (script.IsExpressionScript())
            script = '(' + script + ")()";

        return InvokeAsync(async () =>
        {
            var json = await _webview.ExecuteScriptAsync(script);
            return JsonSerializer.Deserialize<T>(json, options)!;
        });
    }

    public Task ClickAsync(string selector)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\")?.click()"));
    }

    public Task FillAsync(string selector, string content)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\").value = '{content}'"));
    }

    public Task<bool> IsVisibleAsync(string selector)
    {
        return InvokeAsync(() => EvaluateAsync<bool>($"document.querySelectorAll(\"{selector}\").length > 0"));
    }
}