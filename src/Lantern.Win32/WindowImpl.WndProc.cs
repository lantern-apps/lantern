using Lantern.Threading;
using Lantern.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Lantern.Win32.Interop.NativeMethods;

namespace Lantern.Win32;

public partial class WindowImpl
{
    protected virtual void OnPaint(IntPtr hWnd) { }

    protected virtual unsafe IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        var message = (WindowsMessage)msg;

        try
        {
            switch (message)
            {
                case WindowsMessage.WM_LBUTTONDOWN:
                case WindowsMessage.WM_RBUTTONDOWN:
                case WindowsMessage.WM_MBUTTONDOWN:
                case WindowsMessage.WM_XBUTTONDOWN:
                    break;
                case WindowsMessage.WM_LBUTTONUP:
                case WindowsMessage.WM_RBUTTONUP:
                case WindowsMessage.WM_MBUTTONUP:
                case WindowsMessage.WM_XBUTTONUP:
                    break;

                case WindowsMessage.WM_MOUSEMOVE:
                    break;
                case WindowsMessage.WM_NCPOINTERDOWN:
                case WindowsMessage.WM_NCPOINTERUP:
                case WindowsMessage.WM_POINTERDOWN:
                case WindowsMessage.WM_POINTERUP:
                case WindowsMessage.WM_POINTERUPDATE:
                    break;
                case WindowsMessage.WM_NCPAINT:
                    {
                        if (_windowProperties.Decorations != WindowDecorations.Full)
                        {
                            return IntPtr.Zero;
                        }

                        break;
                    }
                //case WindowsMessage.WM_NCLBUTTONDOWN:
                //    return 0;
                case WindowsMessage.WM_ERASEBKGND:
                    break;
                case WindowsMessage.WM_PAINT:
                    {
                        OnPaint(hWnd);
                    }
                    break;
                case WindowsMessage.WM_ACTIVATE:
                    {
                        var wa = (WindowActivate)(ToInt32(wParam) & 0xffff);

                        switch (wa)
                        {
                            case WindowActivate.WA_ACTIVE:
                            case WindowActivate.WA_CLICKACTIVE:
                                {
                                    Activated?.Invoke();
                                    break;
                                }
                            case WindowActivate.WA_INACTIVE:
                                {
                                    Deactivated?.Invoke();
                                    break;
                                }
                        }

                        return IntPtr.Zero;
                    }
                case WindowsMessage.WM_GETMINMAXINFO:
                    {
                        MINMAXINFO mmi = Marshal.PtrToStructure<MINMAXINFO>(lParam);

                        //_maxTrackSize = mmi.ptMaxTrackSize;

                        if (MinSize.Width > 0)
                        {
                            mmi.ptMinTrackSize.X = MinSize.Width;
                        }

                        if (MinSize.Height > 0)
                        {
                            mmi.ptMinTrackSize.Y = MinSize.Height;
                        }

                        if (!double.IsInfinity(MaxSize.Width) && MaxSize.Width > 0)
                        {
                            mmi.ptMaxTrackSize.X = MaxSize.Width;
                        }

                        if (!double.IsInfinity(MaxSize.Height) && MaxSize.Height > 0)
                        {
                            mmi.ptMaxTrackSize.Y = MaxSize.Height;
                        }

                        Marshal.StructureToPtr(mmi, lParam, true);
                        return IntPtr.Zero;
                    }

                case WindowsMessage.WM_SIZE:
                    {
                        var mode = (SizeCommand)wParam;

                        if (SizeChanged != null && (mode == SizeCommand.Restored || mode == SizeCommand.Maximized))
                        {
                            var clientSize = new PhysicsSize(ToInt32(lParam) & 0xffff, ToInt32(lParam) >> 16);
                            SizeChanged(clientSize);
                        }

                        var windowState = mode switch
                        {
                            SizeCommand.Maximized => WindowState.Maximized,
                            SizeCommand.Minimized => WindowState.Minimized,
                            _ when _isFullScreenActive => WindowState.FullScreen,
                            _ => WindowState.Normal,
                        };

                        if (windowState != _windowState)
                        {
                            _windowState = windowState;
                            WindowStateChanged?.Invoke(windowState);
                        }

                        return IntPtr.Zero;
                    }
                case WindowsMessage.WM_MOVE:
                    {
                        var x = (short)(ToInt32(lParam) & 0xffff);
                        var y = (short)(ToInt32(lParam) >> 16);
                        LocationChanged?.Invoke(new PhysicsPosition(x, y));
                        return IntPtr.Zero;
                    }
                case WindowsMessage.WM_CLOSE:
                    {
                        bool? preventClosing = Closing?.Invoke();
                        if (preventClosing == true)
                        {
                            return IntPtr.Zero;
                        }
                        break;
                    }
                case WindowsMessage.WM_DESTROY:
                    {
                        _hwnd = IntPtr.Zero;
                        _instances.Remove(this);

                        Closed?.Invoke();
                        Dispatcher.UIThread.Post(OnDestory);
                        return IntPtr.Zero;
                    }
                case WindowsMessage.WM_DPICHANGED:
                    {
                        var dpi = ToInt32(wParam) & 0xffff;
                        var newDisplayRect = Marshal.PtrToStructure<RECT>(lParam);
                        _scaling = dpi / 96.0;

                        ScalingChanged?.Invoke(_scaling);

                        SetWindowPos(hWnd,
                            IntPtr.Zero,
                            newDisplayRect.left,
                            newDisplayRect.top,
                            newDisplayRect.right - newDisplayRect.left,
                            newDisplayRect.bottom - newDisplayRect.top,
                            SetWindowPosFlags.SWP_NOZORDER |
                            SetWindowPosFlags.SWP_NOACTIVATE);

                        return IntPtr.Zero;
                    }
            }
        }
        catch (Exception ex)
        {
            Debug.Print(ex.Message);
        }

        return DefWindowProc(hWnd, msg, wParam, lParam);
    }

    private static int ToInt32(IntPtr ptr)
    {
        if (IntPtr.Size == 4)
            return ptr.ToInt32();

        return (int)(ptr.ToInt64() & 0xffffffff);
    }

    private void OnDestory()
    {
        if (_className != null)
        {
            UnregisterClass(_className, GetModuleHandle(null));
            _className = null!;
        }
    }

    public void Dispose()
    {
        if (_hwnd != IntPtr.Zero)
        {
            _instances.Remove(this);
            DestroyWindow(_hwnd);
            _hwnd = IntPtr.Zero;
        }
    }

}
