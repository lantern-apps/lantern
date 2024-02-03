using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;

namespace Lantern.AsService;

public static class CdpDomExtensions
{
    public static DOM.Node? Find(this DOM.Node node, string nodeName)
    {
        if (node.NodeName == nodeName)
        {
            return node;
        }

        if (node.Children != null)
        {
            foreach (var child in node.Children)
            {
                var result = Find(child, nodeName);
                if (result != null)
                    return result;
            }
        }

        return null;
    }
}
