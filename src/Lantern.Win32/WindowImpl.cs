using Lantern.Platform;
using Lantern.Win32.Interop;
using Lantern.Windows;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public partial class WindowImpl : IWindowImpl, IDisposable
{
    private string? _title;
    private bool _topmost;
    private WindowState _windowState;
    private WindowProperties _windowProperties;
    private bool _isFullScreenActive;
    private double _scaling = 1;
    private bool _shown;
    private bool _hiddenWindowIsParent;
    private SavedWindowInfo _savedWindowInfo;
    private WindowImpl? _parent;

    public Action? Activated { get; set; }
    public Action? Deactivated { get; set; }
    public Action<PhysicsSize>? SizeChanged { get; set; }
    public Action<PhysicsPosition>? LocationChanged { get; set; }
    public Action<WindowState>? WindowStateChanged { get; set; }
    public Action<double>? ScalingChanged { get; set; }
    public Action? Closed { get; set; }
    public Func<bool>? Closing { get; set; }

    public PhysicsSize MinSize { get; set; }
    public PhysicsSize MaxSize { get; set; }
    public double Scaling => _scaling;
    public bool IsTopmost => _topmost;
    public nint NativeHandle => _hwnd;
    public string? Title => _title;
    public WindowState WindowState => _windowState;

    public IScreenImpl Screen => Win32Platform.Instance.Screen;

    public virtual void SetWindowState(WindowState windowState)
    {
        if (_windowState != windowState)
        {
            _windowState = windowState;
            ShowWindow(_windowState, windowState != WindowState.Minimized);
        }
    }

    public virtual void Show()
    {
        var state = _windowState == WindowState.Minimized ? WindowState.Normal : _windowState;
        ShowWindow(state, false);
    }

    public virtual void Activate()
    {
        var state = _windowState == WindowState.Minimized ? WindowState.Normal : _windowState;
        ShowWindow(state, true);
    }

    public virtual void Hide()
    {
        NativeMethods.ShowWindow(_hwnd, ShowWindowCommand.Hide);
        _shown = false;
    }

    public virtual void Close()
    {
        _ = SendMessage(_hwnd, (uint)WindowsMessage.WM_CLOSE, 0, 0);
    }

    public void SetTitle(string title)
    {
        if (_title != title && SetWindowText(_hwnd, title))
        {
            _title = title;
        }
    }

    public void SetLocation(int? x, int? y)
    {
        if (!x.HasValue && !y.HasValue)
            return;

        if (x.HasValue && y.HasValue)
        {
            SetWindowPos(_hwnd, IntPtr.Zero,
                x.Value,
                y.Value,
                0, 0,
                SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOZORDER);
        }
        else
        {
            GetClientRect(_hwnd, out var clientRect);
            int px = x.HasValue ? x.Value : clientRect.left;
            int py = y.HasValue ? y.Value : clientRect.top;

            if (px != clientRect.left || py != clientRect.top)
            {
                SetWindowPos(_hwnd, IntPtr.Zero,
                   px,
                   py,
                   0, 0,
                   SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOZORDER);
            }
        }
    }

    public void SetClientSize(int? width, int? height)
    {
        if (WindowState != WindowState.Normal)
            return;

        GetClientRect(_hwnd, out var clientRect);

        int requestedClientWidth = width.HasValue ? width.Value : clientRect.Width;
        int requestedClientHeight = height.HasValue ? height.Value : clientRect.Height;

        // do comparison after scaling to avoid rounding issues
        if (requestedClientWidth != clientRect.Width || requestedClientHeight != clientRect.Height)
        {
            GetWindowRect(_hwnd, out var windowRect);

            //using var scope = SetResizeReason(reason);
            SetWindowPos(
                _hwnd,
                IntPtr.Zero,
                0,
                0,
                requestedClientWidth + (windowRect.Width - clientRect.Width),
                requestedClientHeight + (windowRect.Height - clientRect.Height),
                SetWindowPosFlags.SWP_RESIZE);
        }
    }

    public PhysicsSize GetClientSize()
    {
        GetClientRect(_hwnd, out var rect);
        return new PhysicsSize(rect.right, rect.bottom);
    }

    public void SetTopmost(bool value)
    {
        if (value == _topmost)
            return;

        IntPtr hWndInsertAfter = value ? WindowPosZOrder.HWND_TOPMOST : WindowPosZOrder.HWND_NOTOPMOST;
        SetWindowPos(_hwnd,
            hWndInsertAfter,
            0, 0, 0, 0,
            SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOACTIVATE);

        _topmost = value;
    }

    public virtual void SetSystemDecorations(WindowDecorations value)
    {
        var newWindowProperties = _windowProperties;
        newWindowProperties.Decorations = value;
        UpdateWindowProperties(newWindowProperties);
    }

    public virtual void ShowInTaskbar(bool value)
    {
        var newWindowProperties = _windowProperties;
        newWindowProperties.ShowInTaskbar = value;
        UpdateWindowProperties(newWindowProperties);
    }

    public virtual void SetResizable(bool enable)
    {
        var newWindowProperties = _windowProperties;
        newWindowProperties.IsResizable = enable;
        UpdateWindowProperties(newWindowProperties);
    }

    private void ShowWindow(WindowState state, bool activate)
    {
        _shown = true;
        ShowWindowCommand? command = state switch
        {
            WindowState.Minimized => (ShowWindowCommand?)ShowWindowCommand.Minimize,
            WindowState.Maximized => (ShowWindowCommand?)ShowWindowCommand.Maximize,
            WindowState.Normal => (ShowWindowCommand?)(IsWindowVisible(_hwnd) ? ShowWindowCommand.Restore : activate ? ShowWindowCommand.Normal : ShowWindowCommand.ShowNoActivate),
            WindowState.FullScreen => IsWindowVisible(_hwnd) ? null : ShowWindowCommand.Restore,
            _ => throw new ArgumentException("Invalid WindowState."),
        };

        var newWindowProperties = _windowProperties;
        newWindowProperties.IsFullScreen = state == WindowState.FullScreen;
        UpdateWindowProperties(newWindowProperties);

        if (command.HasValue)
        {
            NativeMethods.ShowWindow(_hwnd, command.Value);
        }

        if (activate)
        {
            SetFocus(_hwnd);
            SetForegroundWindow(_hwnd);
        }
    }

    public void SetParent(IWindowImpl? parent)
    {
        _parent = (WindowImpl?)parent;
        var parentHwnd = _parent?._hwnd ?? IntPtr.Zero;

        if (parentHwnd == IntPtr.Zero && !_windowProperties.ShowInTaskbar)
        {
            parentHwnd = OffscreenParentWindow.Handle;
            _hiddenWindowIsParent = true;
        }

        SetWindowLongPtr(_hwnd, (int)WindowLongParam.GWL_HWNDPARENT, parentHwnd);
    }

    public void SetIcon(string iconPath)
    {
        var hIcon = NativeUtilities.LoadIcon(iconPath);
        PostMessage(_hwnd, (int)WindowsMessage.WM_SETICON, new IntPtr(1), hIcon);
    }

    public void StartDragMove()
    {
        ReleaseCapture();
        DefWindowProc(_hwnd, (int)WindowsMessage.WM_NCLBUTTONDOWN, new IntPtr((int)HitTestValues.HTCAPTION), IntPtr.Zero);
    }

    private void SetFullScreen(bool fullscreen)
    {
        if (fullscreen)
        {
            GetWindowRect(_hwnd, out var windowRect);
            GetClientRect(_hwnd, out var clientRect);

            clientRect.left += windowRect.left;
            clientRect.right += windowRect.left;
            clientRect.top += windowRect.top;
            clientRect.bottom += windowRect.top;

            _savedWindowInfo.WindowRect = clientRect;

            var current = GetStyle();
            var currentEx = GetExtendedStyle();

            _savedWindowInfo.Style = current;
            _savedWindowInfo.ExStyle = currentEx;

            // Set new window style and size.
            SetStyle(current & ~(WindowStyles.WS_CAPTION | WindowStyles.WS_THICKFRAME), false);
            SetExtendedStyle(currentEx & ~(WindowStyles.WS_EX_DLGMODALFRAME | WindowStyles.WS_EX_WINDOWEDGE | WindowStyles.WS_EX_CLIENTEDGE | WindowStyles.WS_EX_STATICEDGE), false);

            // On expand, if we're given a window_rect, grow to it, otherwise do
            // not resize.
            MONITORINFO monitor_info = MONITORINFO.Create();
            GetMonitorInfo(MonitorFromWindow(_hwnd, MONITOR.MONITOR_DEFAULTTONEAREST), ref monitor_info);

            var x = monitor_info.rcMonitor.left;
            var y = monitor_info.rcMonitor.top;
            var width = monitor_info.rcMonitor.right - monitor_info.rcMonitor.left;
            var height = monitor_info.rcMonitor.bottom - monitor_info.rcMonitor.top;

            _isFullScreenActive = true;
            SetWindowPos(_hwnd,
                IntPtr.Zero,
                x, y, width, height,
                SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_FRAMECHANGED);
        }
        else
        {
            const WindowStyles WindowStateMask = (WindowStyles.WS_MAXIMIZE | WindowStyles.WS_MINIMIZE);

            // Reset original window style and size.  The multiple window size/moves
            // here are ugly, but if SetWindowPos() doesn't redraw, the taskbar won't be
            // repainted.  Better-looking methods welcome.
            _isFullScreenActive = false;

            var windowStates = GetStyle() & WindowStateMask;
            SetStyle((_savedWindowInfo.Style & ~WindowStateMask) | windowStates, false);
            SetExtendedStyle(_savedWindowInfo.ExStyle, false);

            // On restore, resize to the previous saved rect size.
            var x = _savedWindowInfo.WindowRect.left;
            var y = _savedWindowInfo.WindowRect.top;
            var width = _savedWindowInfo.WindowRect.right - _savedWindowInfo.WindowRect.left;
            var height = _savedWindowInfo.WindowRect.bottom - _savedWindowInfo.WindowRect.top;


            SetWindowPos(_hwnd, IntPtr.Zero, x, y, width, height,
                        SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_FRAMECHANGED);

            UpdateWindowProperties(_windowProperties, true);
        }

        NativeUtilities.MarkFullscreen(_hwnd, fullscreen);
    }

    private WindowStyles GetStyle()
    {
        if (_isFullScreenActive)
        {
            return _savedWindowInfo.Style;
        }
        else
        {
            return (WindowStyles)GetWindowLong(_hwnd, (int)WindowLongParam.GWL_STYLE);
        }
    }

    private WindowStyles GetExtendedStyle()
    {
        if (_isFullScreenActive)
        {
            return _savedWindowInfo.ExStyle;
        }
        else
        {
            return (WindowStyles)GetWindowLong(_hwnd, (int)WindowLongParam.GWL_EXSTYLE);
        }
    }

    private void SetStyle(WindowStyles style, bool save = true)
    {
        if (save)
        {
            _savedWindowInfo.Style = style;
        }

        if (!_isFullScreenActive)
        {
            SetWindowLong(_hwnd, (int)WindowLongParam.GWL_STYLE, (uint)style);
        }
    }

    private void SetExtendedStyle(WindowStyles style, bool save = true)
    {
        if (save)
        {
            _savedWindowInfo.ExStyle = style;
        }

        if (!_isFullScreenActive)
        {
            SetWindowLong(_hwnd, (int)WindowLongParam.GWL_EXSTYLE, (uint)style);
        }
    }

    private void UpdateWindowProperties(WindowProperties newProperties, bool forceChanges = false)
    {
        var oldProperties = _windowProperties;

        // Calling SetWindowPos will cause events to be sent and we need to respond
        // according to the new values already.
        _windowProperties = newProperties;

        if ((oldProperties.ShowInTaskbar != newProperties.ShowInTaskbar) || forceChanges)
        {
            var exStyle = GetExtendedStyle();

            if (newProperties.ShowInTaskbar)
            {
                exStyle |= WindowStyles.WS_EX_APPWINDOW;

                if (_hiddenWindowIsParent)
                {
                    // Can't enable the taskbar icon by clearing the parent window unless the window
                    // is hidden. Hide the window and show it again with the same activation state
                    // when we've finished. Interestingly it seems to work fine the other way.
                    var shown = IsWindowVisible(_hwnd);
                    var activated = GetActiveWindow() == _hwnd;

                    if (shown)
                        Hide();

                    _hiddenWindowIsParent = false;
                    SetParent(null);

                    if (shown)
                    {
                        SetParent(_parent);
                        ShowWindow(_windowState, activated);
                    }
                    //Show(activated, false);
                }
            }
            else
            {
                // To hide a non-owned window's taskbar icon we need to parent it to a hidden window.
                if (_parent is null)
                {
                    SetWindowLongPtr(_hwnd, (int)WindowLongParam.GWL_HWNDPARENT, OffscreenParentWindow.Handle);
                    _hiddenWindowIsParent = true;
                }

                exStyle &= ~WindowStyles.WS_EX_APPWINDOW;
            }

            SetExtendedStyle(exStyle);
        }

        WindowStyles style;
        if ((oldProperties.IsResizable != newProperties.IsResizable) || forceChanges)
        {
            style = GetStyle();

            if (newProperties.IsResizable)
            {
                style |= WindowStyles.WS_SIZEFRAME;
                style |= WindowStyles.WS_MAXIMIZEBOX;
            }
            else
            {
                style &= ~WindowStyles.WS_SIZEFRAME;
                style &= ~WindowStyles.WS_MAXIMIZEBOX;
            }

            SetStyle(style);
        }

        if (oldProperties.IsFullScreen != newProperties.IsFullScreen)
        {
            SetFullScreen(newProperties.IsFullScreen);
        }

        if ((oldProperties.Decorations != newProperties.Decorations) || forceChanges)
        {
            style = GetStyle();

            const WindowStyles fullDecorationFlags = WindowStyles.WS_CAPTION | WindowStyles.WS_SYSMENU;

            if (newProperties.Decorations == WindowDecorations.Full)
            {
                style |= fullDecorationFlags;
            }
            else
            {
                style &= ~fullDecorationFlags;
            }

            SetStyle(style);

            if (!_isFullScreenActive)
            {
                var margin = newProperties.Decorations == WindowDecorations.BorderOnly ? 1 : 0;

                var margins = new MARGINS
                {
                    cyBottomHeight = margin,
                    cxRightWidth = margin,
                    cxLeftWidth = margin,
                    cyTopHeight = margin
                };

                DwmExtendFrameIntoClientArea(_hwnd, ref margins);

                GetClientRect(_hwnd, out var oldClientRect);
                var oldClientRectOrigin = new POINT();
                ClientToScreen(_hwnd, ref oldClientRectOrigin);
                oldClientRect.Offset(oldClientRectOrigin);

                var newRect = oldClientRect;

                if (newProperties.Decorations == WindowDecorations.Full)
                {
                    AdjustWindowRectEx(ref newRect, (uint)style, false, (uint)GetExtendedStyle());
                }

                SetWindowPos(_hwnd, IntPtr.Zero, newRect.left, newRect.top, newRect.Width, newRect.Height,
                    SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOACTIVATE |
                    SetWindowPosFlags.SWP_FRAMECHANGED);
            }
        }
    }

    private struct WindowProperties
    {
        public bool ShowInTaskbar;
        public bool IsResizable;
        public bool IsFullScreen;
        public WindowDecorations Decorations;
    }

    private struct SavedWindowInfo
    {
        public WindowStyles Style { get; set; }
        public WindowStyles ExStyle { get; set; }
        public RECT WindowRect { get; set; }
    };
}
