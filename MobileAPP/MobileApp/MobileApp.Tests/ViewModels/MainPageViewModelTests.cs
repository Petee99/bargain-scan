// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModelTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;

    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Tests.Mocks;
    using MobileApp.ViewModels;

    using Moq;

    #endregion

    [TestClass]
    public class MainPageViewModelTests
    {
        #region Constants and Private Fields

        private readonly Mock<IDataService> _dataServiceMock = ServiceMockProvider.GetDataServiceMock();

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void AddItemToActiveShoppingCarCommand_ItemIsSelected_ItemGetsAddedToActiveShoppingCart()
        {
            // Arrange
            var userProfileMock = new Mock<IUserProfile>();
            var activeShoppingCartMock = new Mock<IShoppingCart>();
            userProfileMock.Setup(x => x.ActiveShoppingCart).Returns(activeShoppingCartMock.Object);
            _dataServiceMock.Setup(x => x.UserProfile).Returns(userProfileMock.Object);

            var viewModel = new MainPageViewModel(_dataServiceMock.Object);
            var item = new ShopItem();

            // Act
            viewModel.AddToShoppingCartCommand.Execute(item);

            // Assert
            activeShoppingCartMock.Verify(x => x.AddItem(item), Times.Once);
        }

        [TestMethod]
        public void UpdateShopItems_WithCollection_TitleIsSet()
        {
            // Arrange
            var viewModel = new MainPageViewModel(_dataServiceMock.Object)
            {
                ShopItems = new ObservableCollection<IShopItem> { new ShopItem { SubCategory = "Test" } }
            };

            // Assert
            Assert.AreEqual("Test", viewModel.Title);
        }

        [TestMethod]
        public void UpdateShopItems_WithEmptyCollection_TitleIsNull()
        {
            // Arrange
            var viewModel = new MainPageViewModel(_dataServiceMock.Object)
            {
                ShopItems = new ObservableCollection<IShopItem>()
            };

            // Assert
            Assert.IsNull(viewModel.Title);
        }

        #endregion
    }
}