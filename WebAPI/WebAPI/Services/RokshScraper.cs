// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AldiScraper.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports
    
    using OpenQA.Selenium;
    using OpenQA.Selenium.Support.UI;

    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Models.WebScraping;

    #endregion

    public class RokshScraper(IWebDriver webDriver, Shop selectedShop, IList<ShopItemCategory> defaultCategories) : IShopScraper
    {
        #region Constants and Private Fields

        private const string CategoryCssSelector =
            "a[class='category-page-sidebar-category-link category-page-sidebar-category-text collapsed']";

        private static readonly Dictionary<Shop, string> shopUrls = new()
        {
            { Shop.Aldi, "https://shop.aldi.hu" },
            { Shop.Penny, "https://www.roksh.com/penny-hu/kinalat" },
            { Shop.Metro, "https://www.roksh.com/metro_hu/kinalat" }
        };
        
        private List<IShopItemCategory> allCategories = new();

        private static readonly List<Shop> availableShops = new() { Shop.Penny, Shop.Metro, Shop.Aldi };

        private readonly Shop selectedShop = selectedShop;

        #endregion

        #region Public Methods and Operators

        public static IList<Shop> AvailableShops => availableShops;

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

                    await OpenAiOptimizer.OptimizeCategoriesToMatchSchema(allCategories, defaultCategories.Cast<IShopItemCategory>().ToList());

                    return items;
                }
                catch (Exception e)
                {
                    attempts++;

                    if (attempts > maxAttempts)
                    {
                        Console.WriteLine(e);
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

                ShopItemCategory category = new(categoryElement.Text, categoryElement.GetAttribute("href"), String.Empty);

                List<IShopItemCategory> subCategories = new();

                foreach (var subCategory in categoryElement.FindElement(By.XPath(".."))
                             .FindElements(By.CssSelector("a[class='category-page-sidebar-subcategory-link']")))
                    subCategories.Add(new ShopItemCategory(subCategory.FindElement(By.CssSelector("span")).Text,
                        subCategory.GetAttribute("href"), category.Name));

                if (subCategories.Count is 0) subCategories.Add(new ShopItemCategory(category.Name, category.Link, category.Name));

                categoriesWithSubCategories.Add(category, subCategories);
            }
            
            return categoriesWithSubCategories;
        }
        

        private IEnumerable<IShopItem> ScrapeShopItemsAsync()
        {
            webDriver.Navigate().GoToUrl(shopUrls[selectedShop]);

            IList<IShopItem> items = new List<IShopItem>();
            
            foreach (var category in GetCategoriesWithSubcategories())
            {
                foreach (var subCategory in category.Value)
                {
                    allCategories.Add(subCategory);
                    webDriver.Navigate().GoToUrl(subCategory.Link);
                    ScrollTillEnd("div[itemprop='item']");

                    foreach (var item in webDriver.FindElements(By.CssSelector("div[itemprop='item']")))
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