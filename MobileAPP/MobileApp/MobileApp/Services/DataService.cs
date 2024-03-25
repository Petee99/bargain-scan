// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Services
{
    #region Imports

    using System.Text.Json;

    using MobileApp.Interfaces;
    using MobileApp.Models;

    #endregion

    internal class DataService : IDataService
    {
        #region Constants and Private Fields

        private readonly Dictionary<ICategory, IList<IShopItem>> _shopItemsMap = new();

        private readonly Dictionary<string, IList<IShopItem>> _shopItemBarcodeDictionary = new();

        private readonly IDataPersistenceService _persistenceService = new DataPersistenceService();

        private readonly IFuzzySearch<IShopItem> _fuzzyItemRepository = new FuzzySearch<IShopItem>();

        private readonly IOnlineDataService _onlineDataService = new OnlineDataService();

        private readonly List<Category> _categories = new();

        private readonly List<Task> _loadingTasks;

        #endregion

        #region Constructors and Destructors

        public DataService()
        {
            _loadingTasks = new List<Task>
            {
                Task.Run(LoadShopData),
                Task.Run(LoadUserProfile)
            };
        }

        #endregion

        #region Public Properties

        public IUserProfile UserProfile { get; private set; }

        #endregion

        #region Public Methods and Operators

        public bool TryUpdateBarCode(IShopItem item, string barCode)
        {
            return !item.TryUpdateBarCode(barCode) && !TryAddBarcodeToDictionary(barCode, item);
        }

        public async Task<IEnumerable<ICategory>> GetMainCategories()
        {
            await EnsureDataLoaded();
            return await Task.FromResult(_categories);
        }

        public async Task<IEnumerable<IShopItem>> GetShopItems(ICategory category)
        {
            await EnsureDataLoaded();

            if (!_categories.Contains(category))
            {
                return await Task.FromResult(Enumerable.Empty<IShopItem>());
            }

            List<IShopItem> shopItems = new();

            foreach (ISubCategory subCategory in category.SubCategories)
            {
                shopItems.AddRange(GetShopItems(subCategory).Result);
            }

            return await Task.FromResult((IEnumerable<IShopItem>)shopItems);
        }

        public async Task<IEnumerable<IShopItem>> GetShopItems(ISubCategory subCategory)
        {
            await EnsureDataLoaded();
            _shopItemsMap.TryGetValue(subCategory, out IList<IShopItem> items);
            return await Task.FromResult(items ?? Enumerable.Empty<IShopItem>());
        }

        public async Task<IEnumerable<IShopItem>> GetShopItemsByBarcode(string barCode)
        {
            await EnsureDataLoaded();
            _shopItemBarcodeDictionary.TryGetValue(barCode, out IList<IShopItem> foundItems);
            return await Task.FromResult(foundItems);
        }

        public async Task<IEnumerable<IShopItem>> GetShopItemsBySearchTerm(string searchTerm)
        {
            await EnsureDataLoaded();
            return await Task.FromResult(_fuzzyItemRepository.Search(searchTerm));
        }

        #endregion

        #region Private Methods

        private bool TryAddBarcodeToDictionary(string barCode, IShopItem item)
        {
            _shopItemBarcodeDictionary.TryGetValue(barCode, out var shopItems);

            if (shopItems is null)
            {
                _shopItemBarcodeDictionary.TryAdd(item.BarCode, new List<IShopItem> { item });
                return true;
            }

            if (shopItems.Contains(item))
            {
                return false;
            }

            shopItems.Add(item);
            return true;
        }

        private ICategory GetItemContainingCategory(IShopItem item)
        {
            if (_categories.FirstOrDefault(c => c.Title == item.Category) is { } category)
            {
                return category.GetOrCreateSubCategory(item.SubCategory);
            }

            category = new Category(item.Category);
            _categories.Add(category);

            return category.GetOrCreateSubCategory(item.SubCategory);
        }

        private async Task EnsureDataLoaded()
        {
            await Task.WhenAll(_loadingTasks);
        }

        private async Task LoadShopData()
        {
            string data = await _persistenceService.TryLoadLocalData(DataPersistenceService.ItemsFileName);

            if (data == string.Empty)
            {
                data = await _onlineDataService.LoadShopItemsFromApi();
                _persistenceService.TrySaveLocalData(DataPersistenceService.ItemsFileName, data);
            }

            foreach (ShopItem item in JsonSerializer.Deserialize<List<ShopItem>>(data))
            {
                FixItemCategory(item);
                ICategory containingCategory = GetItemContainingCategory(item);

                if (!_shopItemsMap.ContainsKey(containingCategory))
                {
                    _shopItemsMap.Add(containingCategory, new List<IShopItem>());
                }

                if (!_shopItemsMap[containingCategory].Contains(item))
                {
                    _shopItemsMap[containingCategory].Add(item);
                    _fuzzyItemRepository.AddItem(item);
                }

                if (item.BarCode != null)
                {
                    TryAddBarcodeToDictionary(item.BarCode, item);
                }
            }
        }

        private async Task LoadUserProfile()
        {
            UserProfile = await _persistenceService.TryLoadLocalData(DataPersistenceService.UserProfileFileName) is { Length : > 0 } data
                ? JsonSerializer.Deserialize<LocalUserProfile>(data)
                : new LocalUserProfile();
        }

        private static void FixItemCategory(ShopItem item)
        {
            if (item.Category.StartsWith("Karácsony"))
            {
                item.Category = "Nassolnivalók";
                item.SubCategory = "Sütemények, desszert";
            }

            if (!item.Category.StartsWith("Közép"))
            {
                return;
            }

            item.Category = item.SubCategory = "Egyéb";
        }

        #endregion
    }
}