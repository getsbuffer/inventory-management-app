using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using IM.Library.Models;
using IM.Library.Services;

namespace IM.MAUI.ViewModels
{
    public class ShopViewModel : INotifyPropertyChanged
    {
        private readonly ShopItemService _shopItemService;
        private readonly ShoppingCartProxy _shoppingCartProxy;

        public ObservableCollection<ShopItem> ShopItems { get; set; }

        public ICommand AddItemToCartCommand { get; }
        public ICommand RemoveItemFromCartCommand { get; }
        public ICommand ViewCartCommand { get; }
        public ICommand CheckoutCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public ShopViewModel()
        {
            _shopItemService = new ShopItemService();
            _shoppingCartProxy = ShoppingCartProxy.Instance;
            ShopItems = new ObservableCollection<ShopItem>(_shopItemService.GetAllItems());

            AddItemToCartCommand = new Command(AddItemToCart);
            RemoveItemFromCartCommand = new Command(RemoveItemFromCart);
            ViewCartCommand = new Command(ViewCart);
            CheckoutCommand = new Command(Checkout);
            NavigateBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private void AddItemToCart()
        {
            // Implementation for adding an item to the cart
        }

        private void RemoveItemFromCart()
        {
            // Implementation for removing an item from the cart
        }

        private void ViewCart()
        {
            // Implementation for viewing the cart
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
