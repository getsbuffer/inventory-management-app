using System.Windows.Input;
using IM.MAUI.Views;
using Microsoft.Maui.Controls;

namespace IM.MAUI.ViewModels
{
    public class MainViewModel
    {
        public ICommand NavigateToInventoryManagementCommand { get; }
        public ICommand NavigateToShopCommand { get; }

        public MainViewModel()
        {
            NavigateToInventoryManagementCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(InventoryManagementPage)));
            NavigateToShopCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ShopPage)));
        }
    }
}
