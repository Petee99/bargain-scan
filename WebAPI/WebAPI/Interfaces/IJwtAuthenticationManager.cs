// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IJwtAuthenticationManager.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces

{
    #region Imports

    using System.Security.Claims;

    #endregion

    public interface IJwtAuthenticationManager
    {
        #region Public Methods and Operators

        string GenerateAccessToken(IEnumerable<Claim> claims);

        string GenerateRefreshToken();

        Task<string> Authenticate(IAuthInformation authInformation);

        #endregion
    }
}