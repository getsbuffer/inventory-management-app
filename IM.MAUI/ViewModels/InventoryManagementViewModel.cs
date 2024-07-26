using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using IM.Library.Models;
using IM.Library.Services;

namespace IM.MAUI.ViewModels
{
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        private readonly ShopItemService _shopItemService;

        public ObservableCollection<ShopItem> ShopItems { get; set; }

        public ICommand CreateItemCommand { get; }
        public ICommand ReadItemsCommand { get; }
        public ICommand UpdateItemCommand { get; }
        public ICommand DeleteItemCommand { get; }
        public ICommand NavigateBackCommand { get; }

        public InventoryManagementViewModel()
        {
            _shopItemService = new ShopItemService();
            ShopItems = new ObservableCollection<ShopItem>(_shopItemService.GetAllItems());

            CreateItemCommand = new Command(CreateItem);
            ReadItemsCommand = new Command(ReadItems);
            UpdateItemCommand = new Command(UpdateItem);
            DeleteItemCommand = new Command(DeleteItem);
            NavigateBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private void CreateItem()
        {
            // Implementation for creating an item
        }

        private void ReadItems()
        {
            ShopItems.Clear();
            foreach (var item in _shopItemService.GetAllItems())
            {
                ShopItems.Add(item);
            }
        }

        private void UpdateItem()
        {
            // Implementation for updating an item
        }

        private void DeleteItem()
        {
            // Implementation for deleting an item
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
