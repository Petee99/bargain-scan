// --------------------------------------------------------------------------------------------------------------------
// <copyright file="JwtAuthenticationManagerTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Models.Authentication
{
    #region Imports

    using System.Security.Claims;

    using Microsoft.IdentityModel.Tokens;

    using Moq;

    using WebAPI.Interfaces;
    using WebAPI.Models.Authentication;
    using WebAPI.Models.DataModels;
    using WebAPI.Properties;

    #endregion

    [TestClass]
    public class JwtAuthenticationManagerTests
    {
        #region Constants and Private Fields

        private const string MockedJwtKey = "my-32-character-ultra-secure-and-ultra-long-secret";

        private readonly Mock<IDataBaseService<UserModel>> _dataBaseServiceMock = new();

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable(Constants.JwtKeyVariable, MockedJwtKey);
        }

        [TestMethod]
        public void Authenticate_WhenCalledWithInvalidAuthInformation_ReturnsEmptyString()
        {
            // Arrange
            var authInformation = new AuthInformation("username", "password");
            var userModel = new UserModel { UserName = "username", Password = "password" };
            userModel.HashPassword();

            _dataBaseServiceMock.Setup(x => x.GetAll()).Returns(Task.FromResult(new List<UserModel> { userModel }));

            var jwtAuthenticationManager = new JwtAuthenticationManager(_dataBaseServiceMock.Object);

            // Act
            var token = jwtAuthenticationManager.Authenticate(new AuthInformation("username", "invalid")).Result;

            // Assert
            Assert.IsTrue(token.Length is 0);
        }

        [TestMethod]
        public void Authenticate_WhenCalledWithValidAuthInformation_ReturnsToken()
        {
            // Arrange
            var authInformation = new AuthInformation("username", "password");
            var userModel = new UserModel { UserName = "username", Password = "password" };
            userModel.HashPassword();

            _dataBaseServiceMock.Setup(x => x.GetAll()).Returns(Task.FromResult(new List<UserModel> { userModel }));

            var jwtAuthenticationManager = new JwtAuthenticationManager(_dataBaseServiceMock.Object);

            // Act
            var token = jwtAuthenticationManager.Authenticate(authInformation).Result;

            // Assert
            Assert.IsTrue(token.Length > 0);
        }

        [TestMethod]
        public void GenerateAccessToken_WhenCalled_ReturnsToken()
        {
            // Arrange
            var jwtAuthenticationManager = new JwtAuthenticationManager(_dataBaseServiceMock.Object);

            // Act
            var token = jwtAuthenticationManager.GenerateAccessToken(new Claim[]
                { new(ClaimTypes.Name, "username") });

            // Assert
            Assert.IsTrue(token.Length > 0);
        }

        [TestMethod]
        public void GenerateRefreshToken_WhenCalled_ReturnsToken()
        {
            // Arrange
            var jwtAuthenticationManager = new JwtAuthenticationManager(_dataBaseServiceMock.Object);

            // Act
            var token = jwtAuthenticationManager.GenerateRefreshToken();

            // Assert
            Assert.IsTrue(token.Length > 0);
        }

        [TestMethod]
        public void GetPrincipalFromExpiredToken_WhenCalledWithRightSecurityKey_ReturnsPrincipal()
        {
            // Arrange
            var jwtAuthenticationManager = new JwtAuthenticationManager(_dataBaseServiceMock.Object);
            var token = jwtAuthenticationManager.GenerateAccessToken(new Claim[]
                { new(ClaimTypes.Name, "username") });

            // Act
            var principal = JwtAuthenticationManager.GetPrincipalFromExpiredToken(token, MockedJwtKey);

            // Assert
            Assert.IsNotNull(principal);
        }

        [TestMethod]
        public void GetPrincipalFromExpiredToken_WhenCalledWithWrongSecurityKey_ThrowsSecurityTokenException()
        {
            // Arrange
            var jwtAuthenticationManager = new JwtAuthenticationManager(_dataBaseServiceMock.Object);
            var token = jwtAuthenticationManager.GenerateAccessToken(new Claim[]
                { new(ClaimTypes.Name, "username") });

            // Act
            void Action()
            {
                JwtAuthenticationManager.GetPrincipalFromExpiredToken(token, "wrong-character-ultra-secure-and-ultra-long-secret");
            }

            // Assert
            Assert.ThrowsException<SecurityTokenException>(Action);
        }

        #endregion
    }
}