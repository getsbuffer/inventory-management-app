using IM.MAUI.ViewModels;
namespace IM.MAUI.Views
{
    public partial class CartPage : ContentPage
    {
        public CartPage()
        {
            InitializeComponent();
            BindingContext = App.Services.GetService<CartViewModel>();
        }
    }
}
