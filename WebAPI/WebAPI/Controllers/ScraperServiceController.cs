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
    using WebAPI.Services;

    #endregion

    [Route(Constants.SetScraperServiceRoute)]
    public class ScraperServiceController : ApiControllerBase<ScrapeRequest>
    {
        #region Constants and Private Fields

        private readonly ScraperService _scraperService;

        #endregion

        #region Constructors and Destructors

        public ScraperServiceController(IDataBaseService<ScrapeRequest> dataBaseService, ScraperService scraperService)
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
                Task.Run(_scraperService.ScrapeAllShops);
            }
            else
            {
                _scraperService.SetUpScrapeInterval(TimeSpan.FromDays(request.Days));
            }

            return Task.FromResult<IActionResult>(Ok());
        }

        #endregion
    }
}