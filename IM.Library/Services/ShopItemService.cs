using IM.Library.Models;
using IM.Library.DTO;
using IM.Library.Helpers;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace IM.Library.Services
{
    public interface IShopItemService
    {
        ShopItemDTO GetItemById(int id);
        IEnumerable<ShopItemDTO> GetAllItems();
        void AddItem(ShopItemDTO item);
        void UpdateItem(ShopItemDTO item);
        void DeleteItem(int id);
        void ImportItemsFromCsv(string filePath);
    }

    public class ShopItemService : IShopItemService
    {
        private readonly List<ShopItem> _items = new List<ShopItem>();

        public ShopItemDTO GetItemById(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            return item?.ToDTO();
        }

        public IEnumerable<ShopItemDTO> GetAllItems()
        {
            return _items.Select(x => x.ToDTO());
        }

        public void AddItem(ShopItemDTO shopItemDTO)
        {
            var item = shopItemDTO.ToModel();
            _items.Add(item);
        }

        public void UpdateItem(ShopItemDTO shopItemDTO)
        {
            var existingItem = _items.FirstOrDefault(x => x.Id == shopItemDTO.Id);
            if (existingItem != null)
            {
                existingItem.Name = shopItemDTO.Name;
                existingItem.Desc = shopItemDTO.Desc;
                existingItem.Price = shopItemDTO.Price;
                existingItem.Amount = shopItemDTO.Amount;
                existingItem.IsBogo = shopItemDTO.IsBogo;
            }
        }

        public void DeleteItem(int id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _items.Remove(item);
            }
        }

        public void ImportItemsFromCsv(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<ShopItemDTO>();
                foreach (var item in records)
                {
                    AddItem(item);
                }
            }
        }
    }
}
