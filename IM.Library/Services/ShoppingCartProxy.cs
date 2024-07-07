using IM.Library.Models;

namespace IM.Library.Services
{
    public class ShoppingCartProxy
    {
        private static ShoppingCartProxy? _instance;
        private static readonly object _lock = new object();
        private ShoppingCart _cart;

        private ShoppingCartProxy()
        {
            _cart = new ShoppingCart { Id = 1 }; 
        }

        public static ShoppingCartProxy Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ShoppingCartProxy();
                    }
                    return _instance;
                }
            }
        }

        public ShoppingCart GetCart()
        {
            return _cart;
        }

        public void AddItemToCart(ShopItem item, int amount)
        {
            var cartItem = _cart.Items.FirstOrDefault(i => i.Item?.Id == item.Id);
            if (cartItem == null)
            {
                cartItem = new ShoppingCartItem { Id = _cart.Items.Count + 1, Item = item, Amount = amount };
                _cart.Items.Add(cartItem);
            }
            else
            {
                cartItem.Amount += amount;
            }
        }

        public void RemoveItemFromCart(int itemId)
        {
            var cartItem = _cart.Items.FirstOrDefault(i => i.Item?.Id == itemId);
            if (cartItem != null)
            {
                _cart.Items.Remove(cartItem);
            }
        }

        public void UpdateItemQuantity(int itemId, int amount)
        {
            var cartItem = _cart.Items.FirstOrDefault(i => i.Item?.Id == itemId);
            if (cartItem != null)
            {
                cartItem.Amount = amount;
            }
        }

        public void ClearCart()
        {
            _cart.Items.Clear();
        }
    }
}