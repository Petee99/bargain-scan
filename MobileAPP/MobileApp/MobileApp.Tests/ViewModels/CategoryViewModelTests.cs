// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryViewModelTests.cs" owner="Peter Mako">
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
    public class CategoryViewModelTests
    {
        #region Constants and Private Fields

        private readonly Mock<IEventAggregator> _eventAggregatorMock = ServiceMockProvider.GetEventAggregatorMock();

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void Constructor_InitializesProperties()
        {
            // Arrange
            var category = new Category("Test");
            category.GetOrCreateSubCategory("TestSub");
            var viewModel = new CategoryViewModel(category, _eventAggregatorMock.Object);

            // Assert
            Assert.IsFalse(viewModel.IsExpanded);
            Assert.IsNotNull(viewModel.SelectCommand);
            Assert.AreEqual(category.Title, viewModel.Title);
            Assert.AreEqual(category.SubCategories.Count(), viewModel.SubCategories.Count());
            Assert.AreEqual(category.SubCategories.First().Title, viewModel.SubCategories.First().Title);
            Assert.AreEqual(category.IconPath, viewModel.IconPath);
        }

        [TestMethod]
        public void IsExpandedChanged_RaisesEvent()
        {
            // Arrange
            var category = new Category("Test");
            var viewModel = new CategoryViewModel(category, _eventAggregatorMock.Object);
            bool eventRaised = false;
            viewModel.IsExpandedChanged += (_, _) => eventRaised = true;

            // Act
            viewModel.IsExpanded = true;

            // Assert
            Assert.IsTrue(eventRaised);
        }

        [TestMethod]
        public void SelectCommand_InvokesCategorySelectedMessage()
        {
            // Arrange
            var category = new Mock<ICategory>();
            var viewModel = new CategoryViewModel(category.Object, _eventAggregatorMock.Object);

            // Act
            viewModel.SelectCommand.Execute(null);

            // Assert
            _eventAggregatorMock.Verify(x =>
                x.Publish(It.Is<EventMessageBase>(z => z.EventType == EventType.CategorySelected)), Times.Once);
        }

        #endregion
    }
}