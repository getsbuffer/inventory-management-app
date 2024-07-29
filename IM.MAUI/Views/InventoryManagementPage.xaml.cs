using IM.MAUI.ViewModels;

namespace IM.MAUI.Views
{
    public partial class InventoryManagementPage : ContentPage
    {
        public InventoryManagementPage()
        {
            InitializeComponent();
            BindingContext = App.Services.GetService<InventoryManagementViewModel>();
        }
    }
}
