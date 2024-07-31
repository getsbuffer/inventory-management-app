using IM.Library.DTO;

namespace IM.API.EC
{
    public class InventoryEC
    {
        private static List<ShopItemDTO> _inventory = new List<ShopItemDTO>();

        public IEnumerable<ShopItemDTO> GetAllItems()
        {
            return _inventory;
        }

        public ShopItemDTO GetItemById(int id)
        {
            return _inventory.FirstOrDefault(item => item.Id == id);
        }

        public ShopItemDTO AddItem(ShopItemDTO newItem)
        {
            newItem.Id = _inventory.Any() ? _inventory.Max(item => item.Id) + 1 : 1;
            _inventory.Add(newItem);
            return newItem;
        }

        public bool UpdateItem(int id, ShopItemDTO updatedItem)
        {
            var existingItem = _inventory.FirstOrDefault(item => item.Id == id);
            if (existingItem == null)
            {
                return false;
            }

            existingItem.Name = updatedItem.Name;
            existingItem.Desc = updatedItem.Desc;
            existingItem.Price = updatedItem.Price;
            existingItem.Amount = updatedItem.Amount;

            return true;
        }

        public bool DeleteItem(int id)
        {
            var item = _inventory.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return false;
            }

            _inventory.Remove(item);
            return true;
        }
    }
}