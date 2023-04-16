using Lantern.Windows;

namespace Lantern.Platform;

public interface INotifyingPlatform
{
    bool ShowNotification(string title, string? info, MessageIconType iconType = MessageIconType.None);
}