// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScraperService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    public interface IScraperService
    {
        #region Public Methods and Operators

        Task ScrapeAllShops();

        void SetUpScrapeInterval(TimeSpan timeSpan);

        #endregion
    }
}