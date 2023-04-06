using Lantern.Threading;
using Lantern.Win32;
using static Lantern.Win32.Interop.NativeMethods;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        WebView2WindowFactory1.SetDpiAwareness();

        //Thread thread = new(Run);
        //thread.SetApartmentState(ApartmentState.STA);
        //thread.Start();
        //thread.Join();
        Run();
    }

    public static void Run()
    {


        ScreenImpl screen = new();
        var a = screen.AllScreens;

        for (int i = 0; i < 1; i++)
        {
            var hwnd = WebView2WindowFactory1.CreateWindow();
            WebView2WindowFactory1.AttachWebView2(hwnd);
            ShowWindow(hwnd, ShowWindowCommand.Show);
        }

        WebView2WindowFactory1.RunMessagePump();
    }
}