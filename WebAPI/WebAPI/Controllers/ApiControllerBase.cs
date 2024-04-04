// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiControllerBase.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Controllers

{
    #region Imports

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using WebAPI.Interfaces;
    using WebAPI.Models.Authentication;
    using WebAPI.Properties;
    
    #endregion

    [Authorize]
    public class ApiControllerBase<T> : ControllerBase
    {
        #region Fields

        protected readonly IDataBaseService<T> DataBaseService;

        #endregion

        #region Constructors and Destructors

        public ApiControllerBase(IDataBaseService<T> dataBaseService)
        {
            this.DataBaseService = dataBaseService;
        }

        #endregion

        #region Properties

        protected string AccessTokenCookie =>
            Request.Cookies.First(cookie => cookie.Key.Equals(Constants.AccessToken)).Value;

        protected string RefreshTokenCookie =>
            Request.Cookies.First(cookie => cookie.Key.Equals(Constants.RefreshToken)).Value;

        protected string? RequestUserName => JwtAuthenticationManager
            .GetPrincipalFromExpiredToken(AccessTokenCookie, Environment.GetEnvironmentVariable(Constants.SymmetricKeyVariable)!)
            .Identities.FirstOrDefault()?.Name;

        #endregion

        #region Public Methods and Operators

        [HttpDelete(Constants.ID)]
        public virtual async Task<IActionResult> Delete([FromRoute] string id)
        {
            await DataBaseService.Delete(id);

            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] T model)
        {
            await DataBaseService.Create(model);

            return Ok();
        }

        [HttpGet]
        public virtual async Task<List<T>> Get()
        {
            return await DataBaseService.GetAll();
        }

        [HttpPut(Constants.ID)]
        public virtual async Task<T?> Put([FromBody] T model)
        {
            await DataBaseService.Update(model);

            return await DataBaseService.GetByModel(model);
        }

        #endregion
    }
}