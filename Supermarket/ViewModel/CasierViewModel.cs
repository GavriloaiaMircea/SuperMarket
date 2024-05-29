using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Supermarket.Helpers;
using Supermarket.Models;

namespace Supermarket.ViewModel
{
    public class CasierViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Stoc> Stocks { get; set; }
        public ObservableCollection<BonProdus> BonProducts { get; set; }
        public Stoc SelectedStock { get; set; }
        public Bon CurrentBon { get; set; }
        public string SearchTerm { get; set; }
        public User CurrentUser { get; private set; }

        public ICommand SearchByNameCommand { get; set; }
        public ICommand SearchByBarcodeCommand { get; set; }
        public ICommand AddToBonCommand { get; set; }
        public ICommand CancelBonCommand { get; set; }
        public ICommand EmitBonCommand { get; set; }

        public CasierViewModel(User curentUser)
        {
            CurrentUser = curentUser;
            Stocks = new ObservableCollection<Stoc>();
            BonProducts = new ObservableCollection<BonProdus>();
            CurrentBon = new Bon();

            SearchByNameCommand = new RelayCommand(SearchByName);
            SearchByBarcodeCommand = new RelayCommand(SearchByBarcode);
            AddToBonCommand = new RelayCommand(AddToBon);
            CancelBonCommand = new RelayCommand(CancelBon);
            EmitBonCommand = new RelayCommand(EmitBon);

            LoadStocks();
        }

        private void LoadStocks()
        {
            Stocks.Clear();
            var allStocks = DataService.GetAllStocks();
            foreach (var stock in allStocks)
            {
                Stocks.Add(stock);
            }
        }

        private void SearchByName(object parameter)
        {
            Stocks.Clear();
            var foundStocks = DataService.SearchStocksByName(SearchTerm);
            foreach (var stock in foundStocks)
            {
                Stocks.Add(stock);
            }
        }

        private void SearchByBarcode(object parameter)
        {
            Stocks.Clear();
            var foundStocks = DataService.SearchStocksByBarcode(SearchTerm);
            foreach (var stock in foundStocks)
            {
                Stocks.Add(stock);
            }
        }

        private void AddToBon(object parameter)
        {
            if (SelectedStock == null) return;

            var existingProduct = BonProducts.FirstOrDefault(p => p.NumeProdus == SelectedStock.ProductName);

            if (existingProduct != null)
            {
                if (SelectedStock.Cantitate >= 1)
                {
                    existingProduct.Cantitate++;
                    existingProduct.Subtotal = existingProduct.Cantitate * SelectedStock.PretVanzare;
                    SelectedStock.Cantitate--;
                }
                else
                {
                    MessageBox.Show("Not enough stock available.", "Stock Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                if (SelectedStock.Cantitate >= 1)
                {
                    BonProducts.Add(new BonProdus
                    {
                        NumeProdus = SelectedStock.ProductName,
                        Cantitate = 1,
                        Subtotal = SelectedStock.PretVanzare
                    });
                    SelectedStock.Cantitate--;
                }
                else
                {
                    MessageBox.Show("Not enough stock available.", "Stock Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

            OnPropertyChanged(nameof(BonProducts));
            OnPropertyChanged(nameof(Stocks));
        }

        private void CancelBon(object parameter)
        {
            foreach (var bonProdus in BonProducts)
            {
                var stock = Stocks.FirstOrDefault(s => s.ProductName == bonProdus.NumeProdus);
                if (stock != null)
                {
                    stock.Cantitate += bonProdus.Cantitate;
                }
            }
            BonProducts.Clear();
            OnPropertyChanged(nameof(BonProducts));
            OnPropertyChanged(nameof(Stocks));
        }

        private void EmitBon(object parameter)
        {
            if (BonProducts.Count == 0) return;

            CurrentBon.DataEliberarii = DateTime.Now;
            CurrentBon.Casier = CurrentUser.Username;
            CurrentBon.Produse = BonProducts.ToList();
            CurrentBon.Total = BonProducts.Sum(p => p.Subtotal);

            try
            {
                // Update stock quantities
                foreach (var bonProdus in BonProducts)
                {
                    var stock = Stocks.FirstOrDefault(s => s.ProductName == bonProdus.NumeProdus);
                    if (stock != null)
                    {
                        DataService.UpdateStockQuantity(stock.ProductID, stock.Cantitate);
                    }
                }

                DataService.EmitBon(CurrentBon);
                MessageBox.Show("Bon emis cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                BonProducts.Clear();
                LoadStocks();
                OnPropertyChanged(nameof(BonProducts));
                OnPropertyChanged(nameof(Stocks));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Eroare la emiterea bonului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
