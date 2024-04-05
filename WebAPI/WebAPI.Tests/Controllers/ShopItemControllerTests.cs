// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItemControllerTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Controllers
{
    #region Imports

    using Microsoft.AspNetCore.Mvc;

    using Moq;

    using WebAPI.Controllers;
    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;

    #endregion

    [TestClass]
    public class ShopItemControllerTests
    {
        #region Public Methods and Operators
        
        [TestMethod]
        public void Post_DataServiceCreateCalled()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<ShopItem>>();
            var controller = new ShopItemController(dataService.Object);
            var item = new ShopItem(string.Empty, null, 0.ToString(), Shop.Aldi);

            // Act
            var result = controller.Post(item).Result;

            // Assert
            dataService.Verify(service => service.Create(item), Times.Once);
        }

        [TestMethod]
        public void UploadDataFromWebsite_WhenCalledWithInvalidData_ReturnsStatusCode500()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<ShopItem>>();
            var controller = new ShopItemController(dataService.Object);
            var items = new List<ShopItem> { new(null, null, null, Shop.Aldi) };

            // Act
            var result = controller.UploadDataFromWebsite(items).Result as StatusCodeResult;

            // Assert
            Assert.AreEqual(500, result?.StatusCode);
        }

        [TestMethod]
        public void UploadDataFromWebsite_WhenCalledWithNullData_ReturnsStatusCode500()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<ShopItem>>();
            var controller = new ShopItemController(dataService.Object);

            // Act
            var result = controller.UploadDataFromWebsite(null).Result as StatusCodeResult;

            // Assert
            Assert.AreEqual(500, result?.StatusCode);
        }

        [TestMethod]
        public void UploadDataFromWebsite_WhenCalledWithValidData_ReturnsOk()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<ShopItem>>();
            var controller = new ShopItemController(dataService.Object);
            var items = new List<ShopItem> { new("name", new ShopItemCategory("name", string.Empty, string.Empty), 10.ToString(), Shop.Aldi) };

            // Act
            var result = controller.UploadDataFromWebsite(items).Result as OkResult;

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion
    }
}