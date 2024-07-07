namespace IM.Library.Models
{
    public class ShopItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Desc { get; set; }
        public decimal Price { get; set; }
        public int Amount {get; set; }
    }
}