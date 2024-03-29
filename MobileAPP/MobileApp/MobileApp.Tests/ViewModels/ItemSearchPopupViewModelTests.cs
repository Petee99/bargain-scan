// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSearchPopupViewModelTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.ViewModels
{
    #region Imports

    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Tests.Mocks;
    using MobileApp.ViewModels;

    using Moq;

    #endregion

    [TestClass]
    public class ItemSearchPopupViewModelTests
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

            var viewModel = new ItemSearchPopupViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var item = new ShopItem();
            viewModel.SelectedItem = item;

            // Act
            viewModel.AddItemToActiveShoppingCarCommand.Execute(item);

            // Assert
            activeShoppingCartMock.Verify(x => x.AddItem(item), Times.Once);
        }

        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var viewModel = new ItemSearchPopupViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);

            // Assert
            Assert.IsNotNull(viewModel.StartSearchCommand);
            Assert.IsNotNull(viewModel.AddItemToActiveShoppingCarCommand);
            Assert.IsNotNull(viewModel.SearchResults);
            Assert.IsNotNull(viewModel.SelectItemCommand);
        }

        [TestMethod]
        public void SelectItemCommand_ItemIsSelected_SelectedItemIsSet()
        {
            // Arrange
            var viewModel = new ItemSearchPopupViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var item = new ShopItem();
            bool selectedItemChanged = false;
            viewModel.PropertyChanged += (_, args) =>
            {
                if (args.PropertyName == nameof(viewModel.SelectedItem))
                {
                    selectedItemChanged = true;
                }
            };

            // Act
            viewModel.SelectItemCommand.Execute(item);

            // Assert
            Assert.IsTrue(selectedItemChanged);
            Assert.AreEqual(item, viewModel.SelectedItem);
        }

        [TestMethod]
        public void StartSearchCommand_SearchTermDoesNotMatchItem_SearchResultsAreEmpty()
        {
            // Arrange
            var viewModel = new ItemSearchPopupViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object)
            {
                SearchInput = "Test"
            };

            _dataServiceMock.Setup(x => x.GetShopItemsBySearchTerm(viewModel.SearchInput))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(Array.Empty<IShopItem>()));

            // Act
            viewModel.StartSearchCommand.Execute(null);

            // Assert
            Assert.IsFalse(viewModel.SearchResults.Any());
        }

        [TestMethod]
        public void StartSearchCommand_SearchTermMatchesItem_OnlyFirstTenSearchResultsArePresented()
        {
            // Arrange
            var viewModel = new ItemSearchPopupViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object)
            {
                SearchInput = "Test"
            };

            var itemsList = new List<IShopItem>();

            for (int i = 0; i <= 11; i++)
            {
                itemsList.Add(new ShopItem());
            }

            var completion = new TaskCompletionSource();

            _dataServiceMock.Setup(x => x.GetShopItemsBySearchTerm(viewModel.SearchInput))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(itemsList));

            // Act
            viewModel.StartSearchCommand.Execute(null);
            viewModel.SearchResults.CollectionChanged += (_, _) =>
            {
                if (viewModel.SearchResults.Count == 10)
                {
                    completion.SetResult();
                }
            };

            // Assert
            completion.Task.Wait();
            Assert.IsTrue(viewModel.SearchResults.Contains(itemsList[9]));
            Assert.IsFalse(viewModel.SearchResults.Contains(itemsList.Last()));
        }

        [TestMethod]
        public void StartSearchCommand_SearchTermMatchesItem_SearchResultsArePopulated()
        {
            // Arrange
            var viewModel = new ItemSearchPopupViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object)
            {
                SearchInput = "Test"
            };

            var completion = new TaskCompletionSource();

            _dataServiceMock.Setup(x => x.GetShopItemsBySearchTerm(viewModel.SearchInput))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(new List<IShopItem> { new ShopItem() }));

            // Act
            viewModel.StartSearchCommand.Execute(null);
            viewModel.SearchResults.CollectionChanged += (_, _) =>
            {
                if (viewModel.SearchResults.Any())
                {
                    completion.SetResult();
                }
            };

            // Assert
            completion.Task.Wait();
            Assert.IsTrue(viewModel.SearchResults.Any());
        }

        #endregion
    }
}