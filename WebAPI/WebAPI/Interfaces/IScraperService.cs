﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScraperService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    #region Imports

    using WebAPI.Enums;

    #endregion

    public interface IScraperService
    {
        #region Public Properties

        IEnumerable<IShopScraper> ActiveScrapers { get; }

        #endregion

        #region Public Methods and Operators

        Task CreateScraper(Shop shop);

        Task ScrapeAllShops();

        void SetUpScrapeInterval(TimeSpan timeSpan);

        #endregion
    }
}