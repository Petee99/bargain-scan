// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalUserProfileTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Models
{
    #region Imports

    using MobileApp.Models;
    using MobileApp.Properties;

    #endregion

    [TestClass]
    public class LocalUserProfileTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void ActivateShoppingCart_ShoppingCartIsInList_ActivatesCart()
        {
            // Arrange
            var profile = new LocalUserProfile();
            var cart = new ShoppingCart();
            profile.AddShoppingCart(cart);
            bool eventRaised = false;
            profile.ActiveShoppingCartChanged += (sender, args) => eventRaised = true;
            
            // Act
            bool activated = profile.ActivateShoppingCart(cart);

            // Assert
            Assert.IsTrue(activated);
            Assert.IsTrue(eventRaised);
            Assert.AreEqual(cart, profile.ActiveShoppingCart);
        }

        [TestMethod]
        public void ActivateShoppingCart_ShoppingCartIsNotInList_ReturnsFalse()
        {
            // Arrange
            var profile = new LocalUserProfile();
            var cart = new ShoppingCart();

            // Act
            bool activated = profile.ActivateShoppingCart(cart);

            // Assert
            Assert.IsFalse(activated);
        }

        [TestMethod]
        public void AddShoppingCart_CartIsAlreadyInList_ReturnsFalse()
        {
            // Arrange
            var profile = new LocalUserProfile();
            var cart = new ShoppingCart();
            profile.AddShoppingCart(cart);

            // Act
            bool added = profile.AddShoppingCart(cart);

            // Assert
            Assert.IsFalse(added);
        }

        [TestMethod]
        public void AddShoppingCart_CartIsNotInList_AddsCart()
        {
            // Arrange
            var profile = new LocalUserProfile();
            var cart = new ShoppingCart();

            // Act
            bool added = profile.AddShoppingCart(cart);

            // Assert
            Assert.IsTrue(added);
            Assert.IsTrue(profile.ShoppingCarts.Contains(cart));
        }

        [TestMethod]
        public void CreateShoppingCart_CreatesCartWithDefaultName()
        {
            // Arrange
            var profile = new LocalUserProfile();

            // Act
            var cart = profile.CreateShoppingCart();

            // Assert
            Assert.AreEqual($"{Resources.ShoppingCartTitle}_{profile.ShoppingCarts.Count()}", cart.Name);
        }

        [TestMethod]
        public void RemoveShoppingCart_CartIsInList_RemovesCart()
        {
            // Arrange
            var profile = new LocalUserProfile();
            var cart = new ShoppingCart();
            profile.AddShoppingCart(cart);

            // Act
            bool removed = profile.RemoveShoppingCart(cart);

            // Assert
            Assert.IsTrue(removed);
            Assert.IsFalse(profile.ShoppingCarts.Contains(cart));
        }

        [TestMethod]
        public void RemoveShoppingCart_CartIsNotInList_ReturnsFalse()
        {
            // Arrange
            var profile = new LocalUserProfile();
            var cart = new ShoppingCart();

            // Act
            bool removed = profile.RemoveShoppingCart(cart);

            // Assert
            Assert.IsFalse(removed);
        }

        [TestMethod]
        public void ShoppingCartsChanged_EventIsRaisedWhenCartIsAdded()
        {
            // Arrange
            var profile = new LocalUserProfile();
            var cart = new ShoppingCart();
            bool eventRaised = false;
            profile.ShoppingCartsChanged += (sender, args) => eventRaised = true;

            // Act
            profile.AddShoppingCart(cart);

            // Assert
            Assert.IsTrue(eventRaised);
        }

        #endregion
    }
}