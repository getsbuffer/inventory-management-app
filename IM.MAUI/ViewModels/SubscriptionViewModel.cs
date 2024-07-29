using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using IM.Library.Models;
using IM.Library.Services;
using IM.MAUI.Views;

namespace IM.MAUI.ViewModels
{
    public class SubscriptionViewModel : INotifyPropertyChanged
    {
        private readonly SubscriptionService _subscriptionService;
        private readonly ShopItemService _shopItemService;

        public ObservableCollection<Subscription> Subscriptions { get; set; }
        private Subscription _selectedSubscription;

        public Subscription SelectedSubscription
        {
            get => _selectedSubscription;
            set
            {
                _selectedSubscription = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddSubscriptionCommand { get; }
        public ICommand DeleteSubscriptionCommand { get; }
        public ICommand NavigateToMainMenuCommand { get; }

        public SubscriptionViewModel(SubscriptionService subscriptionService, ShopItemService shopItemService)
        {
            _subscriptionService = subscriptionService;
            _shopItemService = shopItemService;

            Subscriptions = new ObservableCollection<Subscription>(
                _subscriptionService.GetAllSubscriptions().Select(sub =>
                {
                    var shopItem = _shopItemService.GetItemById(sub.ShopItemId);
                    sub.ShopItemName = shopItem?.Name ?? "Unknown";
                    return sub;
                })
            );

            AddSubscriptionCommand = new Command(AddSubscription);
            DeleteSubscriptionCommand = new Command(DeleteSubscription);
            NavigateToMainMenuCommand = new Command(async () => await Shell.Current.GoToAsync(nameof(MainPage)));
        }

        private async void AddSubscription()
        {
            string shopItemIdString = await Application.Current.MainPage.DisplayPromptAsync("Add Subscription", "Enter ShopItem ID:");
            if (!int.TryParse(shopItemIdString, out int shopItemId))
                return;

            string amountString = await Application.Current.MainPage.DisplayPromptAsync("Add Subscription", "Enter Amount:");
            if (!int.TryParse(amountString, out int amount))
                return;

            string[] frequencies = { "Daily", "Weekly", "Biweekly", "Monthly", "Yearly" };
            string frequency = await Application.Current.MainPage.DisplayActionSheet("Select Frequency", "Cancel", null, frequencies);

            var shopItem = _shopItemService.GetItemById(shopItemId);
            if (shopItem != null)
            {
                var subscription = new Subscription
                {
                    Id = Subscriptions.Count + 1,
                    ShopItemId = shopItemId,
                    Amount = amount,
                    Frequency = frequency,
                    MonthlyCost = CalculateMonthlyCost(shopItem.Price * 0.95m, amount, frequency),
                    ShopItemName = shopItem.Name
                };
                _subscriptionService.AddSubscription(subscription);
                Subscriptions.Add(subscription);

                OnPropertyChanged(nameof(Subscriptions));
            }
        }

        private async void DeleteSubscription()
        {
            if (SelectedSubscription == null)
            {
                await Application.Current.MainPage.DisplayAlert("No Subscription Selected", "Please select a subscription to delete.", "OK");
                return;
            }

            var subscription = SelectedSubscription;

            _subscriptionService.DeleteSubscription(subscription.Id);
            Subscriptions.Remove(subscription);

            OnPropertyChanged(nameof(Subscriptions));
        }

        private decimal CalculateMonthlyCost(decimal price, int amount, string frequency)
        {
            decimal monthlyCost = price * amount;
            switch (frequency)
            {
                case "Daily":
                    monthlyCost *= 30;
                    break;
                case "Weekly":
                    monthlyCost *= 4;
                    break;
                case "Biweekly":
                    monthlyCost *= 2;
                    break;
                case "Yearly":
                    monthlyCost /= 12;
                    break;
            }
            return monthlyCost;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
