using IM.Library.DTO;
using IM.Library.Models;

namespace IM.Library.Helpers
{
    public static class MappingHelper
    {
        public static ShopItemDTO ToDTO(this ShopItem shopItem)
        {
            return new ShopItemDTO
            {
                Id = shopItem.Id,
                Name = shopItem.Name,
                Desc = shopItem.Desc,
                Price = shopItem.Price,
                Amount = shopItem.Amount,
                IsBogo = shopItem.IsBogo
            };
        }

        public static ShopItem ToModel(this ShopItemDTO shopItemDTO)
        {
            return new ShopItem
            {
                Id = shopItemDTO.Id,
                Name = shopItemDTO.Name,
                Desc = shopItemDTO.Desc,
                Price = shopItemDTO.Price,
                Amount = shopItemDTO.Amount,
                IsBogo = shopItemDTO.IsBogo
            };
        }
    }
}
