﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IOnlineDataService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IOnlineDataService
    {
        #region Public Methods and Operators

        Task<string> LoadShopItemsFromApi();

        #endregion
    }
}