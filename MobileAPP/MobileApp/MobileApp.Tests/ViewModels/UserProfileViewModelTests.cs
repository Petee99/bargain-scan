// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileViewModelTests.cs" owner="Peter Mako">
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
    using MobileApp.Views;

    using Moq;

    #endregion

    [TestClass]
    public class UserProfileViewModelTests
    {
        #region Constants and Private Fields

        private readonly LocalUserProfile _userProfile = new();

        private readonly Mock<IDataService> _dataServiceMock = ServiceMockProvider.GetDataServiceMock();

        private readonly Mock<IEventAggregator> _eventAggregatorMock = ServiceMockProvider.GetEventAggregatorMock();

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void TestInitialize()
        {
            _dataServiceMock.Setup(x => x.UserProfile).Returns(_userProfile);
        }

        [TestMethod]
        public void AddShoppingCartCommand_AddsNewShoppingCart()
        {
            // Arrange
            var viewModel = new UserProfileViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);

            // Act
            viewModel.AddShoppingCartCommand.Execute(null);

            // Assert
            Assert.AreEqual(2, _userProfile.ShoppingCarts.Count());
        }

        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var viewModel = new UserProfileViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);

            // Assert
            Assert.IsNotNull(viewModel.ActiveShoppingCart);
            Assert.IsNotNull(viewModel.AddShoppingCartCommand);
            Assert.IsNotNull(viewModel.SubstituteItemCommand);
        }

        [TestMethod]
        public void SubstituteItemCommand_SearchPopupGetsCreated()
        {
            // Arrange
            var viewModel = new UserProfileViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);

            // Act
            viewModel.SubstituteItemCommand.Execute(null);

            // Assert
            _eventAggregatorMock.Verify(x => x.Subscribe(It.IsAny<ItemSearchPopupView>()), Times.Once);
        }

        [TestMethod]
        public void UpdateName_PropertyChangedEventRaised()
        {
            // Arrange
            var viewModel = new UserProfileViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            bool propertyChanged = false;
            viewModel.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(viewModel.Name))
                {
                    propertyChanged = true;
                }
            };

            // Act
            viewModel.Name = "Test";

            // Assert
            Assert.IsTrue(propertyChanged);
        }

        [TestMethod]
        public void UserProfileActiveShoppingCartChanged_ViewModelActiveShoppingCartChanges()
        {
            // Arrange
            var viewModel = new UserProfileViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var newShoppingCart = _userProfile.CreateShoppingCart();

            // Act
            _userProfile.ActivateShoppingCart(newShoppingCart);

            // Assert
            Assert.AreEqual(newShoppingCart.Name, viewModel.ActiveShoppingCart.Name);
        }

        [TestMethod]
        public void UserProfileShoppingCartAdded_ViewModelGetsNotified()
        {
            // Arrange
            var viewModel = new UserProfileViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            bool eventRaised = false;
            viewModel.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(viewModel.ShoppingCarts))
                {
                    eventRaised = true;
                }
            };

            // Act
            _userProfile.CreateShoppingCart();

            // Assert
            Assert.IsTrue(eventRaised);
        }

        #endregion
    }
}