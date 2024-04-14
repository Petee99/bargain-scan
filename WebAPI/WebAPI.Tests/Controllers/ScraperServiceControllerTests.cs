// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScraperServiceControllerTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Controllers
{
    #region Imports

    using Moq;

    using WebAPI.Controllers;
    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;

    #endregion

    [TestClass]
    public class ScraperServiceControllerTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void Post_WhenCalledWithImmediateRequest_CallsScrapeAllShops()
        {
            // Arrange
            var scraperService = new Mock<IScraperService>();
            var dataBaseService = new Mock<IDataBaseService<ScrapeRequest>>();
            var controller = new ScraperServiceController(dataBaseService.Object, scraperService.Object);
            var request = new ScrapeRequest(ScrapeRequestType.Immediate, 0);

            // Act
            controller.Post(request).Wait();
            Task.Delay(10000).Wait();

            // Assert
            scraperService.Verify(service => service.ScrapeAllShops(), Times.Once);
        }

        [TestMethod]
        public void Post_WhenCalledWithTimedRequest_CallsSetUpScrapeInterval()
        {
            // Arrange
            var scraperService = new Mock<IScraperService>();
            var dataBaseService = new Mock<IDataBaseService<ScrapeRequest>>();
            var controller = new ScraperServiceController(dataBaseService.Object, scraperService.Object);
            var request = new ScrapeRequest(ScrapeRequestType.Timed, 1);

            // Act
            var result = controller.Post(request).Result;

            // Assert
            scraperService.Verify(service => service.SetUpScrapeInterval(It.IsAny<TimeSpan>()), Times.Once);
        }

        #endregion
    }
}