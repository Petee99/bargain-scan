// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JwtAuthenticationManager.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
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

    #endregion

    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        #region Constants and Private Fields

        private readonly IDataBaseService<UserModel> _dataBaseService;
        
        private readonly string _key;

        #endregion

        #region Constructors and Destructors

        public JwtAuthenticationManager(IDataBaseService<UserModel> dataBaseService)
        {
            _key = Environment.GetEnvironmentVariable(Constants.JwtKeyVariable)!;
            _dataBaseService = dataBaseService;
        }

        #endregion

        #region Public Methods and Operators

        public static ClaimsPrincipal GetPrincipalFromExpiredToken(string token, string symmetricSecurityKey)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(symmetricSecurityKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException();
                }

                return principal;
            }
            catch
            {
                throw new SecurityTokenException();
            }
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
            if (!await IsRightUsernamePasswordCombination(authInformation))
            {
                return string.Empty;
            }

            var claims = new Claim[]
                { new(ClaimTypes.Name, authInformation.UserName) };

            return GenerateAccessToken(claims);
        }

        #endregion

        #region Private Methods

        private async Task<bool> IsRightUsernamePasswordCombination(IAuthInformation authInformation)
        {
            List<UserModel> users = await _dataBaseService.GetAll();

            UserModel user = users.First(user => user.UserName == authInformation.UserName);

            return user.IsValidPassword(authInformation.Password);
        }

        #endregion
    }
}