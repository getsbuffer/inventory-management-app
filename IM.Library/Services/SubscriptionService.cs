using IM.Library.Models;

namespace IM.Library.Services
{
    public class SubscriptionService
    {
        private readonly List<Subscription> _subscriptions = new List<Subscription>();

        public IEnumerable<Subscription> GetAllSubscriptions()
        {
            return _subscriptions;
        }

        public Subscription GetSubscriptionById(int id)
        {
            return _subscriptions.FirstOrDefault(sub => sub.Id == id);
        }

        public void AddSubscription(Subscription subscription)
        {
            _subscriptions.Add(subscription);
        }

        public void DeleteSubscription(int id)
        {
            var subscription = GetSubscriptionById(id);
            if (subscription != null)
            {
                _subscriptions.Remove(subscription);
            }
        }
    }
}
