using Microsoft.Maui.Controls;
using IM.MAUI.Views;

namespace IM.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(InventoryManagementPage), typeof(Views.InventoryManagementPage));
            Routing.RegisterRoute(nameof(ShopPage), typeof(Views.ShopPage));
            Routing.RegisterRoute(nameof(CartPage), typeof(Views.CartPage));
        }
    }
}