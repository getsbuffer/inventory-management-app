using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace IM.Library.DTO
{
    public class ShopItemDTO : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _desc;
        private decimal _price;
        private int _amount;
        private bool _isBogo;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Desc
        {
            get => _desc;
            set
            {
                _desc = value;
                OnPropertyChanged();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
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

        public bool IsBogo
        {
            get => _isBogo;
            set
            {
                _isBogo = value;
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
