// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrapeRequest.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.WebScraping
{
    #region Imports

    using WebAPI.Enums;

    #endregion

    public class ScrapeRequest(ScrapeRequestType type, int days)
    {
        #region Public Properties

        public int Days => days;

        public ScrapeRequestType Type => type;

        #endregion
    }
}