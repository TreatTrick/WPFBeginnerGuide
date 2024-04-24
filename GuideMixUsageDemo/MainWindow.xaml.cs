using FreshGuidance;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GuideMixUsageDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UserControl1.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            (wrapper.Content as GuideMask)?.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            (wrapper2.Content as GuideMask)?.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, () =>
            {
                var dialog = new Dialog();
                dialog.Width = 400;
                dialog.Height = 400;
                dialog.Owner = this;
                dialog.Left = 100;
                dialog.Top = 100;
                dialog.ShowDialog();
            });
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserControl1.Visibility = Visibility.Visible;
        }
    }
}