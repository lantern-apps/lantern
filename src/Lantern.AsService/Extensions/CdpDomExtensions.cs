using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;

namespace Lantern.AsService;

public static class CdpDomExtensions
{
    public static DOM.Node? Find(this DOM.Node node, string nodeName)
    {
        return Find(node, node => node.NodeName == nodeName);
    }

    public static DOM.Node? Find(this DOM.Node node, Func<DOM.Node, bool> selector)
    {
        if (selector(node))
        {
            return node;
        }

        if (node.Children != null)
        {
            foreach (var child in node.Children)
            {
                var result = Find(child, selector);
                if (result != null)
                    return result;
            }
        }

        return null;
    }

}
