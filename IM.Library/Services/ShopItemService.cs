using IM.Library.Models;

namespace IM.Library.Services
{
    public interface IShopItemService
    {
        ShopItem GetItemById(int id);
        IEnumerable<ShopItem> GetAllItems();
        void AddItem(ShopItem item);
        void UpdateItem(ShopItem item);
        void DeleteItem(int id);
    }
    public class ShopItemService : IShopItemService
    {
        private readonly List<ShopItem> _items = new List<ShopItem>();
        public ShopItem GetItemById(int id)
        {
            return _items.FirstOrDefault(item => item.Id == id);
        }

        public IEnumerable<ShopItem> GetAllItems()
        {
            return _items;
        }

        public void AddItem(ShopItem item)
        {
            _items.Add(item);
        }

        public void UpdateItem(ShopItem item)
        {
            var existingItem = GetItemById(item.Id);
            if (existingItem != null)
            {
                existingItem.Name = item.Name;
                existingItem.Desc = item.Desc;
                existingItem.Price = item.Price;
                existingItem.Amount = item.Amount;
            }
        }

    public void DeleteItem(int id)
    {
        var item = GetItemById(id);
        if (item != null)
        {
            _items.Remove(item);
        }
    }
    }
}