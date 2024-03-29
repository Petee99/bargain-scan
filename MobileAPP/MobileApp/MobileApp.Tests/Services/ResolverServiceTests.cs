// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResolverServiceTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Services
{
    #region Imports

    using MobileApp.Services;
    using MobileApp.Views;

    using Moq;

    #endregion

    [TestClass]
    public class ResolverServiceTests
    {
        #region Constants and Private Fields

        private readonly Mock<IServiceProvider> _serviceProviderMock = new();

        #endregion

        #region Public Methods and Operators

        [TestMethod]
        public void Resolve_RegisteredType_ReturnsInstance()
        {
            // Arrange
            var service = new ResolverService(_serviceProviderMock.Object);
            var mainPageView = new MainPageView();
            _serviceProviderMock.Setup(x => x.GetService(typeof(MainPageView))).Returns(mainPageView);

            // Act
            var instance = service.Resolve<MainPageView>();

            // Assert
            Assert.AreEqual(mainPageView, instance);
        }

        [TestMethod]
        public void Resolve_UnregisteredType_ReturnsNull()
        {
            // Arrange
            var service = new ResolverService(_serviceProviderMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(MainPageView))).Returns(null!);

            // Act
            var instance = service.Resolve<MainPageView>();

            // Assert
            Assert.IsNull(instance);
        }

        #endregion
    }
}