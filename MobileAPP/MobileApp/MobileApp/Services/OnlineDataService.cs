// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OnlineDataService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Services
{
    #region Imports

    using MobileApp.Interfaces;

    #endregion

    internal class OnlineDataService : IOnlineDataService
    {
        #region Constants and Private Fields

        private const string ApiUrl = "";

        #endregion

        #region Public Methods and Operators

        public async Task<string> LoadShopItemsFromApi()
        {
            var response = await new HttpClient().GetAsync(ApiUrl);
            return await response.Content.ReadAsStringAsync();
        }

        #endregion
    }
}