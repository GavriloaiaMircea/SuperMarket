using Supermarket.Helpers;
using Supermarket.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    public class AddManufacturesViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _country;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddManufacturerCommand { get; }

        public AddManufacturesViewModel()
        {
            AddManufacturerCommand = new RelayCommand(AddManufacturer);
        }

        private void AddManufacturer(object parameter)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Country))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Producator newManufacturer = new Producator
            {
                NumeProducator = Name,
                TaraOrigine = Country
            };

            try
            {
                DataService.AddManufacturer(newManufacturer);
                MessageBox.Show("Manufacturer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                // Clear fields after adding
                Name = string.Empty;
                Country = string.Empty;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add manufacturer: {ex.Message}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
