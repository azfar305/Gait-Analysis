using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Trace
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GaitExtraction());
        }
    }
}
