using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Supermarket.Models
{
    public class BonProdus : INotifyPropertyChanged
    {
        private decimal cantitate;
        private decimal subtotal;

        public string NumeProdus { get; set; }
        public decimal Cantitate
        {
            get { return cantitate; }
            set
            {
                cantitate = value;
                OnPropertyChanged();
                Subtotal = Cantitate * PretVanzare;
            }
        }
        public decimal PretVanzare { get; set; }
        public decimal Subtotal
        {
            get { return subtotal; }
            set
            {
                subtotal = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
