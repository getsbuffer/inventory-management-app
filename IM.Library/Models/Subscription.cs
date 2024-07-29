using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IM.Library.Models
{
    public class Subscription : INotifyPropertyChanged
    {
        private int _id;
        private int _shopItemId;
        private int _amount;
        private string _frequency;
        private decimal _monthlyCost;
        private string _shopItemName;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public int ShopItemId
        {
            get => _shopItemId;
            set
            {
                _shopItemId = value;
                OnPropertyChanged();
            }
        }

        public int Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged();
            }
        }

        public string Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                OnPropertyChanged();
            }
        }

        public decimal MonthlyCost
        {
            get => _monthlyCost;
            set
            {
                _monthlyCost = value;
                OnPropertyChanged();
            }
        }

        public string ShopItemName
        {
            get => _shopItemName;
            set
            {
                _shopItemName = value;
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
