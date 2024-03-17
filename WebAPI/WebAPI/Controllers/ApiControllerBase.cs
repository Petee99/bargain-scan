// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiControllerBase.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Controllers

{
    #region Imports

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using WebAPI.Models.Authentication;
    using WebAPI.Properties;
    using WebAPI.Services;

    #endregion

    [Authorize]
    public class ApiControllerBase<T> : ControllerBase
    {
        #region Fields

        protected readonly DataBaseService<T> dataBaseService;

        #endregion

        #region Constructors and Destructors

        public ApiControllerBase(DataBaseService<T> dataBaseService)
        {
            this.dataBaseService = dataBaseService;
        }

        #endregion

        #region Properties

        protected string AccessTokenCookie =>
            Request.Cookies.First(cookie => cookie.Key.Equals(Constants.AccessToken)).Value;

        protected string RefreshTokenCookie =>
            Request.Cookies.First(cookie => cookie.Key.Equals(Constants.RefreshToken)).Value;

        protected string? RequestUserName => JwtAuthenticationManager.GetPrincipalFromExpiredToken(AccessTokenCookie)
            ?.Identities?.FirstOrDefault()?.Name;

        #endregion

        #region Public Methods and Operators

        [HttpDelete(Constants.ID)]
        public virtual async Task<IActionResult> Delete([FromRoute] string id)
        {
            await dataBaseService.Delete(id);

            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> Post([FromBody] T model)
        {
            await dataBaseService.Create(model);

            return Ok();
        }

        [HttpGet]
        public virtual async Task<List<T>> Get()
        {
            return await dataBaseService.GetAll();
        }

        [HttpPut(Constants.ID)]
        public virtual async Task<T?> Put([FromBody] T model)
        {
            await dataBaseService.Update(model);

            return await dataBaseService.GetById(model);
        }

        #endregion
    }
}