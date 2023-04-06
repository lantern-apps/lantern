using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Lantern.AsServices.WpfAppTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = await ((App)Application.Current).Services
                .GetRequiredService<IWebViewWindowManager>()
                .CreateWindowAsync(new WebViewWindowOptions());
            window.Show();
                
        }
    }
}
