using Supermarket.Helpers;
using Supermarket.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;
using Supermarket.Views;

namespace Supermarket.ViewModel
{
    public class CategoriesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Categorie> _categories;
        private Categorie _selectedCategory;

        public ICommand OpenAddCategoryCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand CalculateTotalCommand { get; private set; }

        public ObservableCollection<Categorie> Categories
        {
            get => _categories;
            set
            {
                _categories = value;
                OnPropertyChanged();
            }
        }

        public Categorie SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                OnPropertyChanged();
            }
        }

        public CategoriesViewModel()
        {
            Categories = new ObservableCollection<Categorie>(DataService.GetAllCategories());
            OpenAddCategoryCommand = new RelayCommand(OpenAddCategory);
            RefreshCommand = new RelayCommand(RefreshCategories);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            CalculateTotalCommand = new RelayCommand(CalculateTotal, CanCalculateTotal);
        }

        private void OpenAddCategory(object obj)
        {
            AddCategoryWindow addCategoryWindow = new AddCategoryWindow();
            addCategoryWindow.Show();
        }

        private void RefreshCategories(object obj)
        {
            Categories = new ObservableCollection<Categorie>(DataService.GetAllCategories());
        }

        private bool CanSaveChanges(object obj)
        {
            return SelectedCategory != null && !string.IsNullOrWhiteSpace(SelectedCategory.NumeCategorie);
        }

        private void SaveChanges(object obj)
        {
            if (SelectedCategory != null)
            {
                DataService.UpdateCategory(SelectedCategory);
                RefreshCategories(null);
            }
        }

        private bool CanCalculateTotal(object obj)
        {
            return SelectedCategory != null;
        }

        private void CalculateTotal(object obj)
        {
            if (SelectedCategory != null)
            {
                decimal totalValue = DataService.CalculateTotalValueByCategory(SelectedCategory.CategorieID);
                MessageBox.Show($"Total value of products in {SelectedCategory.NumeCategorie}: {totalValue:C}", "Total Value", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
