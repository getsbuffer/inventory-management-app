namespace IM.Library.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();
        public decimal TotalPrice => Items.Sum(item => item.TotalPrice);
    }
}