using IM.Library.Models;
using IM.Library.DTO;
using IM.Library.Helpers;

namespace IM.Library.Services
{
    public class ShoppingCartProxy
    {
        private ShoppingCart _cart;
        private readonly List<ShoppingCart> _wishLists;
        private int _nextWishlistId;
        private readonly ShopItemService _shopItemService;

        public decimal TaxRate { get; set; }

        public ShoppingCartProxy(ShopItemService shopItemService)
        {
            _cart = new ShoppingCart { Id = 1 };
            _wishLists = new List<ShoppingCart>();
            _nextWishlistId = 1;
            TaxRate = 0;
            _shopItemService = shopItemService;
        }

        public ShoppingCart GetCart()
        {
            return _cart;
        }

        public void SetCurrentCart(ShoppingCart cart)
        {
            _cart = cart;
        }

        public int SaveWishlist(ShoppingCart cart)
        {
            var newWishlist = new ShoppingCart
            {
                Id = _nextWishlistId++,
                Items = cart.Items.Select(item => new ShoppingCartItem
                {
                    Id = item.Id,
                    Item = item.Item,
                    Amount = item.Amount
                }).ToList()
            };
            _wishLists.Add(newWishlist);
            return newWishlist.Id;
        }

        public ShoppingCart GetWishlistById(int id)
        {
            return _wishLists.FirstOrDefault(wl => wl.Id == id);
        }

        public void RemoveWishlistById(int id)
        {
            var wishlist = _wishLists.FirstOrDefault(wl => wl.Id == id);
            if (wishlist != null)
            {
                _wishLists.Remove(wishlist);
            }
        }

        public void AddItemToCart(ShopItemDTO itemDto, int amount)
        {
            var item = MappingHelper.ToModel(itemDto);
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
                if (cartItem.Amount > 1)
                {
                    cartItem.Amount--;
                }
                else
                {
                    _cart.Items.Remove(cartItem);
                }
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

        public void ReturnItemsToShop()
        {
            foreach (var item in _cart.Items)
            {
                var shopItemDto = _shopItemService.GetItemById(item.Item.Id);
                if (shopItemDto != null)
                {
                    var shopItem = MappingHelper.ToModel(shopItemDto);
                    shopItem.Amount += item.Amount;
                    _shopItemService.UpdateItem(MappingHelper.ToDTO(shopItem));
                }
                else
                {
                    var newItemDto = MappingHelper.ToDTO(new ShopItem
                    {
                        Id = item.Item.Id,
                        Name = item.Item.Name,
                        Desc = item.Item.Desc,
                        Price = item.Item.Price,
                        Amount = item.Amount,
                        IsBogo = item.Item.IsBogo
                    });
                    _shopItemService.AddItem(newItemDto);
                }
            }
        }
    }
}
