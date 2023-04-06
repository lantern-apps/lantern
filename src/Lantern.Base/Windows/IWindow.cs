namespace Lantern.Windows;

public interface IWindow
{
    double ScaleFactor { get; }
    bool IsActive { get; }
    bool IsVisible { get; }
    bool IsMaximized { get; }
    bool IsMinimized { get; }
    bool IsFullScreen { get; }
    string? Title { get; set; }
    LogisticSize MinSize { get; set; }
    LogisticSize MaxSize { get; set; }
    LogisticSize Size { get; set; }
    LogisticPosition Position { get; set; }
    PhysicsSize PhysicsSize { get; set; }
    PhysicsPosition PhysicsPosition { get; set; }

    string? Icon { get; set; }
    bool Resizable { get; set; }
    bool AlwaysOnTop { get; set; }
    bool SkipTaskbar { get; set; }
    WindowTitleBarStyle TitleBarStyle { get; set; }

    void Show();
    void Hide();
    void Activate();
    void Maximize();
    void Minimize();
    void Restore();
    void FullScreen();
    void Close(bool forceClose = false);
    void Center();

    void StartDragMove();
    Screen? GetScreen();

    event Action? Closed;
    event Func<bool>? Closing;
    event Action<LogisticSize>? Resized;
    event Action<WindowState>? StateChanged;
    event Action<LogisticPosition>? Moved;
}
