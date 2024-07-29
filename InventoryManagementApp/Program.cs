using IM.Library.Services;
using IM.Library.Models;

namespace InventoryManagementApp
{
    internal class Program
    {
        private static IShopItemService _shopItemService = new ShopItemService();
        private static ShoppingCartProxy _shoppingCartProxy = new ShoppingCartProxy();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Dr. Chris Mills' Mechanical Keyboard Shop! Here are your options: ");
            _shopItemService.AddItem(new ShopItem{ Id = 1, Name = "Test 1", Desc = "Test Desc 1", Price = 40.99m, Amount = 100});
            _shopItemService.AddItem(new ShopItem{ Id = 2, Name = "Test 2", Desc = "Test Desc 2", Price = 50.99m, Amount = 60});
            _shopItemService.AddItem(new ShopItem{ Id = 3, Name = "Test 3", Desc = "Test Desc 3", Price = 90.99m, Amount = 50});
            _shopItemService.AddItem(new ShopItem{ Id = 4, Name = "Test 4", Desc = "Test Desc 4", Price = 60.99m, Amount = 80});
            _shopItemService.AddItem(new ShopItem{ Id = 5, Name = "Test 5", Desc = "Test Desc 5", Price = 70.99m, Amount = 55});
            _shopItemService.AddItem(new ShopItem{ Id = 6, Name = "Test 6", Desc = "Test Desc 6", Price = 80.99m, Amount = 12});
            ReadItems();
            while (true)
            {
                Console.WriteLine("1. Inventory Management");
                Console.WriteLine("2. Shop");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageInventory();
                        break;
                    case "2":
                        Shop();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private static void ManageInventory()
        {
            while (true)
            {
                Console.WriteLine("\nInventory Management:");
                Console.WriteLine("1. Create Item");
                Console.WriteLine("2. Read Items");
                Console.WriteLine("3. Update Item");
                Console.WriteLine("4. Delete Item");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateItem();
                        break;
                    case "2":
                        ReadItems();
                        break;
                    case "3":
                        UpdateItem();
                        break;
                    case "4":
                        DeleteItem();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private static void CreateItem()
        {
            Console.Write("Enter item name: ");
            var name = Console.ReadLine();
            Console.Write("Enter item description: ");
            var description = Console.ReadLine();
            Console.Write("Enter item price: ");
            var price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter item amount stocked: ");
            var amount = int.Parse(Console.ReadLine());

            var item = new ShopItem { Id = _shopItemService.GetAllItems().Count() + 1, Name = name, Desc = description, Price = price, Amount = amount };
            _shopItemService.AddItem(item);
            Console.WriteLine("Item created successfully.");
        }

        private static void ReadItems()
        {
            var items = _shopItemService.GetAllItems();
            if (!items.Any())
            {
                Console.WriteLine("No items found.");
                return;
            }

            foreach (var cartItem in items)
            {
                Console.WriteLine($"ID: {cartItem.Id}, Name: {cartItem.Name}, Description: {cartItem.Desc}, Price: {cartItem.Price}, Stock: {cartItem.Amount}");
            }
        }

        private static void UpdateItem()
        {
            Console.Write("Enter item ID to update: ");
            var id = int.Parse(Console.ReadLine());
            var item = _shopItemService.GetItemById(id);

            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            Console.Write("Enter new item name (leave blank to keep current): ");
            var name = Console.ReadLine();
            Console.Write("Enter new item description (leave blank to keep current): ");
            var description = Console.ReadLine();
            Console.Write("Enter new item price (leave blank to keep current): ");
            var priceInput = Console.ReadLine();
            Console.Write("Enter new item stock quantity (leave blank to keep current): ");
            var amountInput = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name)) item.Name = name;
            if (!string.IsNullOrWhiteSpace(description)) item.Desc = description;
            if (decimal.TryParse(priceInput, out var price)) item.Price = price;
            if (int.TryParse(amountInput, out var amount)) item.Amount = amount;

            _shopItemService.UpdateItem(item);
            Console.WriteLine("Item updated successfully.");
        }

        private static void DeleteItem()
        {
            Console.Write("Enter item ID to delete: ");
            var id = int.Parse(Console.ReadLine());
            var item = _shopItemService.GetItemById(id);

            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            _shopItemService.DeleteItem(id);
            Console.WriteLine("Item deleted successfully.");
        }

        private static void Shop()
        {
            while (true)
            {
                Console.WriteLine("\nShop:");
                Console.WriteLine("1. Add Item to Cart");
                Console.WriteLine("2. Remove Item from Cart");
                Console.WriteLine("3. View Cart");
                Console.WriteLine("4. Checkout");
                Console.WriteLine("5. Back to Main Menu");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddItemToCart();
                        break;
                    case "2":
                        RemoveItemFromCart();
                        break;
                    case "3":
                        ViewCart();
                        break;
                    case "4":
                        Checkout();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private static void AddItemToCart()
        {
            Console.Write("Enter item ID to add to cart: ");
            var id = int.Parse(Console.ReadLine());
            Console.Write("Enter quantity: ");
            var amount = int.Parse(Console.ReadLine());

            var item = _shopItemService.GetItemById(id);

            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            if (item.Amount < amount)
            {
                Console.WriteLine("Not enough stock available.");
                return;
            }

            _shoppingCartProxy.AddItemToCart(item, amount);
            Console.WriteLine("Item added to cart.");
        }

        private static void RemoveItemFromCart()
        {
            Console.Write("Enter item ID to remove from cart: ");
            var id = int.Parse(Console.ReadLine());
            _shoppingCartProxy.RemoveItemFromCart(id);
            Console.WriteLine("Item removed from cart.");
        }

        private static void ViewCart()
        {
            var cart = _shoppingCartProxy.GetCart();
            if (!cart.Items.Any())
            {
                Console.WriteLine("Cart is empty.");
                return;
            }

            foreach (var cartItem in cart.Items)
            {
                Console.WriteLine($"ID: {cartItem.Item.Id}, Name: {cartItem.Item.Name}, Quantity: {cartItem.Amount}, Total Price: {cartItem.TotalPrice}");
            }

            Console.WriteLine($"Subtotal: {cart.TotalPrice}");
        }

        private static void Checkout()
        {
            var cart = _shoppingCartProxy.GetCart();
            if (!cart.Items.Any())
            {
                Console.WriteLine("Cart is empty.");
                return;
            }

            decimal subtotal = cart.TotalPrice;
            decimal tax = subtotal * 0.07m;
            decimal total = subtotal + tax;

            Console.WriteLine("\nReceipt:");
            foreach (var cartItem in cart.Items)
            {
                Console.WriteLine($"ID: {cartItem.Item.Id}, Name: {cartItem.Item.Name}, Quantity: {cartItem.Amount}, Total Price: {cartItem.TotalPrice}");
            }
            Console.WriteLine($"Subtotal: {subtotal}");
            Console.WriteLine($"Tax (7%): {tax}");
            Console.WriteLine($"Total: {total}");

            _shoppingCartProxy.ClearCart();
            Console.WriteLine("Checkout complete. Thank you for shopping!");
        }
    }
}
