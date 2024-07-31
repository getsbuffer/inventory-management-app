using IM.Library.DTO;
using IM.Library.Utilities;
using Newtonsoft.Json;

public class InventoryEC
{
    private readonly WebRequestHandler _webRequestHandler;

    public InventoryEC()
    {
        _webRequestHandler = new WebRequestHandler();
    }

    public async Task<IEnumerable<ShopItemDTO>> GetAllItemsAsync()
    {
        string result = await _webRequestHandler.Get("/api/inventory");
        return result != null ? JsonConvert.DeserializeObject<IEnumerable<ShopItemDTO>>(result) : new List<ShopItemDTO>();
    }

    public async Task<ShopItemDTO> GetItemByIdAsync(int id)
    {
        string result = await _webRequestHandler.Get($"/api/inventory/{id}");
        return result != null ? JsonConvert.DeserializeObject<ShopItemDTO>(result) : null;
    }

    public async Task<bool> AddItemAsync(ShopItemDTO item)
    {
        string result = await _webRequestHandler.Post("/api/inventory", item);
        return result != null && result != "ERROR";
    }

    public async Task<bool> UpdateItemAsync(ShopItemDTO item)
    {
        string result = await _webRequestHandler.Put($"/api/inventory/{item.Id}", item);
        return result != null && result != "ERROR";
    }

    public async Task<bool> DeleteItemAsync(int id)
    {
        string result = await _webRequestHandler.Delete($"/api/inventory/{id}");
        return result != null && result != "ERROR";
    }
}
