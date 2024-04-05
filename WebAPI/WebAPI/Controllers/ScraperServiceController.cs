// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScraperServiceController.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Controllers
{
    #region Imports

    using Microsoft.AspNetCore.Mvc;

    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;
    using WebAPI.Properties;

    #endregion

    [Route(Constants.SetScraperServiceRoute)]
    public class ScraperServiceController : ApiControllerBase<ScrapeRequest>
    {
        #region Constants and Private Fields

        private readonly IScraperService _scraperService;

        #endregion

        #region Constructors and Destructors

        public ScraperServiceController(IDataBaseService<ScrapeRequest> dataBaseService, IScraperService scraperService)
            : base(dataBaseService)
        {
            _scraperService = scraperService;
        }

        #endregion

        #region Public Methods and Operators

        [HttpPost]
        public override Task<IActionResult> Post(ScrapeRequest request)
        {
            if (request.Type is ScrapeRequestType.Immediate)
            {
                Task.Run(ScrapeAllShops);
            }
            else
            {
                _scraperService.SetUpScrapeInterval(TimeSpan.FromDays(request.Days));
            }

            return Task.FromResult<IActionResult>(Ok());
        }

        #endregion

        #region Private Methods

        private async Task ScrapeAllShops()
        {
            foreach (Shop shop in Enum.GetValues<Shop>())
            {
                await _scraperService.CreateScraper(shop);
            }

            await _scraperService.ScrapeAllShops();
        }

        #endregion
    }
}