// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RokshScraper.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports

    using OpenAI_API;

    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;
    using WebAPI.Properties;

    #endregion

    public class RokshScraper(IWebDriver webDriver, IOpenAiOptimizer openAiOptimizer, Shop selectedShop, IList<IShopItemCategory> defaultCategories) : IShopScraper
    {
        #region Constants and Private Fields

        private const string CategoryCssSelector =
            "a[class='category-page-sidebar-category-link category-page-sidebar-category-text collapsed']";

        private static readonly Dictionary<Shop, string> ShopUrls = new()
        {
            { Shop.Aldi, "https://shop.aldi.hu" },
            { Shop.Penny, "https://www.roksh.com/penny-hu/kinalat" },
            { Shop.Metro, "https://www.roksh.com/metro_hu/kinalat" }
        };

        private static readonly List<Shop> Shops = new() { Shop.Penny, Shop.Metro, Shop.Aldi };

        private readonly IList<IShopItemCategory> _allCategories = new List<IShopItemCategory>();

        #endregion

        #region Public Properties

        public static IList<Shop> AvailableShops => Shops;

        #endregion

        #region Public Methods and Operators

        public async Task<IEnumerable<IShopItem>> ScrapeAllItemsAsync()
        {
            int attempts = 0;
            int maxAttempts = 3;

            while (attempts < maxAttempts)
            {
                try
                {
                    IList<IShopItem> items = ScrapeShopItemsAsync().ToList();

                    webDriver.Quit();

                    await openAiOptimizer.OptimizeCategoriesToMatchSchema(_allCategories, defaultCategories.Cast<IShopItemCategory>().ToList());

                    return items;
                }
                catch (Exception)
                {
                    attempts++;

                    if (attempts > maxAttempts)
                    {
                        break;
                    }
                }
            }

            return new List<IShopItem>();
        }

        #endregion

        #region Private Methods

        private Dictionary<IShopItemCategory, List<IShopItemCategory>> GetCategoriesWithSubcategories()
        {
            Dictionary<IShopItemCategory, List<IShopItemCategory>> categoriesWithSubCategories = new();

            WebDriverWait wait = new(webDriver, TimeSpan.FromSeconds(5));
            wait.Until(x => true);

            for (int index = 0; index < webDriver.FindElements(By.CssSelector(CategoryCssSelector)).Count; index++)
            {
                IWebElement categoryElement = webDriver.FindElements(By.CssSelector(CategoryCssSelector))[index];
                categoryElement.Click();

                ShopItemCategory category = new(categoryElement.Text, categoryElement.GetAttribute("href"), string.Empty);

                List<IShopItemCategory> subCategories = categoryElement.FindElement(By.XPath(".."))
                    .FindElements(By.CssSelector("a[class='category-page-sidebar-subcategory-link']"))
                    .Select(subCategory => 
                        new ShopItemCategory(subCategory.FindElement(By.CssSelector("span")).Text, subCategory.GetAttribute("href"), category.Name))
                    .Cast<IShopItemCategory>()
                    .ToList();

                if (subCategories.Count is 0)
                {
                    subCategories.Add(new ShopItemCategory(category.Name, category.Link, category.Name));
                }

                categoriesWithSubCategories.Add(category, subCategories);
            }

            return categoriesWithSubCategories;
        }


        private IEnumerable<IShopItem> ScrapeShopItemsAsync()
        {
            webDriver.Navigate().GoToUrl(ShopUrls[selectedShop]);

            IList<IShopItem> items = new List<IShopItem>();

            foreach (var subCategory in GetCategoriesWithSubcategories().SelectMany(category => category.Value))
            {
                _allCategories.Add(subCategory);
                webDriver.Navigate().GoToUrl(subCategory.Link);
                ScrollTillEnd("div[itemprop='item']");

                foreach (var item in webDriver.FindElements(By.CssSelector("div[itemprop='item']")))
                {
                    items.Add(new ShopItem(
                        item.FindElement(By.CssSelector("span[itemprop='name']")).Text,
                        subCategory,
                        item.FindElement(By.CssSelector("span[itemprop='price']")).Text + " Ft",
                        selectedShop));
                }
            }

            return items;
        }

        private void ScrollTillEnd(string cssElementToLookFor)
        {
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)webDriver;
            bool endOfPage = false;

            WebDriverWait wait = new(webDriver, TimeSpan.FromSeconds(5));

            while (!endOfPage)
            {
                var itemNumber = webDriver.FindElements(By.CssSelector("div[itemprop='item']")).Count;
                jsExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                try
                {
                    wait.Until(x => itemNumber < webDriver.FindElements(By.CssSelector(cssElementToLookFor)).Count);
                }
                catch (Exception)
                {
                    endOfPage = true;
                }
            }
        }

        #endregion
    }
}