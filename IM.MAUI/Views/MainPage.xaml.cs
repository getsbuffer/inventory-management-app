using IM.MAUI.ViewModels;
using IM.Library.Services;
namespace IM.MAUI.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainViewModel(App.Services.GetService<ShopItemService>(), App.Services.GetService<ShoppingCartProxy>());
        }
    }
}
