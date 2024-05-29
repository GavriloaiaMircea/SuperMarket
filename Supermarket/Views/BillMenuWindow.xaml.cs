using Supermarket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Supermarket.Views
{
    /// <summary>
    /// Interaction logic for BillMenuWindow.xaml
    /// </summary>
    public partial class BillMenuWindow : Window
    {
        public BillMenuWindow()
        {
            InitializeComponent();
            var viewModel = new BillMenuViewModel();
            DataContext = viewModel;
        }
    }
}
