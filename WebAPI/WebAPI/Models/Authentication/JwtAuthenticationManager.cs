// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JwtAuthenticationManager.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.Authentication

{
    #region Imports

    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.Text;

    using Microsoft.IdentityModel.Tokens;

    using WebAPI.Interfaces;
    using WebAPI.Models.DataModels;
    using WebAPI.Properties;
    using WebAPI.Services;

    #endregion

    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        #region Constants and Private Fields

        private readonly DataBaseService<UserModel> dataBaseService;
        private readonly string key;

        #endregion

        #region Constructors and Destructors

        public JwtAuthenticationManager(string key, DataBaseService<UserModel> dataBaseService)
        {
            this.key = key;
            this.dataBaseService = dataBaseService;
        }

        #endregion

        #region Public Methods and Operators

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Constants.Key)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase)) throw new SecurityTokenException();

            return principal;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> Authenticate(IAuthInformation authInformation)
        {
            if (!await IsRightUsernamePasswordCombination(authInformation)) return string.Empty;

            var claims = new Claim[]
                { new(ClaimTypes.Name, authInformation.UserName) };

            return GenerateAccessToken(claims);
        }

        #endregion

        #region Private Methods

        private async Task<bool> IsRightUsernamePasswordCombination(IAuthInformation authInformation)
        {
            List<UserModel> users = await dataBaseService.GetAll();

            UserModel user = users.First(user => user.UserName == authInformation.UserName);

            return user != null && user.IsValidPassword(authInformation.Password);
        }

        #endregion
    }
}