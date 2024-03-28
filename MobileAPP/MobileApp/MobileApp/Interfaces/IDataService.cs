// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IDataService
    {
        #region Public Properties

        IUserProfile UserProfile { get; }

        #endregion

        #region Public Methods and Operators

        bool TryUpdateBarCode(IShopItem item, string barCode);

        Task<IEnumerable<ICategory>> GetMainCategories();

        Task<IEnumerable<IShopItem>> GetShopItems(ICategory category);

        Task<IEnumerable<IShopItem>> GetShopItems(ISubCategory subCategory);

        Task<IEnumerable<IShopItem>> GetShopItemsByBarcode(string barCode);

        Task<IEnumerable<IShopItem>> GetShopItemsBySearchTerm(string searchTerm);

        void SaveApplicationData();

        #endregion
    }
}