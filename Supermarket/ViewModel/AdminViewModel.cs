using System.Windows.Input;
using Supermarket.Helpers;
using Supermarket.Views;

namespace Supermarket.ViewModel
{
    public class AdminViewModel
    {
        public ICommand OpenUserManagementCommand { get; private set; }
        public ICommand OpenStockManagementCommand { get; private set; }
        public ICommand OpenProductManagementCommand { get; private set; }  
        public ICommand OpenManufacturersManagementCommand { get; private set; }
        public ICommand OpenCategoriesManagementCommand { get; private set; } 
        public ICommand OpenBillMenuCommand { get; private set; }

        public AdminViewModel()
        {
            OpenUserManagementCommand = new RelayCommand(OpenUserManagement);
            OpenStockManagementCommand = new RelayCommand(OpenStockManagement);
            OpenProductManagementCommand = new RelayCommand(OpenProductManagement);
            OpenManufacturersManagementCommand = new RelayCommand(OpenManufacturersManagement);
            OpenCategoriesManagementCommand = new RelayCommand(OpenCategoriesManagement);
            OpenBillMenuCommand = new RelayCommand(OpenBillMenu);
        }
        private void OpenBillMenu(object obj)
        {
            BillMenuWindow billMenuWindow = new BillMenuWindow();
            billMenuWindow.Show();
        }
        private void OpenManufacturersManagement(object obj)
        {
            ManufacturesWindow manufacturesManagementWindow = new ManufacturesWindow();
            manufacturesManagementWindow.Show();
        }
        private void OpenCategoriesManagement(object obj)
        {
            CategoriesWindow categoriesManagementWindow = new CategoriesWindow();
            categoriesManagementWindow.Show();
        }
        private void OpenUserManagement(object obj)
        {
            UserManagementWindow userManagementWindow = new UserManagementWindow();
            userManagementWindow.Show();
        }
        private void OpenStockManagement(object obj)
        {
            StockManagementWindow stockManagementWindow = new StockManagementWindow();
            stockManagementWindow.Show();
        }

        private void OpenProductManagement(object obj)
        {
            ProductManagementWindow productManagementWindow = new ProductManagementWindow();
            productManagementWindow.Show();
        }   
    }
}
