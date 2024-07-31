using CsvHelper.Configuration;
using CsvHelper;
using IM.Library.DTO;
using IM.Library.Utilities;
using Newtonsoft.Json;
using System.Globalization;

namespace IM.Library.Services
{
    public class ShopItemService
    {
        private readonly WebRequestHandler _webRequestHandler;

        public ShopItemService(WebRequestHandler webRequestHandler)
        {
            _webRequestHandler = webRequestHandler;
        }

        public async Task<IEnumerable<ShopItemDTO>> GetAllItemsAsync()
        {
            string response = await _webRequestHandler.Get("/api/inventory");
            return response != null ? JsonConvert.DeserializeObject<IEnumerable<ShopItemDTO>>(response) : new List<ShopItemDTO>();
        }

        public async Task<ShopItemDTO> GetItemByIdAsync(int id)
        {
            string response = await _webRequestHandler.Get($"/api/inventory/{id}");
            return response != null ? JsonConvert.DeserializeObject<ShopItemDTO>(response) : null;
        }
        public async Task AddOrUpdateItemAsync(ShopItemDTO item)
        {
            var existingItem = await GetItemByIdAsync(item.Id);
            if (existingItem != null)
            {
                await _webRequestHandler.Put($"/api/inventory/{item.Id}", item);
            }
            else
            {
                await _webRequestHandler.Post("/api/inventory", item);
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            await _webRequestHandler.Delete($"/api/inventory/{id}");
        }

        public async Task ImportItemsFromCsvAsync(string filePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<ShopItemDTO>();

            foreach (var item in records)
            {
                await AddOrUpdateItemAsync(item);
            }
        }
    }
}
