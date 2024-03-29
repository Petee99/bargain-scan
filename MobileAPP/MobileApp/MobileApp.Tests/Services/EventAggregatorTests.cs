// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventAggregatorTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Services
{
    #region Imports

    using MobileApp.Interfaces;
    using MobileApp.Services;

    #endregion

    [TestClass]
    public class EventAggregatorTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void MultipleSubscribers_AllSubscribersAreIHandle_AllSubscribersAreNotified()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var subscriber1 = new TestSubscriber();
            var subscriber2 = new TestSubscriber();
            eventAggregator.Subscribe(subscriber1);
            eventAggregator.Subscribe(subscriber2);

            // Act
            eventAggregator.Publish(new TestEvent());

            // Assert
            Assert.IsTrue(subscriber1.Handled);
            Assert.IsTrue(subscriber2.Handled);
        }

        [TestMethod]
        public void Subscribe_SubscriberIsIHandle_SubscribesToEvent()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var subscriber = new TestSubscriber();

            // Act
            eventAggregator.Subscribe(subscriber);
            eventAggregator.Publish(new TestEvent());

            // Assert
            Assert.IsTrue(subscriber.Handled);
        }

        [TestMethod]
        public void Unsubscribe_RemovesSubscriber()
        {
            // Arrange
            var eventAggregator = new EventAggregator();
            var subscriber = new TestSubscriber();
            eventAggregator.Subscribe(subscriber);

            // Act
            eventAggregator.UnSubscribe(subscriber);
            eventAggregator.Publish(new TestEvent());

            // Assert
            Assert.IsFalse(subscriber.Handled);
        }

        #endregion

        private class TestEvent
        {
        }

        private class TestSubscriber : IHandle<TestEvent>
        {
            #region Public Properties

            public bool Handled { get; private set; }

            #endregion

            #region Public Methods and Operators

            public void Handle(TestEvent message)
            {
                Handled = true;
            }

            #endregion
        }
    }
}