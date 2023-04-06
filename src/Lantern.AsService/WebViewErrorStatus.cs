namespace Lantern.AsService;

public enum WebViewErrorStatus
{
    //
    // 摘要:
    //     Indicates that an unknown error occurred.
    Unknown,
    //
    // 摘要:
    //     Indicates that the SSL certificate common name does not match the web address.
    CertificateCommonNameIsIncorrect,
    //
    // 摘要:
    //     Indicates that the SSL certificate has expired.
    CertificateExpired,
    //
    // 摘要:
    //     Indicates that the SSL client certificate contains errors.
    ClientCertificateContainsErrors,
    //
    // 摘要:
    //     Indicates that the SSL certificate has been revoked.
    CertificateRevoked,
    //
    // 摘要:
    //     Indicates that the SSL certificate is not valid. The certificate may not match
    //     the public key pins for the host name, the certificate is signed by an untrusted
    //     authority or using a weak sign algorithm, the certificate claimed DNS names violate
    //     name constraints, the certificate contains a weak key, the validity period of
    //     the certificate is too long, lack of revocation information or revocation mechanism,
    //     non-unique host name, lack of certificate transparency information, or the certificate
    //     is chained to a [legacy Symantec root](https://security.googleblog.com/2018/03/distrust-of-symantec-pki-immediate.html).
    CertificateIsInvalid,
    //
    // 摘要:
    //     Indicates that the host is unreachable.
    ServerUnreachable,
    //
    // 摘要:
    //     Indicates that the connection has timed out.
    Timeout,
    //
    // 摘要:
    //     Indicates that the server returned an invalid or unrecognized response.
    ErrorHttpInvalidServerResponse,
    //
    // 摘要:
    //     Indicates that the connection was stopped.
    ConnectionAborted,
    //
    // 摘要:
    //     Indicates that the connection was reset.
    ConnectionReset,
    //
    // 摘要:
    //     Indicates that the Internet connection has been lost.
    Disconnected,
    //
    // 摘要:
    //     Indicates that a connection to the destination was not established.
    CannotConnect,
    //
    // 摘要:
    //     Indicates that the provided host name was not able to be resolved.
    HostNameNotResolved,
    //
    // 摘要:
    //     Indicates that the operation was canceled. This status code is also used in the
    //     following cases:
    OperationCanceled,
    //
    // 摘要:
    //     Indicates that the request redirect failed.
    RedirectFailed,
    //
    // 摘要:
    //     An unexpected error occurred.
    UnexpectedError,
    //
    // 摘要:
    //     Indicates that user is prompted with a login, waiting on user action. Initial
    //     navigation to a login site will always return this even if app provides credential
    //     using Microsoft.Web.AsService.Core.CoreWebView2.BasicAuthenticationRequested.
    //     HTTP response status code in this case is 401. See status code reference here:
    //     https://developer.mozilla.org/docs/Web/HTTP/Status.
    ValidAuthenticationCredentialsRequired,
    //
    // 摘要:
    //     Indicates that user lacks proper authentication credentials for a proxy server.
    //     HTTP response status code in this case is 407. See status code reference here:
    //     https://developer.mozilla.org/docs/Web/HTTP/Status.
    ValidProxyAuthenticationRequired
}
