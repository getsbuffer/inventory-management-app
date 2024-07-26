using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using IM.Library.Models;
using IM.Library.Services;
using IM.MAUI.Views;

namespace IM.MAUI.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private readonly ShoppingCartProxy _shoppingCartProxy;

        public ObservableCollection<ShoppingCartItem> CartItems { get; set; }
        public decimal TotalPrice => _shoppingCartProxy.GetCart().TotalPrice;

        public ICommand CheckoutCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public CartViewModel()
        {
            _shoppingCartProxy = ShoppingCartProxy.Instance;
            CartItems = new ObservableCollection<ShoppingCartItem>(_shoppingCartProxy.GetCart().Items);

            CheckoutCommand = new Command(Checkout);
            NavigateBackCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ShopPage)));
        }

        private void Checkout()
        {
            // Implementation for checking out
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
