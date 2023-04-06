namespace Lantern.Windows;

public interface ITrayManager
{
    ITray CreateTray(TrayOptions options);
    ITray? GetDefaultTray();
    ITray? GetTray(string name);
    ITray[] GetAllTarys();
}