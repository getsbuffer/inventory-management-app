using System.Windows.Input;
using IM.MAUI.Views;

namespace IM.MAUI.ViewModels
{
    public class MainViewModel
    {
        public ICommand NavigateToInventoryManagementCommand { get; }
        public ICommand NavigateToShopCommand { get; }
        public ICommand NavigateToSubscriptionsCommand { get; }

        public MainViewModel()
        {
            NavigateToInventoryManagementCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(InventoryManagementPage)));
            NavigateToShopCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ShopPage)));
            NavigateToSubscriptionsCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(SubscriptionPage)));
        }
    }
}
