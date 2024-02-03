using Lantern.Platform;
using Lantern.Threading;

namespace Lantern.Windows;

public class Window : IWindow
{
    private readonly IWindowImpl _impl;
    private bool _forceClose;

    public Window(IWindowImpl impl)
    {
        _impl = impl;
        _size = _impl.GetClientSize().ToLogisticSize(_impl.Scaling);
        _impl.Activated = HandleActivated;
        _impl.Deactivated = HandleDeactivated;
        _impl.LocationChanged = HandleMoved;
        _impl.SizeChanged = HandleResized;
        _impl.Closing = HandleClosing;
        _impl.Closed = HandleClosed;
        _impl.WindowStateChanged = HandleWindowStateChanged;
    }

    public event Action? Closed;
    public event Func<bool>? Closing;
    public event Action<LogisticSize>? Resized;
    public event Action<WindowState>? StateChanged;
    public event Action<bool>? VisibleChanged;

    public event Action<LogisticPosition>? Moved;

    private LogisticSize _size;
    private LogisticPosition _position;

    private PhysicsSize _physicsSize;
    private PhysicsPosition _physicsPosition;

    private bool _activated;
    private bool _visible;
    private bool _resizble = true;
    private bool _skipTaskbar = false;
    private WindowTitleBarStyle _titleBarStyle = WindowTitleBarStyle.Default;
    private string? _icon;

    public IWindowImpl WindowImpl => _impl;
    public bool IsActive => _activated;
    public bool IsVisible => _visible;

    public bool IsMaximized => _impl.WindowState == WindowState.Maximized;
    public bool IsMinimized => _impl.WindowState == WindowState.Minimized;
    public bool IsFullScreen => _impl.WindowState == WindowState.FullScreen;

    public double ScaleFactor => _impl.Scaling;

    public string? Title
    {
        get => _impl.Title;
        set => _impl.SetTitle(value!);
    }

    public LogisticSize MinSize
    {
        get => _impl.MinSize.ToLogisticSize(_impl.Scaling);
        set => _impl.MinSize = value.ToPhysicsSize(_impl.Scaling);
    }

    public LogisticSize MaxSize
    {
        get => _impl.MaxSize.ToLogisticSize(_impl.Scaling);
        set => _impl.MaxSize = value.ToPhysicsSize(_impl.Scaling);
    }

    public string? Icon
    {
        get => _icon;
        set
        {
            if (_icon != value)
            {
                _icon = value;
                _impl.SetIcon(value!);
            }
        }
    }

    public bool Resizable
    {
        get => _resizble;
        set
        {
            if (_resizble != value)
            {
                _resizble = value;
                _impl.SetResizable(_resizble);
            }
        }
    }

    public LogisticSize Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _physicsSize = value.ToPhysicsSize(_impl.Scaling);
                _size = value;
                _impl.SetClientSize(_physicsSize.Width, _physicsSize.Height);
            }
        }
    }

    public PhysicsSize PhysicsSize
    {
        get => _physicsSize;
        set
        {
            if (_physicsSize != value)
            {
                _size = value.ToLogisticSize(_impl.Scaling);
                _physicsSize = value;
                _impl.SetClientSize(_physicsSize.Width, _physicsSize.Height);
            }
        }
    }

    public LogisticPosition Position
    {
        get => _position;
        set
        {
            if (_position != value)
            {
                _physicsPosition = value.ToPhysicsPosition(_impl.Scaling);
                _position = value;
                _impl.SetLocation(_physicsPosition.X, _physicsPosition.Y);
            }
        }
    }

    public PhysicsPosition PhysicsPosition
    {
        get => _physicsPosition;
        set
        {
            if (_physicsPosition != value)
            {
                _position = value.ToLogisticPosition(_impl.Scaling);
                _physicsPosition = value;
                _impl.SetLocation(_physicsPosition.X, _physicsPosition.Y);
            }
        }
    }

    public bool AlwaysOnTop
    {
        get => _impl.IsTopmost;
        set => _impl.SetTopmost(value);
    }

    public bool SkipTaskbar
    {
        get => _skipTaskbar;
        set
        {
            if (_skipTaskbar != value)
            {
                _skipTaskbar = value;
                _impl.ShowInTaskbar(!value);
            }
        }
    }

    public WindowTitleBarStyle TitleBarStyle
    {
        get => _titleBarStyle;
        set
        {
            if (_titleBarStyle != value)
            {
                _titleBarStyle = value;
                _impl.SetSystemDecorations(value == WindowTitleBarStyle.None ? WindowDecorations.None : WindowDecorations.Full);
            }
        }
    }

    public virtual void StartDragMove()
    {
        if (Dispatcher.UIThread.CheckAccess())
        {
            _impl.StartDragMove();
        }
        else
        {
            Dispatcher.UIThread.Post(() => _impl.StartDragMove());
        }
    }

    public virtual void Show()
    {
        _visible = true;
        _impl.Show();
        OnVisibleChanged();
        VisibleChanged?.Invoke(_visible);
    }

    public virtual void Hide()
    {
        _visible = false;
        _impl.Hide();
        OnVisibleChanged();
        VisibleChanged?.Invoke(_visible);
    }

    public virtual void Activate()
    {
        var changed = !_visible;
        _visible = true;
        _impl.Activate();

        if (changed)
        {
            OnVisibleChanged();
            VisibleChanged?.Invoke(_visible);
        }
    }

    public virtual void Maximize() => _impl.SetWindowState(WindowState.Maximized);

    public virtual void Minimize() => _impl.SetWindowState(WindowState.Minimized);

    public virtual void Restore() => _impl.SetWindowState(WindowState.Normal);

    public virtual void FullScreen() => _impl.SetWindowState(WindowState.FullScreen);

    public virtual void Close() => _impl.Close();

    public virtual void Close(bool forceClose)
    {
        _forceClose = forceClose;
        Close();
    }

    public virtual void Center()
    {
        var screen = _impl.Screen.ScreenFromWindow(_impl);
        if (screen != null)
        {
            var x = (screen.PhysicsWorkingArea.Width - _physicsSize.Width) / 2;
            var y = (screen.PhysicsWorkingArea.Height - _physicsSize.Height) / 2;
            if (x > 0 && y > 0)
            {
                _impl.SetLocation(x, y);
            }
        }
    }


    public Screen? GetScreen() => _impl.Screen.ScreenFromWindow(_impl);

    private void HandleWindowStateChanged(WindowState state)
    {
        OnWindowStateChanged();
        StateChanged?.Invoke(state);
    }

    private void HandleResized(PhysicsSize size)
    {
        _physicsSize = size;
        _size = size.ToLogisticSize(_impl.Scaling);
        OnResized();
        Resized?.Invoke(_size);
    }

    private void HandleMoved(PhysicsPosition position)
    {
        _physicsPosition = position;
        _position = position.ToLogisticPosition(_impl.Scaling);
        OnMoved();
        Moved?.Invoke(_position);
    }

    private void HandleDeactivated()
    {
        _activated = false;
        OnDeactivated();
    }

    private void HandleActivated()
    {
        _activated = true;
        OnActivated();
    }

    private bool HandleClosing()
    {
        if (_forceClose)
            return false;

        OnClosing();

        return Closing?.Invoke() ?? false;
    }

    private void HandleClosed()
    {
        OnClosed();
        Closed?.Invoke();
    }

    protected virtual void OnDeactivated() { }

    protected virtual void OnActivated() { }

    protected virtual void OnClosing() { }

    protected virtual void OnClosed() { }

    protected virtual void OnWindowStateChanged() { }

    protected virtual void OnMoved() { }

    protected virtual void OnResized() { }

    protected virtual void OnVisibleChanged() { }

}
