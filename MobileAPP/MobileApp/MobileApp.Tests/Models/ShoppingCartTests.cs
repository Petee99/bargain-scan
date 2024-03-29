// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Models
{
    #region Imports

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Models;

    #endregion

    [TestClass]
    public class ShoppingCartTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void AddItem_ItemIsAddedToTheCart()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();

            // Act
            cart.AddItem(item);

            // Assert
            Assert.IsTrue(cart.Items.Contains(item));
        }

        [TestMethod]
        public void AddItem_ItemIsAlreadyInCart_ReturnsFalse()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();
            cart.AddItem(item);

            // Act
            bool added = cart.AddItem(item);

            // Assert
            Assert.IsFalse(added);
        }

        [TestMethod]
        public void ItemsChanged_EventIsRaisedWhenItemIsAdded()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();
            ItemsChangedEventArgs? eventArgs = null;
            cart.ItemsChanged += (sender, args) => eventArgs = args;

            // Act
            cart.AddItem(item);

            // Assert
            Assert.IsNotNull(eventArgs);
            Assert.AreEqual(EventType.ItemAdded, eventArgs.EventType);
            Assert.AreEqual(item, eventArgs.ChangedItem);
        }

        [TestMethod]
        public void RemoveItem_ItemIsNotInCart_ReturnsFalse()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();

            // Act
            bool removed = cart.RemoveItem(item);

            // Assert
            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void RemoveItem_ItemIsRemovedFromTheCart()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();
            cart.AddItem(item);

            // Act
            cart.RemoveItem(item);

            // Assert
            Assert.IsFalse(cart.Items.Contains(item));
        }

        [TestMethod]
        public void Total_CalculatesTheSumOfTheItems()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item1 = new ShopItem { Price = 100.ToString() };
            var item2 = new ShopItem { Price = 200.ToString() };
            cart.AddItem(item1);
            cart.AddItem(item2);

            // Act
            double total = cart.Total;

            // Assert
            Assert.AreEqual(300, total);
        }

        [TestMethod]
        public void Total_NoItemsInCart_ReturnsZero()
        {
            // Arrange
            var cart = new ShoppingCart();

            // Act
            double total = cart.Total;

            // Assert
            Assert.AreEqual(0, total);
        }

        #endregion
    }
}