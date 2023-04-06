using Lantern.Messaging.Serialization;
using System.Text.Json.Serialization;

namespace Lantern.Windows;

[JsonStringEnumConverter]
public enum WindowTitleBarStyle
{
    Default,
    None,
}
