// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartViewModelTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.ViewModels
{
    #region Imports

    using MobileApp.Models;
    using MobileApp.ViewModels;

    #endregion

    [TestClass]
    public class ShoppingCartViewModelTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void Constructor_InitializesItems()
        {
            // Arrange
            var cart = new ShoppingCart();
            var vm = new ShoppingCartViewModel(cart);

            // Act
            cart.AddItem(new ShopItem());

            // Assert
            Assert.IsNotNull(vm.Items);
            Assert.IsNotNull(vm.RemoveItemCommand);
            Assert.AreEqual(1, vm.Items.Count);
        }

        [TestMethod]
        public void Dispose_UnSubscribesFromCartEvents()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();
            var vm = new ShoppingCartViewModel(cart);
            cart.AddItem(item);

            // Act
            vm.Dispose();
            cart.AddItem(new ShopItem());

            // Assert
            Assert.AreEqual(1, vm.Items.Count);
        }


        [TestMethod]
        public void ItemAdded_TotalIncreases()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem
            {
                Price = 10.ToString()
            };
            var vm = new ShoppingCartViewModel(cart);

            // Act
            cart.AddItem(item);

            // Assert
            Assert.AreEqual(10, vm.Total);
        }

        [TestMethod]
        public void ItemRemoved_TotalDecreases()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem
            {
                Price = 10.ToString()
            };
            var vm = new ShoppingCartViewModel(cart);
            cart.AddItem(item);

            // Act
            cart.RemoveItem(item);

            // Assert
            Assert.AreEqual(0, vm.Total);
        }

        [TestMethod]
        public void RemoveItemCommand_ItemIsInCart_RemovesItem()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();
            var vm = new ShoppingCartViewModel(cart);
            cart.AddItem(item);

            // Act
            vm.RemoveItemCommand.Execute(item);

            // Assert
            Assert.AreEqual(0, vm.Items.Count);
        }

        [TestMethod]
        public void RemoveItemCommand_ItemIsNotInCart_DoesNothing()
        {
            // Arrange
            var cart = new ShoppingCart();
            var item = new ShopItem();
            var vm = new ShoppingCartViewModel(cart);
            cart.AddItem(new ShopItem());

            // Act
            vm.RemoveItemCommand.Execute(item);

            // Assert
            Assert.AreEqual(1, vm.Items.Count);
        }

        [TestMethod]
        public void UpdateDescription_RaisesPropertyChanged()
        {
            // Arrange
            var cart = new ShoppingCart();
            var vm = new ShoppingCartViewModel(cart);
            string newDescription = "newDescription";
            bool propertyChanged = false;
            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(vm.Description))
                {
                    propertyChanged = true;
                }
            };

            // Act
            vm.Description = newDescription;

            // Assert
            Assert.IsTrue(propertyChanged);
        }

        [TestMethod]
        public void UpdateName_RaisesPropertyChanged()
        {
            // Arrange
            var cart = new ShoppingCart();
            var vm = new ShoppingCartViewModel(cart);
            string newName = "newName";
            bool propertyChanged = false;
            vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(vm.Name))
                {
                    propertyChanged = true;
                }
            };

            // Act
            vm.Name = newName;

            // Assert
            Assert.IsTrue(propertyChanged);
        }

        #endregion
    }
}