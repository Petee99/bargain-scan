// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScraperService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports

    using System.Reflection;

    using OpenAI_API;

    using OpenQA.Selenium.Chrome;

    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;
    using WebAPI.Properties;

    #endregion

    public class ScraperService : IScraperService
    {
        #region Constants and Private Fields

        private readonly ChromeOptions _options = new();

        private readonly Dictionary<Shop, IShopScraper> _scrapers = new();

        private readonly IDataBaseService<ShopItem> _shopItemService;

        private readonly IDataBaseService<ShopItemCategory> _shopItemCategoryService;

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

        #region Public Properties

        public IEnumerable<IShopScraper> ActiveScrapers => _scrapers.Values;

        #endregion

        #region Public Methods and Operators

        public async Task CreateScraper(Shop shop)
        {
            var categories = await _shopItemCategoryService.GetAll();

            if (GetScraperForShop(shop, categories) is not { } shopScraper)
            {
                return;
            }

            _scrapers.TryAdd(shop, shopScraper);
        }

        public async Task ScrapeAllShops()
        {
            await SetUpScrapers();

            var tasks = _scrapers.Values.Select(scraper => scraper.ScrapeAllItemsAsync()).ToList();

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

        private IShopScraper? GetScraperForShop(Shop shop, List<ShopItemCategory> categories)
        {
            if (GetShopScraperTypeThatContainsShop(shop) is not { } scraperType)
            {
                return null;
            }

            return (IShopScraper)Activator.CreateInstance(scraperType,
                new ChromeDriver(_options), _openAiOptimizer, shop, categories.Cast<IShopItemCategory>().ToList())!;
        }

        private async Task SetUpScrapers()
        {
            var categories = await _shopItemCategoryService.GetAll();

            foreach (Shop shop in _scrapers.Keys)
            {
                _scrapers[shop] = GetScraperForShop(shop, categories)!;
            }
        }

        private static Type? GetShopScraperTypeThatContainsShop(Shop shop)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (!typeof(IShopScraper).IsAssignableFrom(type) || !type.IsClass || type.IsAbstract)
                    {
                        continue;
                    }

                    var availableShopsProperty =
                        type.GetProperty("AvailableShops", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

                    if (availableShopsProperty == null)
                    {
                        continue;
                    }

                    if (availableShopsProperty.GetValue(null) is IList<Shop> availableShops && availableShops.Contains(shop))
                    {
                        return type;
                    }
                }
            }

            return null;
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