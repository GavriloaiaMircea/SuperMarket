using Supermarket.Helpers;
using Supermarket.Models;
using Supermarket.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    public class StockManagementViewModel
    {
        public ObservableCollection<Stoc> Stocks { get; set; }

        public ICommand AddStockCommand { get; private set; }

        public StockManagementViewModel()
        {
            Stocks = new ObservableCollection<Stoc>(DataService.GetAllStocks());
            ReloadStocks();
            AddStockCommand = new RelayCommand(AddStock);
        }
        public void ReloadStocks()
        {
            Stocks.Clear();
            var updatedStocks = DataService.GetAllStocks();
            foreach (var stock in updatedStocks)
            {
                if (stock.Cantitate > 0 && (stock.DataExpirarii == null || stock.DataExpirarii > DateTime.Now))
                {
                    Stocks.Add(stock);
                }
            }
        }



        private void AddStock(object parameter)
        {
            AddStockWindow AddStockWindow = new AddStockWindow();
            AddStockWindow.Closed += (sender, args) => ReloadStocks();
            AddStockWindow.Show();
        }
    }
}
