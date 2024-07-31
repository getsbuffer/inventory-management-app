using System.Windows.Input;
using IM.Library.Services;
using IM.MAUI.Views;
using System.Threading.Tasks;

namespace IM.MAUI.ViewModels
{
    public class MainViewModel
    {
        private readonly ShopItemService _shopItemService;
        private readonly ShoppingCartProxy _shoppingCartProxy;
        public ICommand NavigateToInventoryManagementCommand { get; }
        public ICommand NavigateToShopCommand { get; }
        public ICommand NavigateToSubscriptionsCommand { get; }
        public ICommand ShowMenuCommand { get; }
        public ICommand ImportCsvCommand { get; }
        public ICommand ConfigureTaxRateCommand { get; }
        public ICommand ConfigureBogoCommand { get; }
        public ICommand MarkDownItemCommand { get; }

        public MainViewModel(ShopItemService shopItemService, ShoppingCartProxy shoppingCartProxy)
        {
            _shopItemService = shopItemService;
            _shoppingCartProxy = shoppingCartProxy;

            NavigateToInventoryManagementCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(InventoryManagementPage)));
            NavigateToShopCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(ShopPage)));
            NavigateToSubscriptionsCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(SubscriptionPage)));
            ShowMenuCommand = new Command(async () => await ShowMenu());
            ImportCsvCommand = new Command(async () => await ImportCsv());
            ConfigureTaxRateCommand = new Command(async () => await ConfigureTaxRate());
            ConfigureBogoCommand = new Command(async () => await ConfigureBogo());
            MarkDownItemCommand = new Command(async () => await MarkDownItem());
        }

        private async Task ShowMenu()
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Options", "Cancel", null, "Import CSV", "Configure Tax Rate", "Configure BOGO", "Mark down an item");

            if (action == "Import CSV")
            {
                await ImportCsv();
            }
            else if (action == "Configure Tax Rate")
            {
                await ConfigureTaxRate();
            }
            else if (action == "Configure BOGO")
            {
                await ConfigureBogo();
            }
            else if (action == "Mark down an item")
            {
                await MarkDownItem();
            }
        }

        private async Task ImportCsv()
        {
            string filePath = await PickCsvFileAsync();
            if (filePath != null)
            {
                await _shopItemService.ImportItemsFromCsvAsync(filePath);
            }
        }

        private async Task ConfigureTaxRate()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Configure Tax Rate", "Enter the sales tax rate (e.g., 0.05 for 5%):", initialValue: _shoppingCartProxy.TaxRate.ToString());

            if (decimal.TryParse(result, out decimal taxRate))
            {
                _shoppingCartProxy.TaxRate = taxRate;
            }
        }

        private async Task ConfigureBogo()
        {
            string result = await Application.Current.MainPage.DisplayPromptAsync("Configure BOGO", "Enter the ID of the item to make BOGO:");

            if (int.TryParse(result, out int itemId))
            {
                var item = await _shopItemService.GetItemByIdAsync(itemId);
                if (item != null)
                {
                    item.IsBogo = true;
                    await _shopItemService.AddOrUpdateItemAsync(item);
                }
            }
        }

        private async Task MarkDownItem()
        {
            string idResult = await Application.Current.MainPage.DisplayPromptAsync("Mark Down Item", "Enter the ID of the item to mark down:");
            if (!int.TryParse(idResult, out int itemId))
            {
                return;
            }

            string percentResult = await Application.Current.MainPage.DisplayPromptAsync("Mark Down Item", "Enter the percentage to mark down (e.g., 0.10 for 10%):");
            if (!decimal.TryParse(percentResult, out decimal markdownPercentage))
            {
                return;
            }

            var item = await _shopItemService.GetItemByIdAsync(itemId);
            if (item != null)
            {
                item.Price -= item.Price * markdownPercentage;
                await _shopItemService.AddOrUpdateItemAsync(item);
            }
        }

        public async Task<string> PickCsvFileAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Pick a CSV file",
                    FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".csv" } }
                    })
                });

                return result?.FullPath;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
