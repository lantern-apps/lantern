//using System.Drawing;
//using System.Runtime.InteropServices;
//using static Lantern.Win32.Interop.NativeMethods;

//namespace Lantern.Win32;

//public partial class LoadingWindowImpl : WindowImpl
//{
//    private readonly Image img = Image.FromFile("C:\\Users\\xiaox\\Downloads\\loading.gif");
//    //private readonly Font font = new Font("宋体", 20);
//    private readonly Font font = new Font(SystemFonts.DefaultFont.Name, 20);
//    private int _width;
//    private int _height;

//    [DllImport("user32.dll")]
//    private static extern bool InvalidateRect(IntPtr hWnd,
//                                              RECT lpRect,
//                                              bool bErase);

//    //[DllImport("user32.dll")]
//    //private static extern bool InvalidateRect(IntPtr hWnd,
//    //                                          IntPtr lpRect,
//    //                                          bool bErase);
//    public LoadingWindowImpl()
//    {
//        StartAnimate();
//    }

//    public void StartAnimate()
//    {
//        _width = img.Width;
//        _height = img.Height;
//        ImageAnimator.Animate(img, Repaint);
//    }

//    private void Repaint(object o, EventArgs e)
//    {
//        GetClientRect(NativeHandle, out RECT rect);

//        var x = (rect.Width - _width) / 2;
//        var y = (rect.Height - _height) / 2 + 50;
//        //InvalidateRect(NativeHandle, default, true);
//        InvalidateRect(NativeHandle, new RECT
//        {
//            left = x,
//            top = y,
//            right = x + _width,
//            bottom = y + _height,
//        }, true);
//    }

//    protected override void OnPaint(nint hWnd)
//    {
//        var dc = BeginPaint(hWnd, out PAINTSTRUCT ps);
//        if (dc != IntPtr.Zero)
//        {
//            //var r = ps.rcPaint;
//            //var size = GetClientSize();

//            using var g = Graphics.FromHdc(dc);
//            ImageAnimator.UpdateFrames(img);
//            //g.Clear(Color.White);

//            GetClientRect(NativeHandle, out RECT rect);

//            var x = (rect.Width - _width) / 2;
//            var y = (rect.Height - _height) / 2 + 50;

//            g.DrawImage(img, x, y, _width, _height);
//            var fontSize = g.MeasureString("正在安装必要组件...", font);

//            var fx = (rect.Width - fontSize.Width) / 2;
//            var fy = (rect.Height - fontSize.Height) / 2 - 50;

//            g.DrawString("正在安装必要组件...", font, Brushes.Black, new PointF(fx, fy));
//            g.ReleaseHdc(dc);
//            EndPaint(hWnd, ref ps);

//        }
//    }
//}
