// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthInformation.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.Authentication
{
    #region Imports

    using WebAPI.Interfaces;

    #endregion

    public class AuthInformation : IAuthInformation
    {
        #region Constructors and Destructors

        public AuthInformation(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        #endregion

        #region Public Properties

        public string Password { get; set; }

        public string UserName { get; set; }

        #endregion
    }
}