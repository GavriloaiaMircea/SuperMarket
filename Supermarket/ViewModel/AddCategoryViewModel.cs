using Supermarket.Helpers;
using Supermarket.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Supermarket.ViewModel
{
    public class AddCategoryViewModel : INotifyPropertyChanged
    {
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set { _categoryName = value; OnPropertyChanged(); }
        }

        public RelayCommand AddCategoryCommand { get; set; }

        public AddCategoryViewModel()
        {
            AddCategoryCommand = new RelayCommand(AddCategory);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddCategory(object parameter)
        {
            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Categorie newCategory = new Categorie
            {
                NumeCategorie = CategoryName
            };

            try
            {
                DataService.AddCategory(newCategory);
                MessageBox.Show("Category added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                CategoryName = string.Empty; // Clear the field after successful addition
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add category: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
