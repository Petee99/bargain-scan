// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppShellViewModelTests.cs" owner="Peter Mako">
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

    using Moq;

    #endregion

    [TestClass]
    public class AppShellViewModelTests
    {
        #region Constants and Private Fields

        private readonly Mock<IDataService> _dataServiceMock = ServiceMockProvider.GetDataServiceMock();

        private readonly Mock<IEventAggregator> _eventAggregatorMock = ServiceMockProvider.GetEventAggregatorMock();

        #endregion

        #region Public Methods and Operators
        
        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var viewModel = new AppShellViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);

            // Assert
            _eventAggregatorMock.Verify(x => x.Subscribe(viewModel), Times.Once);
            _dataServiceMock.Verify(x => x.GetMainCategories(), Times.Once);
        }

        [TestMethod]
        public void Handle_NotSubCategorySelected_FlyOutStaysOpen()
        {
            // Arrange
            var viewModel = new AppShellViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object)
            {
                IsFlyOutOpen = true
            };

            // Act
            viewModel.Handle(new EventMessageBase(new Category(string.Empty), EventType.CategorySelected));

            // Assert
            Assert.IsTrue(viewModel.IsFlyOutOpen);
        }

        [TestMethod]
        public void Handle_SubCategorySelected_FlyOutGetsClosed()
        {
            // Arrange
            var viewModel = new AppShellViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object)
            {
                IsFlyOutOpen = true
            };

            // Act
            viewModel.Handle(new EventMessageBase(new SubCategory(string.Empty, new Category(string.Empty)), EventType.CategorySelected));

            // Assert
            viewModel.PropertyChanged += (_, _) => Assert.IsFalse(viewModel.IsFlyOutOpen);
        }

        [TestMethod]
        public void ToggleCategoryCommand_CategoryViewModelIsNotNull_TogglesCategory()
        {
            // Arrange
            var viewModel = new AppShellViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);
            var category = new CategoryViewModel(new Category(string.Empty), _eventAggregatorMock.Object);

            // Act
            viewModel.ToggleCategoryCommand.Execute(category);

            // Assert
            Assert.IsTrue(category.IsExpanded);
        }

        [TestMethod]
        public void ToggleCategoryCommand_CategoryViewModelIsNull_DoesNotToggleCategory()
        {
            // Arrange
            var viewModel = new AppShellViewModel(_dataServiceMock.Object, _eventAggregatorMock.Object);

            // Act
            viewModel.ToggleCategoryCommand.Execute(null);

            // Assert
            Assert.IsFalse(viewModel.Categories.Any(x => x.IsExpanded));
        }

        #endregion
    }
}