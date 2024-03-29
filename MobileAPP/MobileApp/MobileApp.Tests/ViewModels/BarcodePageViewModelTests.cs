// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodePageViewModelTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.ViewModels
{
    #region Imports

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Tests.Mocks;
    using MobileApp.ViewModels;
    using MobileApp.Views;

    using Moq;

    #endregion

    [TestClass]
    public class BarcodePageViewModelTests
    {
        #region Constants and Private Fields

        private readonly Mock<IDataService> _dataServiceMock = ServiceMockProvider.GetDataServiceMock();

        private readonly Mock<IEventAggregator> _eventAggregatorMock = ServiceMockProvider.GetEventAggregatorMock();

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

            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var item = new ShopItem();
            viewModel.Items.Add(item);

            // Act
            viewModel.AddItemToActiveShoppingCarCommand.Execute(item);

            // Assert
            activeShoppingCartMock.Verify(x => x.AddItem(item), Times.Once);
        }

        [TestMethod]
        public void AssignBarcode_ItemSelectedForAssigningBarcodeAndBarcodeIsNotSet_SearchPopupDoesNotGetCreated()
        {
            // Arrange
            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var item = new ShopItem();
            item.TryUpdateBarCode("1111");
            _dataServiceMock.Setup(x => x.GetShopItemsByBarcode("1111"))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(new[] { item }));

            // Act
            viewModel.AssignBarcodeCommand.Execute(null);

            // Assert
            _eventAggregatorMock.Verify(x => x.Subscribe(It.IsAny<ItemSearchPopupView>()), Times.Never);
        }

        [TestMethod]
        public void AssignBarcode_ItemSelectedForAssigningBarcodeAndBarcodeIsSet_SearchPopupGetsCreated()
        {
            // Arrange
            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var item = new ShopItem();
            item.TryUpdateBarCode("1111");
            _dataServiceMock.Setup(x => x.GetShopItemsByBarcode("1111"))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(new[] { item }));
            viewModel.Handle(new BarcodeMessage(null, EventType.BarcodeRead, "1111"));

            // Act
            viewModel.AssignBarcodeCommand.Execute(null);

            // Assert
            _eventAggregatorMock.Verify(x => x.Subscribe(It.IsAny<ItemSearchPopupView>()), Times.Once);
        }

        [TestMethod]
        public void BarcodePageViewModel_Ctor_InitializesCommands()
        {
            // Arrange
            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);

            // Assert
            _eventAggregatorMock.Verify(x => x.Subscribe(viewModel), Times.Once);
            Assert.IsNotNull(viewModel.AddItemToActiveShoppingCarCommand);
            Assert.IsNotNull(viewModel.AssignBarcodeCommand);
        }

        [TestMethod]
        public void Handle_EventTypeIsBarcodeRead_IsAssignBarcodePossibleChanges()
        {
            // Arrange
            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            bool isAssignBarcodeChanged = false;
            viewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(viewModel.IsAssignBarcodePossible))
                {
                    isAssignBarcodeChanged = true;
                }
            };

            // Act
            viewModel.Handle(new BarcodeMessage(null, EventType.BarcodeRead, "1111"));

            // Assert
            Assert.IsTrue(isAssignBarcodeChanged);
        }

        [TestMethod]
        public void Handle_EventTypeIsNotBarcodeRead_IsAssignBarcodePossibleDoesNotChange()
        {
            // Arrange
            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            bool isAssignBarcodeChanged = false;
            viewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(viewModel.IsAssignBarcodePossible))
                {
                    isAssignBarcodeChanged = true;
                }
            };

            // Act
            viewModel.Handle(new BarcodeMessage(null, EventType.PopupCloseInitiated, "1111"));

            // Assert
            Assert.IsFalse(isAssignBarcodeChanged);
            _dataServiceMock.Verify(x => x.GetShopItemsByBarcode(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void Handle_ItemDoesNotExistForBarcode_ItemsCollectionDoesNotIncludeItem()
        {
            // Arrange
            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var item = new ShopItem();
            item.TryUpdateBarCode("1111");

            // Act
            viewModel.Handle(new BarcodeMessage(null, EventType.BarcodeRead, "1111"));

            // Assert
            Assert.AreEqual(0, viewModel.Items.Count);
            Assert.IsTrue(viewModel.IsAssignBarcodePossible);
        }

        [TestMethod]
        public void Handle_ItemExistsForBarcode_ItemsCollectionIncludesItem()
        {
            // Arrange
            var viewModel = new BarcodePageViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var item = new ShopItem();
            item.TryUpdateBarCode("1111");
            _dataServiceMock.Setup(x => x.GetShopItemsByBarcode("1111"))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(new[] { item }));

            // Act
            viewModel.Handle(new BarcodeMessage(null, EventType.BarcodeRead, "1111"));

            // Assert
            Assert.AreEqual(1, viewModel.Items.Count);
            Assert.AreEqual(item, viewModel.Items[0]);
            Assert.IsFalse(viewModel.IsAssignBarcodePossible);
        }

        #endregion
    }
}