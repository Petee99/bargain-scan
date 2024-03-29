// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItemTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Models
{
    #region Imports

    using MobileApp.Enums;
    using MobileApp.Models;

    #endregion

    [TestClass]
    public class ShopItemTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void ShopIconPath_EqualsShopInLowerCase_EndingWithDotPng()
        {
            // Arrange
            var item = new ShopItem
            {
                Shop = Shop.Metro
            };

            // Assert
            Assert.AreEqual($"{Shop.Metro.ToString().ToLower()}.png", item.ShopIconPath);
        }

        [TestMethod]
        public void TryUpdateBarcode_ItemAlreadyHasABarcode_ReturnsFalse()
        {
            // Arrange
            var item = new ShopItem();
            item.TryUpdateBarCode("1111");

            // Act
            bool updated = item.TryUpdateBarCode("2222");

            // Assert
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void TryUpdateBarcode_ItemHasNoBarcode_ReturnsTrue()
        {
            // Arrange
            var item = new ShopItem();

            // Act
            bool updated = item.TryUpdateBarCode("2222");

            // Assert
            Assert.IsTrue(updated);
        }

        #endregion
    }
}