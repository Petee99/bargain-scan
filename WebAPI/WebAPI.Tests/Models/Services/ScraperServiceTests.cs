// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScraperServiceTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Models.Services
{
    #region Imports

    using Moq;

    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;
    using WebAPI.Services;

    #endregion

    [TestClass]
    public class ScraperServiceTests
    {
        #region Constants and Private Fields

        private readonly Mock<IDataBaseService<ShopItem>> _mockDataBaseService = new();
        private readonly Mock<IDataBaseService<ShopItemCategory>> _mockDataBaseServiceCategory = new();

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void TestInitialize()
        {
            _mockDataBaseServiceCategory.Setup(x => x.GetAll()).ReturnsAsync(new List<ShopItemCategory>());
        }

        [TestMethod]
        public void CreateScraper_ScraperGetsAdded()
        {
            // Arrange
            var scraperService = new ScraperService(_mockDataBaseService.Object, _mockDataBaseServiceCategory.Object);

            // Act
            scraperService.CreateScraper(Shop.Aldi).Wait();

            // Assert
            Assert.AreEqual(1, scraperService.ActiveScrapers.Count());
        }

        [TestMethod]
        public void ScrapeAllShopsTest()
        {
            // Arrange
            var scraperService = new ScraperService(_mockDataBaseService.Object, _mockDataBaseServiceCategory.Object);

            // Act
            scraperService.ScrapeAllShops().Wait();

            // Assert
            _mockDataBaseService.Verify(x => x.CreateMany(It.IsAny<IEnumerable<ShopItem>>()));
        }

        [TestMethod]
        public void SetUpScrapeInterval_TimerRunsOut_CreateManyGetsCalled()
        {
            // Arrange
            var scraperService = new ScraperService(_mockDataBaseService.Object, _mockDataBaseServiceCategory.Object);
            scraperService.SetUpScrapeInterval(TimeSpan.FromMilliseconds(1));

            // Act
            Thread.Sleep(10);

            // Assert
            _mockDataBaseService.Verify(x => x.CreateMany(It.IsAny<IEnumerable<ShopItem>>()), Times.AtLeastOnce);
        }

        #endregion
    }
}