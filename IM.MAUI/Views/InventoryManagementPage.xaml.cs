using IM.MAUI.ViewModels;
using IM.Library.Services;

namespace IM.MAUI.Views
{
    public partial class InventoryManagementPage : ContentPage
    {
        public InventoryManagementPage()
        {
            InitializeComponent();
            BindingContext = new InventoryManagementViewModel(App.Services.GetService<ShopItemService>());
        }
    }
}
