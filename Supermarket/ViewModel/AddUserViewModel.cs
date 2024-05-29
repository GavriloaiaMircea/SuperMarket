using Supermarket.Helpers;
using Supermarket.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Supermarket.ViewModel
{
    public class AddUserViewModel
    {
        public ICommand AddUserCommand { get; private set; }
        public ObservableCollection<string> UserTypes { get; } = new ObservableCollection<string> { "admin", "casier" };

        public string Username { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }

        public AddUserViewModel()
        {
            AddUserCommand = new RelayCommand(AddUser);
        }

        private void AddUser(object parameter)
        {
            // Verificarea dacă toate câmpurile sunt completate
            if (string.IsNullOrWhiteSpace(Username) ||
                string.IsNullOrWhiteSpace(Password) ||
                string.IsNullOrWhiteSpace(UserType))
            {
                MessageBox.Show("All fields must be filled out.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Crearea noului utilizator
            User newUser = new User
            {
                Username = Username,
                Password = Password,
                UserType = UserType,
                IsDeleted = false
            };

            // Adăugarea utilizatorului în baza de date
            DataService.AddUser(newUser);

            // Închiderea ferestrei
            OnRequestClose();
        }

        public event Action OnRequestClose; 
    }
}

