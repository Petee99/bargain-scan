// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScraperService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports

    using OpenQA.Selenium.Chrome;

    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;

    #endregion

    public class ScraperService
    {
        #region Constants and Private Fields

        private readonly Timer _scraperTimer;
        private readonly DataBaseService<ShopItem> _shopItemService;
        private readonly DataBaseService<ShopItemCategory> _shopItemCategoryService;
        private readonly IList<IShopScraper> scrapers = new List<IShopScraper>();
        private readonly ChromeOptions _options = new();


        #endregion

        #region Constructors and Destructors

        public ScraperService(DataBaseService<ShopItem> shopItemService, DataBaseService<ShopItemCategory> shopItemCategoryService)
        {
            _shopItemService = shopItemService;
            _shopItemCategoryService = shopItemCategoryService;
            _scraperTimer = new Timer(new TimerCallback(ScraperTimeElapsed), this, Timeout.Infinite, Timeout.Infinite);

            _options.AddArguments("--headless");
            _options.AddArguments("--disable-gpu");
            _options.AddArguments("--no-sandbox");
        }

        #endregion
        

        #region Public Methods and Operators

        public async Task ScrapeAllShops()
        {
            await SetUpScrapers();

            var tasks = new List<Task<IEnumerable<IShopItem>>>();

            foreach (IShopScraper scraper in scrapers)
            {
                tasks.Add(scraper.ScrapeAllItemsAsync());
            }

            var results = await Task.WhenAll(tasks);
            List<ShopItem> shopItems = new();

            foreach (var result in results)
            {
                shopItems.AddRange(result.Cast<ShopItem>());
            }

            _ = _shopItemService.CreateMany(shopItems);
        }

        public void SetUpScrapeInterval(TimeSpan timeSpan)
        {
            _scraperTimer.Change(timeSpan, timeSpan);
        }

        #endregion

        #region Private Methods

        private async Task SetUpScrapers()
        {
            scrapers.Clear();

            var categories = await _shopItemCategoryService.GetAll();

            foreach (var shop in RokshScraper.AvailableShops)
            {
                scrapers.Add(new RokshScraper(new ChromeDriver(_options), shop, categories));
            }
        }

        private static void ScraperTimeElapsed(object? state)
        {
            if(state is ScraperService service)
            {
                _ = service.ScrapeAllShops();
            }
        }

        #endregion
    }
}