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
using Supermarket.ViewModel;
namespace Supermarket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LoginViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            this.DataContext = viewModel;
            viewModel.CloseAction = new Action(this.Close);
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            viewModel.Password = passwordBox.Password;
            viewModel.Login();
        }
    }

}