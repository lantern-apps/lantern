using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lantern.AsService;

public partial class WebViewBrowser
{
    private static readonly JsonSerializerOptions DefaultSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
    };

    public Task<string> UrlAsync() => InvokeAsync(() => _webview.Source);

    public Task<string> TitleAsync() => InvokeAsync(() => _webview.DocumentTitle);

    public Task<string?> GetInnerTextAsync(string selector)
    {
        return EvaluateAsync<string?>($"document.querySelector(\"{selector}\")?.innerText");
    }

    public Task<string?> GetInnerTextAsync(string frameSelector, string selector)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{frameSelector}\")?.contentDocument?.querySelector(\"{selector}\")?.innerText"));
    }

    public Task<string?> GetInputValueAsync(string selector)
    {
        return EvaluateAsync<string?>($"document.querySelector(\"{selector}\")?.value");
    }
    public Task<string?> GetImageSrcAsync(string selector)
    {
        return EvaluateAsync<string?>($"document.querySelector(\"{selector}\")?.src");
    }

    public Task SetInputFileAsync(string selector, string filename)
    {
        var dom = Cdp.DOM;
        return InvokeAsync(async () =>
        {
            var doc = await dom.GetDocumentAsync(-1, true);
            var input = await dom.QuerySelectorAsync(doc.NodeId, selector);
            if (input == 0)
                throw new Exception($"未找到 {selector}");

            await dom.SetFileInputFilesAsync([filename], input);
        });
    }

    public Task EvaluateAsync(string script)
    {
        script = script.Trim();
        if (script.IsExpressionScript())
            script = '(' + script + ")()";

        script = script.Trim();
        return InvokeAsync(() => _webview.ExecuteScriptAsync(script));
    }

    public async Task<T> EvaluateAsync<T>(string script, JsonSerializerOptions? options = null)
    {
        options ??= DefaultSerializerOptions;

        script = script.Trim();
        if (script.IsExpressionScript())
            script = '(' + script + ")()";

        var json = await InvokeAsync(() => _webview.ExecuteScriptAsync(script));
        return JsonSerializer.Deserialize<T>(json, options)!;
    }

    public Task ClickAsync(string selector)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\")?.click()"));
    }

    public Task ClickParentAsync(string selector)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\")?.parentElement?.click()"));
    }


    public Task ClickAsync(string frameSelector, string selector)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{frameSelector}\")?.contentDocument?.querySelector(\"{selector}\")?.click()"));
    }

    public Task ScrollToButtomAsync(string selector)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\")?.scrollBy(0,9999)"));
    }

    public Task ScrollByAsync(string selector, int x, int y)
    {
        return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\")?.scrollBy({x},{y})"));
    }

    public Task FillAsync(string selector, string content)
    {
        string js = @$"(()=>{{ 
    const input = document.querySelector('{selector}')
    let lastValue = '';
    if(input._valueTracker) {{
        lastValue = input._valueTracker.getValue();
    }}
    input.value = '{content}';
    if(input._valueTracker) {{
        input._valueTracker.setValue(lastValue);
    }}
    input.dispatchEvent(new Event('input',{{bubbles:true,cancelable:true}}));
    input.dispatchEvent(new Event('change',{{bubbles:true,cancelable:true}}));
}})();";
        return InvokeAsync(() => _webview.ExecuteScriptAsync(js));
        //return InvokeAsync(() => _webview.ExecuteScriptAsync($"document.querySelector(\"{selector}\").value = '{content}'"));
    }

    public Task<bool> IsVisibleAsync(string selector)
    {
        return EvaluateAsync<bool>($"document.querySelector(\"{selector}\") != null");
    }

    public Task<bool> IsVisibleAsync(string frameSelector, string selector)
    {
        return EvaluateAsync<bool>($"document.querySelector(\"{frameSelector}\")?.contentDocument?.querySelector(\"{selector}\") != null");
    }

    public async Task<DomRect> GetBoundingClientRect(string selector)
    {
        string js = $@"() => {{
    const r = document.querySelector(""{selector}"")?.getBoundingClientRect();
    if(r){{
        return {{ x:r.x,y:r.y,width:r.width,height:r.height,top:r.top,bottom:r.bottom,left:r.left,right:r.right}};
    }}
    return null;
}}";
        var rect = await EvaluateAsync<DomRectDto>(js);
        if (rect == null)
            return default;

        return new DomRect
        {
            Bottom = rect.Bottom,
            Height = rect.Height,
            Left = rect.Left,
            Right = rect.Right,
            Top = rect.Top,
            Width = rect.Width,
            X = rect.X,
            Y = rect.Y,
        };
    }

    private sealed class DomRectDto
    {
        [JsonPropertyName("bottom")]
        public double Bottom { get; set; }

        [JsonPropertyName("top")]
        public double Top { get; set; }

        [JsonPropertyName("left")]
        public double Left { get; set; }

        [JsonPropertyName("right")]
        public double Right { get; set; }

        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }

        [JsonPropertyName("width")]
        public double Width { get; set; }

        [JsonPropertyName("height")]
        public double Height { get; set; }
    }
}

public readonly struct DomPoint
{
    public double X { get; init; }
    public double Y { get; init; }
}

public readonly struct DomRect
{
    public double Bottom { get; init; }
    public double Top { get; init; }
    public double Left { get; init; }
    public double Right { get; init; }
    public double X { get; init; }
    public double Y { get; init; }
    public double Width { get; init; }
    public double Height { get; init; }

    public bool IsEmpty => Top == 0 && Left == 0 && Width == 0 && Height == 0;

    public DomPoint GetCenter() => new()
    {
        X = X + Width / 2,
        Y = Y + Height / 2,
    };
}