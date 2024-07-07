namespace IM.Library.Models
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public ShopItem? Item { get; set; }
        public int Amount { get; set; }
        public decimal TotalPrice => Item.Price * Amount;
    }
}