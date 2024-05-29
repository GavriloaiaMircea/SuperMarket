using Supermarket.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Supermarket.Models;
using Supermarket.Views;
using System.Windows;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Supermarket.ViewModel
{
    public class UserManagementViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<string> UserTypes { get; } = new ObservableCollection<string> { "admin", "casier" };

        private User _selectedUser;
        public User SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsUserAndDateSelected)); }
        }

        private DateTime? _selectedMonth;
        public DateTime? SelectedMonth
        {
            get => _selectedMonth;
            set { _selectedMonth = value; OnPropertyChanged(); OnPropertyChanged(nameof(IsUserAndDateSelected)); }
        }

        public bool IsUserAndDateSelected => SelectedUser != null && SelectedMonth != null;

        public ObservableCollection<UserIncome> UserIncomes { get; set; } = new ObservableCollection<UserIncome>();

        public ICommand AddUserCommand { get; private set; }
        public ICommand EditUserCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand SaveChangesCommand { get; private set; }
        public ICommand ShowIncomesCommand { get; private set; }

        public UserManagementViewModel()
        {
            Users = new ObservableCollection<User>(DataService.GetAllUsers());
            AddUserCommand = new RelayCommand(AddUser);
            SaveChangesCommand = new RelayCommand(SaveChanges);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            ShowIncomesCommand = new RelayCommand(ShowIncomes);
        }

        private void AddUser(object obj)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.Closed += (sender, args) => ReloadUsers();
            addUserWindow.Show();
        }

        private void ReloadUsers()
        {
            Users.Clear();
            var users = DataService.GetAllUsers();
            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        private void SaveChanges(object parameter)
        {
            foreach (var user in Users)
            {
                if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password) ||
                    (user.UserType != "admin" && user.UserType != "casier"))
                {
                    MessageBox.Show("Please fill all fields correctly. User type must be either 'admin' or 'casier'.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DataService.UpdateUser(user);
            }
            MessageBox.Show("Changes saved successfully.");
            ReloadUsers();
        }

        private void DeleteUser(object obj)
        {
            if (SelectedUser != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete user {SelectedUser.Username}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    DataService.DeleteUser(SelectedUser.ID);
                    ReloadUsers();
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ShowIncomes(object obj)
        {
            if (SelectedUser == null || SelectedMonth == null)
                return;

            var incomes = DataService.GetUserIncomesByMonth(SelectedUser.ID, SelectedMonth.Value.Year, SelectedMonth.Value.Month);
            UserIncomes.Clear();
            foreach (var income in incomes)
            {
                UserIncomes.Add(income);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class UserIncome
    {
        public DateTime Date { get; set; }
        public decimal Income { get; set; }
    }
}
