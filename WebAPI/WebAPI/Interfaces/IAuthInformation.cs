// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuthInformation.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    public interface IAuthInformation
    {
        #region Public Properties

        public string Password { get; set; }

        public string UserName { get; set; }

        #endregion
    }
}