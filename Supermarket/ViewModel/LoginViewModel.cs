using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Supermarket.Helpers;
using Supermarket.Models;
using Supermarket.Views;

namespace Supermarket.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; private set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(obj => Login());
        }

        public Action CloseAction { get; set; }

        public void Login()
        {
            User user = DataService.GetUserByUsernameAndPassword(Username, Password);

            if (user != null && !user.IsDeleted)
            {
                MessageBox.Show($"Login successful as {user.UserType}!", "Login Info", MessageBoxButton.OK, MessageBoxImage.Information);

                // Logica pentru deschiderea ferestrelor pe baza tipului de utilizator
                switch (user.UserType)
                {
                    case "admin":
                        Application.Current.Dispatcher.Invoke(() => {
                            AdminWindow adminWindow = new AdminWindow();
                            adminWindow.Show();
                            CloseAction();
                        });
                        break;
                    case "casier":
                        Application.Current.Dispatcher.Invoke(() => {
                            CasierWindow casierWindow = new CasierWindow(user);
                            casierWindow.Show();
                            CloseAction();
                        });
                        break;
                }
            }
            else
            {
                MessageBox.Show("Incorrect username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
