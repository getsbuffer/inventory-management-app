using Microsoft.Maui.Controls;
using IM.MAUI.ViewModels;
using IM.Library.Services;

namespace IM.MAUI.Views
{
    public partial class ShopPage : ContentPage
    {
        public ShopPage()
        {
            InitializeComponent();
            BindingContext = new ShopViewModel(App.Services.GetService<ShopItemService>(), App.Services.GetService<ShoppingCartProxy>());
        }
    }
}
