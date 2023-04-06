namespace Lantern.AsService;

public enum WebViewResourceType
{
    //
    // 摘要:
    //     Specifies all resources.
    All,
    //
    // 摘要:
    //     Specifies a document resources.
    Document,
    //
    // 摘要:
    //     Specifies a CSS resources.
    Stylesheet,
    //
    // 摘要:
    //     Specifies an image resources.
    Image,
    //
    // 摘要:
    //     Specifies another media resource such as a video.
    Media,
    //
    // 摘要:
    //     Specifies a font resource.
    Font,
    //
    // 摘要:
    //     Specifies a script resource.
    Script,
    //
    // 摘要:
    //     Specifies an XML HTTP request.
    XmlHttpRequest,
    //
    // 摘要:
    //     Specifies a Fetch API communication.
    Fetch,
    //
    // 摘要:
    //     Specifies a TextTrack resource.
    TextTrack,
    //
    // 摘要:
    //     Specifies an EventSource API communication.
    EventSource,
    //
    // 摘要:
    //     Specifies a WebSocket API communication.
    Websocket,
    //
    // 摘要:
    //     Specifies a Web App Manifest.
    Manifest,
    //
    // 摘要:
    //     Specifies a Signed HTTP Exchange.
    SignedExchange,
    //
    // 摘要:
    //     Specifies a Ping request.
    Ping,
    //
    // 摘要:
    //     Specifies a CSP Violation Report.
    CspViolationReport,
    //
    // 摘要:
    //     Specifies an other resource.
    Other
}
