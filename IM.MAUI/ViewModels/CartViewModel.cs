using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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

        private string _notificationMessage;
        public string NotificationMessage
        {
            get => _notificationMessage;
            set
            {
                _notificationMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _isNotificationVisible;
        public bool IsNotificationVisible
        {
            get => _isNotificationVisible;
            set
            {
                _isNotificationVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand CheckoutCommand { get; }
        public ICommand NavigateToShopCommand { get; }

        private ShoppingCartItem _selectedCartItem;
        public ShoppingCartItem SelectedCartItem
        {
            get => _selectedCartItem;
            set
            {
                _selectedCartItem = value;
                OnPropertyChanged();
            }
        }

        public CartViewModel()
        {
            _shoppingCartProxy = ShoppingCartProxy.Instance;
            CartItems = new ObservableCollection<ShoppingCartItem>(_shoppingCartProxy.GetCart().Items);

            CheckoutCommand = new Command(Checkout);
            NavigateToShopCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ShopPage)));
        }

        private async void Checkout()
        {
            var cart = _shoppingCartProxy.GetCart();
            if (!cart.Items.Any())
            {
                return;
            }

            _shoppingCartProxy.ClearCart();
            UpdateCartItems();
            await ShowNotification("Checkout successful");
        }

        private void UpdateCartItems()
        {
            CartItems.Clear();
            foreach (var item in _shoppingCartProxy.GetCart().Items)
            {
                CartItems.Add(item);
            }
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(TotalPrice));
        }

        private async Task ShowNotification(string message)
        {
            NotificationMessage = message;
            IsNotificationVisible = true;
            OnPropertyChanged(nameof(NotificationMessage));
            await Task.Delay(3000); 
            IsNotificationVisible = false;
            NotificationMessage = string.Empty;
            OnPropertyChanged(nameof(NotificationMessage));
            OnPropertyChanged(nameof(IsNotificationVisible));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
