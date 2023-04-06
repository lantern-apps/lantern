using Lantern.Windows;

namespace Lantern.Platform;

public interface IWindowImpl
{
    IntPtr NativeHandle { get; }
    PhysicsSize MinSize { get; set; }
    PhysicsSize MaxSize { get; set; }
    WindowState WindowState { get; }
    IScreenImpl Screen { get; }
    bool IsTopmost { get; }
    string? Title { get; }
    double Scaling { get; }
    PhysicsSize GetClientSize();
    void SetTitle(string title);
    void SetTopmost(bool value);
    void SetResizable(bool enable);
    void SetLocation(int? x, int? y);
    void SetClientSize(int? width, int? height);
    void SetIcon(string iconPath);
    void SetWindowState(WindowState windowState);
    void SetSystemDecorations(WindowDecorations value);
    void ShowInTaskbar(bool value);
    void StartDragMove();

    void Show();
    void Hide();
    void Close();
    void Activate();

    Action? Deactivated { get; set; }
    Action? Activated { get; set; }
    Action<PhysicsPosition>? LocationChanged { get; set; }
    Action<PhysicsSize>? SizeChanged { get; set; }
    Action<WindowState>? WindowStateChanged { get; set; }
    Func<bool>? Closing { get; set; }
    Action? Closed { get; set; }
}
