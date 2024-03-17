using Microsoft.AspNetCore.Mvc;
using WebAPI.Models.WebScraping;
using WebAPI.Properties;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route(Constants.SetScraperServiceRoute)]
    public class ScraperServiceController : ApiControllerBase<ScrapeRequest>
    {
        private readonly ScraperService _scraperService;

        public ScraperServiceController(DataBaseService<ScrapeRequest> dataBaseService, ScraperService scraperService) 
            : base(dataBaseService)
        {
            _scraperService = scraperService;
        }

        [HttpPost]
        public override async Task<IActionResult> Post(ScrapeRequest request)
        {
            if (request.Type is Enums.ScrapeRequestType.Immediate)
            {
                _ = _scraperService.ScrapeAllShops();
            }
            else
            {
                _scraperService.SetUpScrapeInterval(TimeSpan.FromDays(request.Days));
            }

            return Ok();
        }
    }
}
