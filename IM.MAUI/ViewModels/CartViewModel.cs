using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IM.Library.Models;
using IM.Library.Services;
using IM.MAUI.Views;

namespace IM.MAUI.ViewModels
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private readonly ShopItemService _shopItemService;
        private readonly ShoppingCartProxy _shoppingCartProxy;

        public ObservableCollection<ShoppingCartItem> CartItems { get; set; }
        public decimal Subtotal => _shoppingCartProxy.GetCart().TotalPrice;
        public decimal Total => CalculateTotal();

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
        public ICommand SaveWishlistCommand { get; }
        public ICommand LoadWishlistCommand { get; }

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

        public CartViewModel(ShopItemService shopItemService, ShoppingCartProxy shoppingCartProxy)
        {
            _shopItemService = shopItemService;
            _shoppingCartProxy = shoppingCartProxy;
            CartItems = new ObservableCollection<ShoppingCartItem>(_shoppingCartProxy.GetCart().Items);

            CheckoutCommand = new Command(Checkout);
            NavigateToShopCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ShopPage)));
            SaveWishlistCommand = new Command(SaveWishlist);
            LoadWishlistCommand = new Command(async () => await LoadWishlist());
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

        private async void SaveWishlist()
        {
            var cart = _shoppingCartProxy.GetCart();
            if (!cart.Items.Any())
            {
                await ShowNotification("Cart is empty. Cannot save as wishlist.");
                return;
            }

            int wishlistId = _shoppingCartProxy.SaveWishlist(cart);
            _shoppingCartProxy.ClearCart();
            UpdateCartItems();
            await ShowNotification($"Wishlist saved with ID: {wishlistId}");
        }

        private async Task LoadWishlist()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Load Wishlist", "Enter the ID of the wishlist to load:");

            if (int.TryParse(result, out int wishlistId))
            {
                var wishlist = _shoppingCartProxy.GetWishlistById(wishlistId);
                if (wishlist != null)
                {
                    ReturnItemsToShop();

                    _shoppingCartProxy.SetCurrentCart(wishlist);
                    _shoppingCartProxy.RemoveWishlistById(wishlistId);
                    UpdateCartItems();
                    await ShowNotification($"Wishlist {wishlistId} loaded.");
                }
                else
                {
                    await ShowNotification("Wishlist not found.");
                }
            }
            else
            {
                await ShowNotification("Invalid wishlist ID entered.");
            }
        }

        private void ReturnItemsToShop()
        {
            var cart = _shoppingCartProxy.GetCart();
            foreach (var item in cart.Items)
            {
                var shopItem = _shopItemService.GetItemById(item.Item.Id);
                if (shopItem != null)
                {
                    shopItem.Amount += item.Amount;
                    _shopItemService.UpdateItem(shopItem);
                }
                else
                {
                    _shopItemService.AddItem(new ShopItem
                    {
                        Id = item.Item.Id,
                        Name = item.Item.Name,
                        Desc = item.Item.Desc,
                        Price = item.Item.Price,
                        Amount = item.Amount,
                        IsBogo = item.Item.IsBogo
                    });
                }
            }
        }

        private decimal CalculateTotal()
        {
            var cart = _shoppingCartProxy.GetCart();
            decimal total = 0;
            foreach (var item in cart.Items)
            {
                int quantityToCharge = item.Item.IsBogo ? (item.Amount / 2) + (item.Amount % 2) : item.Amount;
                total += item.Item.Price * quantityToCharge;
            }
            total += total * _shoppingCartProxy.TaxRate;
            return total;
        }

        private void UpdateCartItems()
        {
            CartItems.Clear();
            foreach (var item in _shoppingCartProxy.GetCart().Items)
            {
                CartItems.Add(item);
            }
            OnPropertyChanged(nameof(CartItems));
            OnPropertyChanged(nameof(Total));
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
