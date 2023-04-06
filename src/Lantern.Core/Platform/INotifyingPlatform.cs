namespace Lantern.Platform;

public interface INotifyingPlatform
{
    bool ShowNotification(string title, string? info);
}