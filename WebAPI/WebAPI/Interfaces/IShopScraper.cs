// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShopScraper.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.Text.Json;

namespace WebAPI.Interfaces
{
    public interface IShopScraper
    {
        #region Public Methods and Operators

        Task<IEnumerable<IShopItem>> ScrapeAllItemsAsync();

        #endregion
    }
}