using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IM.Library.Models;
using IM.Library.Services;
using IM.MAUI.Views;

namespace IM.MAUI.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        private readonly ShopItemService _shopItemService;

        public ObservableCollection<ShopItem> ShopItems { get; set; }

        private ShopItem _selectedShopItem;
        public ShopItem SelectedShopItem
        {
            get => _selectedShopItem;
            set
            {
                _selectedShopItem = value;
                OnPropertyChanged();
            }
        }

        public ICommand CreateItemCommand { get; }
        public ICommand ReadItemsCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand NavigateToMainMenuCommand { get; }
        public ICommand ShowMenuCommand { get; }
        public ICommand ImportCsvCommand { get; }

        public InventoryManagementViewModel(ShopItemService shopItemService)
        {
            _shopItemService = shopItemService;
            ShopItems = new ObservableCollection<ShopItem>(_shopItemService.GetAllItems());

            CreateItemCommand = new Command(CreateItem);
            ReadItemsCommand = new Command(ReadItems);
            UpdateItemCommand = new Command(UpdateItem);
            DeleteItemCommand = new Command(DeleteItem);
            NavigateToMainMenuCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(MainPage)));
            ShowMenuCommand = new Command(async () => await ShowMenu());
            ImportCsvCommand = new Command(async () => await ImportCsv());
        }
        private async Task ShowMenu()
        {
            var action = await Application.Current.MainPage.DisplayActionSheet("Options", "Cancel", null, "Import CSV");

            if (action == "Import CSV")
            {
                await ImportCsv();
            }
        }

        private async Task ImportCsv()
        {
            string filePath = await PickCsvFileAsync();
            if (filePath != null)
            {
                _shopItemService.ImportItemsFromCsv(filePath);
                ReadItems();
            }
        }

        public async Task<string> PickCsvFileAsync()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Pick a CSV file",
                    FileTypes = new Microsoft.Maui.Storage.FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.WinUI, new[] { ".csv" } }
                    })
                });

                if (result == null)
                    return null;

                return result.FullPath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async void CreateItem()
        {
            string name = await Application.Current.MainPage.DisplayPromptAsync("Create Item", "Enter item name:");
            if (name == null) return;

            string description = await Application.Current.MainPage.DisplayPromptAsync("Create Item", "Enter item description:");
            if (description == null) return;

            string priceString = await Application.Current.MainPage.DisplayPromptAsync("Create Item", "Enter item price:");
            if (priceString == null) return;

            string amountString = await Application.Current.MainPage.DisplayPromptAsync("Create Item", "Enter item amount:");
            if (amountString == null) return;

            if (decimal.TryParse(priceString, out decimal price) && int.TryParse(amountString, out int amount))
            {
                var item = new ShopItem
                {
                    Id = _shopItemService.GetAllItems().Count() + 1,
                    Name = name,
                    Desc = description,
                    Price = price,
                    Amount = amount
                };

                _shopItemService.AddItem(item);
                ReadItems();  
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Input", "Please enter valid price and amount.", "OK");
            }
        }

        private void ReadItems()
        {
            ShopItems.Clear();
            foreach (var item in _shopItemService.GetAllItems())
            {
                ShopItems.Add(item);
            }
            OnPropertyChanged(nameof(ShopItems));  
        }

        private async void UpdateItem()
        {
            if (SelectedShopItem == null)
            {
                await Application.Current.MainPage.DisplayAlert("No Item Selected", "Please select an item to update.", "OK");
                return;
            }

            string name = await Application.Current.MainPage.DisplayPromptAsync("Update Item", "Enter new item name (leave blank to keep current):", initialValue: SelectedShopItem.Name);
            string description = await Application.Current.MainPage.DisplayPromptAsync("Update Item", "Enter new item description (leave blank to keep current):", initialValue: SelectedShopItem.Desc);
            string priceString = await Application.Current.MainPage.DisplayPromptAsync("Update Item", "Enter new item price (leave blank to keep current):", initialValue: SelectedShopItem.Price.ToString());
            string amountString = await Application.Current.MainPage.DisplayPromptAsync("Update Item", "Enter new item amount (leave blank to keep current):", initialValue: SelectedShopItem.Amount.ToString());

            if (!string.IsNullOrWhiteSpace(name)) SelectedShopItem.Name = name;
            if (!string.IsNullOrWhiteSpace(description)) SelectedShopItem.Desc = description;
            if (decimal.TryParse(priceString, out decimal price)) SelectedShopItem.Price = price;
            if (int.TryParse(amountString, out int amount)) SelectedShopItem.Amount = amount;

            _shopItemService.UpdateItem(SelectedShopItem);
            ReadItems();  
        }

        private async void DeleteItem()
        {
            if (SelectedShopItem == null)
            {
                await Application.Current.MainPage.DisplayAlert("No Item Selected", "Please select an item to delete.", "OK");
                return;
            }

            bool confirm = await Application.Current.MainPage.DisplayAlert("Delete Item", $"Are you sure you want to delete {SelectedShopItem.Name}?", "Yes", "No");
            if (confirm)
            {
                _shopItemService.DeleteItem(SelectedShopItem.Id);
                ReadItems();  
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
