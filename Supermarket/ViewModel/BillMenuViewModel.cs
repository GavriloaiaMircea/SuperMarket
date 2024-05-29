using Supermarket.Helpers;
using Supermarket.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows;

namespace Supermarket.ViewModel
{
    public class BillMenuViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Bon> Bonuri { get; set; }
        private Bon _selectedBon;
        public Bon SelectedBon
        {
            get { return _selectedBon; }
            set { _selectedBon = value; OnPropertyChanged(); }
        }

        private DateTime? _selectedDate;
        public DateTime? SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsDateSelected));
            }
        }

        public bool IsDateSelected => SelectedDate.HasValue;

        public ICommand ShowBiggestBillCommand { get; }

        public BillMenuViewModel()
        {
            Bonuri = new ObservableCollection<Bon>(DataService.GetBonuri());
            ShowBiggestBillCommand = new RelayCommand(ShowBiggestBill, param => IsDateSelected);
        }

        private void ShowBiggestBill(object obj)
        {
            if (SelectedDate.HasValue)
            {
                Bon biggestBon = DataService.GetBiggestBonOfDay(SelectedDate.Value);
                if (biggestBon != null)
                {
                    MessageBox.Show($"Bon ID: {biggestBon.BonID}\nData Eliberarii: {biggestBon.DataEliberarii}\nCasier: {biggestBon.Casier}\nTotal: {biggestBon.Total}",
                                    "Biggest Bill", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("No bills found for the selected date.", "Biggest Bill", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}