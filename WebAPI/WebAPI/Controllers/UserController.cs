 // --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserController.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Controllers

{
    #region Imports

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;

    using WebAPI.Models.Authentication;
    using WebAPI.Models.DataModels;
    using WebAPI.Properties;
    using WebAPI.Services;

    #endregion

    [Route(Constants.UserRoute)]
    public class UserController : ApiControllerBase<UserModel>
    {
        #region Constants and Private Fields

        private static readonly DataBaseService<AdminModel> adminDataBaseService = new();

        private readonly JwtAuthenticationManager jwtAuthenticationManager;

        #endregion

        #region Constructors and Destructors

        public UserController(DataBaseService<UserModel> dataBaseService)
            : base(dataBaseService)
        {
            jwtAuthenticationManager = new JwtAuthenticationManager(Constants.Key, dataBaseService);
        }

        #endregion

        #region Public Methods and Operators

        [HttpPost]
        [AllowAnonymous]
        public override async Task<IActionResult> Post([FromBody] UserModel model)
        {
            if (model.UserName is null || model.Password is null) return BadRequest();

            model.HashPassword();
            model.Role = Constants.User;

            await dataBaseService.Create(model);
            await Authenticate(new AuthInformation(model.UserName, model.Password));

            return Ok();
        }

        [HttpGet]
        public override async Task<List<UserModel>> Get()
        {
            if (!(await IsAdmin())) return new List<UserModel>();

            List<UserModel> userModels = await dataBaseService.GetAll();
            userModels.ForEach(userModel => userModel.ClearSensitiveData());

            return userModels;
        }

        [HttpPost(Constants.LogOutRoute)]
        [AllowAnonymous]
        public IActionResult LogOut()
        {
            Response.Cookies.Append(Constants.AccessToken, string.Empty,
                new CookieOptions
                    { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddYears(-1) });
            Response.Cookies.Append(Constants.RefreshToken, string.Empty,
                new CookieOptions
                    { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true, Expires = DateTime.Now.AddYears(-1) });

            return Ok();
        }

        public async Task<bool> IsAdmin(string? userName = null)
        {
            if (userName is null && RequestUserName is null) return false;

            userName ??= RequestUserName;

            UserModel? userModel = (await dataBaseService.GetAll()).FirstOrDefault(user => user.UserName == userName);

            if (userModel?.ID is null) return false;

            AdminModel? model = await adminDataBaseService.GetById(userModel.ID);

            if (model is null) return false;

            return true;
        }

        [HttpPost(Constants.AddAdminRoute)]
        public async Task<IActionResult> AddAdmin([FromBody] UserModel userParam)
        {
            if (!(await IsAdmin())) return Unauthorized();

            UserModel? userModel = (await dataBaseService.GetAll()).FirstOrDefault(user => user.Email == userParam.Email);

            if (userModel is null) return BadRequest();

            await adminDataBaseService.Create(new AdminModel { ID = userModel.ID });

            userModel.Role = Constants.Admin;

            await Put(userModel);

            return Ok(Constants.Admin);
        }

        [HttpPost(Constants.AuthenticateRoute)]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] AuthInformation authInformation)
        {
            var accessToken = await jwtAuthenticationManager.Authenticate(authInformation);
            var refreshToken = jwtAuthenticationManager.GenerateRefreshToken();
            var user = (await dataBaseService.GetAll()).FirstOrDefault(user => user.UserName == authInformation.UserName);

            if (accessToken.IsNullOrEmpty() || user is null) return Unauthorized();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);

            await dataBaseService.Update(user);

            Response.Cookies.Append(Constants.AccessToken, accessToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
            Response.Cookies.Append(Constants.RefreshToken, refreshToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });

            return Ok();
        }

        [HttpPost(Constants.DeleteUserRoute)]
        public async Task<IActionResult> DeleteUser([FromBody] UserModel userParam)
        {
            if (!(await IsAdmin())) return Unauthorized();

            UserModel? userModel = (await dataBaseService.GetAll()).FirstOrDefault(user => user.Email == userParam.Email);

            if (userModel?.ID != null) await dataBaseService.Delete(userModel.ID);

            return Ok();
        }

        [HttpPost(Constants.RefreshRoute)]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh()
        {
            var principal = JwtAuthenticationManager.GetPrincipalFromExpiredToken(AccessTokenCookie);
            var userName = principal.Identity?.Name;
            var user = (await dataBaseService.GetAll()).FirstOrDefault(user => user.UserName == userName);

            if (user is null || user.RefreshToken != RefreshTokenCookie || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest();

            var newAccessToken = jwtAuthenticationManager.GenerateAccessToken(principal.Claims);
            var newRefreshToken = jwtAuthenticationManager.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await dataBaseService.Update(user);

            Response.Cookies.Append(Constants.AccessToken, newAccessToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });
            Response.Cookies.Append(Constants.RefreshToken, newRefreshToken,
                new CookieOptions { HttpOnly = true, SameSite = SameSiteMode.None, Secure = true });

            return Ok();
        }

        [HttpPost(Constants.IsAuthenticatedRoute)]
        public async Task<string> IsAuthenticated()
        {
            if (await IsAdmin())
                return Constants.Admin;
            return Constants.User;
        }

        #endregion
    }
}