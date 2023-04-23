using System.Net.Http.Headers;

namespace Lantern;

public class HttpGetProxyRequestFilter : IWebViewRequestFilter
{
    public HttpGetProxyRequestFilter(string urlOrPredicate, WebViewResourceType resourceType)
    {
        UrlOrPredicate = urlOrPredicate;
        ResourceType = resourceType;
    }

    public string UrlOrPredicate { get; }
    public WebViewResourceType ResourceType { get; }

    public async Task HandleAsync(WebViewRequestContext context)
    {
        using HttpClient httpClient = new(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (_, _, _, _) => true
        });

        try
        {
            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(context.Uri),
            };

            FillHeaders(request.Headers, context.Headers);
            var response = await httpClient.SendAsync(request);
            var headers = GetHeaders(response.Headers);

            context.Response((int)response.StatusCode, headers, response.Content.ReadAsStream());
        }
        catch
        {

        }
    }

    private static void FillHeaders(HttpRequestHeaders requestHeaders, IEnumerable<KeyValuePair<string, string>> headers)
    {
        foreach (var header in headers)
        {
            requestHeaders.Add(header.Key, header.Value);
        }
    }

    private static List<KeyValuePair<string, string>> GetHeaders(HttpResponseHeaders headers)
    {
        List<KeyValuePair<string, string>> values = new();
        foreach (var header in headers)
        {
            //if (header.Key == "cross-origin-resource-policy")
            //{
            //    continue;
            //}

            foreach (var value in header.Value)
            {

                values.Add(new KeyValuePair<string, string>(header.Key, value));
            }
        }
        return values;
    }
}
