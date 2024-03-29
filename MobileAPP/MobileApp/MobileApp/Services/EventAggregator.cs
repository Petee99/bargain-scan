// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventAggregator.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Services
{
    #region Imports

    using MobileApp.Interfaces;

    #endregion

    public class EventAggregator : IEventAggregator
    {
        #region Constants and Private Fields

        private readonly Dictionary<Type, List<WeakReference>> _subscriptions = new();

        #endregion

        #region Public Methods and Operators

        public void Publish<T>(T messageToPublish)
        {
            var eventType = typeof(T);
            if (!_subscriptions.ContainsKey(eventType))
            {
                return;
            }

            var toNotify = _subscriptions[eventType].Where(sub => sub.IsAlive).ToList();
            foreach (var weakSub in toNotify)
            {
                if (weakSub.Target is IHandle<T> subscriber)
                {
                    subscriber.Handle(messageToPublish);
                }
            }
        }

        public void Subscribe(object subscriber)
        {
            var subscriberTypes = subscriber.GetType().GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IHandle<>));

            foreach (var subType in subscriberTypes)
            {
                var eventType = subType.GetGenericArguments()[0];
                if (!_subscriptions.ContainsKey(eventType))
                {
                    _subscriptions[eventType] = new List<WeakReference>();
                }

                if (_subscriptions[eventType].All(x => x.Target != subscriber))
                {
                    _subscriptions[eventType].Add(new WeakReference(subscriber));
                }
            }
        }

        public void UnSubscribe(object subscriber)
        {
            foreach (var subs in _subscriptions.Values)
            {
                var found = subs.FirstOrDefault(x => x.Target == subscriber);
                if (found != null)
                {
                    subs.Remove(found);
                }
            }
        }

        #endregion
    }
}