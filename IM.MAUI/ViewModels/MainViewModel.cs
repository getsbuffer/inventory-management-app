using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using IM.Library.Models;
using IM.Library.Services;

namespace IM.MAUI.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ShopItemService _shopItemService;

        public ObservableCollection<ShopItem> ShopItems { get; set; }

        public MainViewModel()
        {
            _shopItemService = new ShopItemService();
            ShopItems = new ObservableCollection<ShopItem>(_shopItemService.GetAllItems());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
