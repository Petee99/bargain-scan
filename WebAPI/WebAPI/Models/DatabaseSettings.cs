// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DatabaseSettings.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models
{
    public class DatabaseSettings
    {
        #region Public Properties

        public string ConnectionURI { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        #endregion
    }
}