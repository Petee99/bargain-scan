// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScraperService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports

    using OpenAI_API;

    using OpenQA.Selenium.Chrome;

    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;
    using WebAPI.Properties;

    #endregion
    
    public class ScraperService : IScraperService
    {
        #region Constants and Private Fields

        private readonly ChromeOptions _options = new();

        private readonly IDataBaseService<ShopItem> _shopItemService;

        private readonly IDataBaseService<ShopItemCategory> _shopItemCategoryService;

        private readonly IList<IShopScraper> _scrapers = new List<IShopScraper>();

        private readonly IOpenAiOptimizer _openAiOptimizer = 
            new OpenAiOptimizer(new OpenAIAPI(Environment.GetEnvironmentVariable(Constants.OpenAiApiKeyVariable)));

        private readonly Timer _scraperTimer;

        #endregion

        #region Constructors and Destructors

        public ScraperService(IDataBaseService<ShopItem> shopItemService, IDataBaseService<ShopItemCategory> shopItemCategoryService)
        {
            _shopItemService = shopItemService;
            _shopItemCategoryService = shopItemCategoryService;
            _scraperTimer = new Timer(ScraperTimeElapsed, this, Timeout.Infinite, Timeout.Infinite);

            _options.AddArguments("--headless");
            _options.AddArguments("--disable-gpu");
            _options.AddArguments("--no-sandbox");
        }

        #endregion

        #region Public Methods and Operators

        public async Task ScrapeAllShops()
        {
            await SetUpScrapers();

            var tasks = _scrapers.Select(scraper => scraper.ScrapeAllItemsAsync()).ToList();

            var results = await Task.WhenAll(tasks);
            List<ShopItem>? shopItems = new();

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
            _scrapers.Clear();

            var categories = await _shopItemCategoryService.GetAll();

            foreach (var shop in RokshScraper.AvailableShops)
            {
                _scrapers.Add(new RokshScraper(
                    new ChromeDriver(_options), _openAiOptimizer, shop, categories.Cast<IShopItemCategory>().ToList()));
            }
        }

        private static void ScraperTimeElapsed(object? state)
        {
            if (state is ScraperService service)
            {
                _ = service.ScrapeAllShops();
            }
        }

        #endregion
    }
}