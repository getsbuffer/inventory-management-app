using IM.MAUI.ViewModels;

namespace IM.MAUI.Views
{
    public partial class SubscriptionPage : ContentPage
    {
        public SubscriptionPage()
        {
            InitializeComponent();
            BindingContext = App.Services.GetService<SubscriptionViewModel>();
        }
    }
}
