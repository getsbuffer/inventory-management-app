using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IM.Library.Models;
using IM.Library.Services;
using IM.MAUI.Views;


namespace IM.MAUI.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly ShopItemService _shopItemService;
        private readonly ShoppingCartProxy _shoppingCartProxy;

        public ObservableCollection<ShopItem> ShopItems { get; set; }
        public ObservableCollection<ShoppingCartItem> CartItems { get; set; }

        private ShopItem _selectedShopItem;
        public ShopItem SelectedShopItem
        {
            get => _selectedShopItem;
            set
            {
                _selectedShopItem = value;
                OnPropertyChanged();
            }
        }

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

        public ICommand AddItemToCartCommand { get; }
        public ICommand RemoveItemFromCartCommand { get; }
        public ICommand ViewCartCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand NavigateToMainMenuCommand { get; }

        public ShopViewModel(ShopItemService shopItemService, ShoppingCartProxy shoppingCartProxy)
        {
            _shopItemService = shopItemService;
            _shoppingCartProxy = shoppingCartProxy;
            ShopItems = new ObservableCollection<ShopItem>(_shopItemService.GetAllItems());
            CartItems = new ObservableCollection<ShoppingCartItem>(_shoppingCartProxy.GetCart().Items);

            AddItemToCartCommand = new Command(AddItemToCart);
            RemoveItemFromCartCommand = new Command(RemoveItemFromCart);
            ViewCartCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(CartPage)));
            CheckoutCommand = new Command(Checkout);
            NavigateToMainMenuCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(MainPage)));
        }


        private async void AddItemToCart()
        {
            if (SelectedShopItem != null)
            {
                int amount = 1; 
                var item = _shopItemService.GetItemById(SelectedShopItem.Id);

                if (item == null)
                {
                    return;
                }

                if (item.Amount < amount)
                {
                    await ShowNotification("Insufficient stock.");
                    return;
                }

                _shoppingCartProxy.AddItemToCart(item, amount);
                item.Amount -= amount; 
                _shopItemService.UpdateItem(item); 
                UpdateCartItems();
                await ShowNotification("Item added to cart");
            }
        }

        private async void RemoveItemFromCart()
        {
            if (SelectedShopItem != null)
            {
                int amount = 1; 
                var item = _shopItemService.GetItemById(SelectedShopItem.Id);

                if (item == null)
                {
                    return;
                }

                var cartItem = _shoppingCartProxy.GetCart().Items.FirstOrDefault(i => i.Item.Id == SelectedShopItem.Id);
                if (cartItem == null || cartItem.Amount < amount)
                {
                    await ShowNotification("Item not in cart or insufficient quantity.");
                    return;
                }


                _shoppingCartProxy.RemoveItemFromCart(SelectedShopItem.Id);
                item.Amount += amount; 
                _shopItemService.UpdateItem(item); 
                UpdateCartItems();
                await ShowNotification("Item removed from cart");
            }
        }

        private void Checkout()
        {
            var cart = _shoppingCartProxy.GetCart();
            if (!cart.Items.Any())
            {
                return;
            }

            decimal subtotal = cart.TotalPrice;
            decimal tax = subtotal * 0.07m;
            decimal total = subtotal + tax;

            _shoppingCartProxy.ClearCart();
            UpdateCartItems();
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

        public decimal TotalPrice => _shoppingCartProxy.GetCart().TotalPrice;

        private async Task ShowNotification(string message)
        {
            NotificationMessage = message;
            IsNotificationVisible = true;
            OnPropertyChanged(nameof(NotificationMessage));
            await Task.Delay(2000); 
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
