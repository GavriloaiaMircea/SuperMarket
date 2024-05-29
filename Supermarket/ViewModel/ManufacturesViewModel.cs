using Supermarket.Helpers;
using Supermarket.Models;
using Supermarket.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    public class ManufacturesViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Producator> Manufacturers { get; set; }
        private Producator _selectedManufacturer;

        public Producator SelectedManufacturer
        {
            get => _selectedManufacturer;
            set { _selectedManufacturer = value; OnPropertyChanged(); }
        }

        public ICommand OpenAddManufacturerCommand { get; }
        public ICommand SaveChangesCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand ShowProductsCommand { get; }

        public ManufacturesViewModel()
        {
            Manufacturers = new ObservableCollection<Producator>(DataService.GetAllProducers());
            OpenAddManufacturerCommand = new RelayCommand(OpenAddManufacturer);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanEditOrDelete);
            RefreshCommand = new RelayCommand(RefreshManufacturers);
            ShowProductsCommand = new RelayCommand(ShowProducts, CanEditOrDelete);
        }

        private void OpenAddManufacturer(object obj)
        {
            AddManufacturesWindow addManufacturesWindow = new AddManufacturesWindow();
            addManufacturesWindow.Show();
        }

        private void SaveChanges(object obj)
        {
            if (SelectedManufacturer != null)
            {
                try
                {
                    DataService.UpdateManufacturer(SelectedManufacturer);
                    MessageBox.Show("Manufacturer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to update manufacturer: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private bool CanEditOrDelete(object obj)
        {
            return SelectedManufacturer != null;
        }

        private void RefreshManufacturers(object obj)
        {
            Manufacturers.Clear();
            foreach (var manufacturer in DataService.GetAllProducers())
            {
                Manufacturers.Add(manufacturer);
            }
        }

        private void ShowProducts(object obj)
        {
            if (SelectedManufacturer != null)
            {
                var products = DataService.GetProductsByManufacturer(SelectedManufacturer.ProducatorID);
                string productNames = string.Join("\n", products.Select(p => p.NumeProdus));
                MessageBox.Show(productNames, "Products", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
