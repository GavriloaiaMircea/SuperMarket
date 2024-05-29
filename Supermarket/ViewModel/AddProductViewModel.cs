using Supermarket.Helpers;
using Supermarket.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    public class AddProductViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Categorie> Categories { get; set; }
        public ObservableCollection<Producator> Producers { get; set; }

        private Categorie _selectedCategory;
        public Categorie SelectedCategory
        {
            get => _selectedCategory;
            set { _selectedCategory = value; OnPropertyChanged(); }
        }

        private Producator _selectedProducer;
        public Producator SelectedProducer
        {
            get => _selectedProducer;
            set { _selectedProducer = value; OnPropertyChanged(); }
        }

        private string _productName;
        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(); }
        }

        private string _barcode;
        public string Barcode
        {
            get => _barcode;
            set { _barcode = value; OnPropertyChanged(); }
        }

        public ICommand AddProductCommand { get; }

        public AddProductViewModel()
        {
            Categories = new ObservableCollection<Categorie>(DataService.GetAllCategories());
            Producers = new ObservableCollection<Producator>(DataService.GetAllProducers());
            AddProductCommand = new RelayCommand(_ => AddProduct());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddProduct()
        {
            if (string.IsNullOrWhiteSpace(ProductName) || string.IsNullOrWhiteSpace(Barcode) ||
                SelectedCategory == null || SelectedProducer == null)
            {
                MessageBox.Show("Please fill in all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Creare obiect produs
            Produs newProduct = new Produs
            {
                NumeProdus = ProductName,
                CodeDeBare=Barcode,
                CategorieID = SelectedCategory.CategorieID,
                ProducatorID = SelectedProducer.ProducatorID
            };

            // Apelare funcție adăugare produs în baza de date
            try
            {
                DataService.AddProduct(newProduct);
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Resetare câmpuri pentru a permite introducerea unui nou produs
                ProductName = string.Empty;
                Barcode = string.Empty;
                SelectedCategory = null;
                SelectedProducer = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add product: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
