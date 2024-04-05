// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserControllerTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Controllers
{
    #region Imports

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    using MongoDB.Bson.Serialization.IdGenerators;

    using Moq;

    using WebAPI.Controllers;
    using WebAPI.Interfaces;
    using WebAPI.Models.Authentication;
    using WebAPI.Models.DataModels;
    using WebAPI.Properties;

    #endregion

    [TestClass]
    public class UserControllerTests
    {
        #region Constants and Private Fields

        private const string MockedJwtKey = "my-32-character-ultra-secure-and-ultra-long-secret";

        private readonly Mock<IDataBaseService<AdminModel>> _adminDataBaseServiceMock = new();

        private readonly Mock<IDataBaseService<UserModel>> _userDataBaseServiceMock = new();

        private readonly Mock<IResponseCookies> _cookiesMock = new();

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Initialize()
        {
            Environment.SetEnvironmentVariable(Constants.JwtKeyVariable, MockedJwtKey);
            Environment.SetEnvironmentVariable(Constants.SymmetricKeyVariable, MockedJwtKey);

            List<UserModel> userModels = new List<UserModel>();
            List<AdminModel> adminModels = new List<AdminModel>();

            _userDataBaseServiceMock.Setup(x => x.Create(It.IsAny<UserModel>())).Callback((UserModel model) => userModels.Add(model));
            _userDataBaseServiceMock.Setup(x => x.GetAll()).Returns(Task.FromResult(userModels));

            _adminDataBaseServiceMock.Setup(x => x.Create(It.IsAny<AdminModel>())).Callback((AdminModel model) => adminModels.Add(model));
            _adminDataBaseServiceMock.Setup(x => x.GetById(It.IsAny<string>()))
                .Returns((string id) => Task.FromResult(adminModels.FirstOrDefault(x => x.ID == id)));

            Dictionary<string, string> capturedCookies = new();

            _cookiesMock.Setup(c => c.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()))
                .Callback<string, string, CookieOptions>((name, value, options) => { capturedCookies[name] = value; });
        }

        [TestMethod]
        public void AddAdmin_WhenCalledWithAdmin_UserDoesNotExist_ReturnsBadRequest()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz", Email = "xyz@xyz.com" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();
            var admin = new UserModel { UserName = "admin", Password = "admin", Email = "admin@admin.com" };
            admin.HashPassword();
            admin.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, admin).ToString();

            _userDataBaseServiceMock.Object.Create(model);
            _userDataBaseServiceMock.Object.Create(admin);
            _adminDataBaseServiceMock.Object.Create(new AdminModel { ID = admin.ID });

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(admin.UserName, "admin")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            var x = context.Request.Cookies.First(cookie => cookie.Key.Equals(Constants.AccessToken));

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.AddAdmin(new UserModel()).Result;

            // Assert
            _userDataBaseServiceMock.Verify(service => service.Update(It.IsAny<UserModel>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void AddAdmin_WhenCalledWithAdminUser_ReturnsOk()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz", Email = "xyz@xyz.com" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();
            var admin = new UserModel { UserName = "admin", Password = "admin" };
            admin.HashPassword();
            admin.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, admin).ToString();

            _userDataBaseServiceMock.Object.Create(model);
            _userDataBaseServiceMock.Object.Create(admin);
            _adminDataBaseServiceMock.Object.Create(new AdminModel { ID = admin.ID });

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(admin.UserName, "admin")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            var x = context.Request.Cookies.First(cookie => cookie.Key.Equals(Constants.AccessToken));

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.AddAdmin(model).Result;

            // Assert
            _userDataBaseServiceMock.Verify(service => service.Update(It.IsAny<UserModel>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        }

        [TestMethod]
        public void AddAdmin_WhenCalledWithNotAdminUser_ReturnsUnauthorized()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz", Email = "xyz@xyz.com" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.AddAdmin(model).Result;

            // Assert
            _userDataBaseServiceMock.Verify(service => service.Update(It.IsAny<UserModel>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void Authenticate_WhenCalledWithInvalidAuthInformation_ReturnsUnauthorized()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);

            HttpContext context = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.Authenticate(new AuthInformation(model.UserName, "wrongPw")).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void Authenticate_WhenCalledWithValidAuthInformation_ReturnsUnauthorized()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);

            HttpContext context = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.Authenticate(new AuthInformation("xyz", "xyz")).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void DeleteUser_RequestUserIsAdmin_ReturnsOk()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz", Email = "xyz@xyz.com" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();
            var admin = new UserModel { UserName = "admin", Password = "admin" };
            admin.HashPassword();
            admin.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, admin).ToString();

            _userDataBaseServiceMock.Object.Create(model);
            _userDataBaseServiceMock.Object.Create(admin);
            _adminDataBaseServiceMock.Object.Create(new AdminModel { ID = admin.ID });

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(admin.UserName, "admin")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.DeleteUser(model).Result;

            // Assert
            _userDataBaseServiceMock.Verify(service => service.Delete(It.IsAny<string>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void DeleteUser_RequestUserIsNotAdmin_ReturnsUnauthorized()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz", Email = "xyz@xyz.com" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.DeleteUser(model).Result;

            // Assert
            _userDataBaseServiceMock.Verify(service => service.Delete(It.IsAny<string>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        public void Get_WhenCalledWithAdminUser_ReturnsNonSensitiveData()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            var model2 = new UserModel { UserName = "x22yz", Password = "x22yz" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);
            _userDataBaseServiceMock.Object.Create(model2);
            _adminDataBaseServiceMock.Object.Create(new AdminModel { ID = model.ID });

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.Get().Result;

            // Assert
            Assert.AreEqual(result.Count, _userDataBaseServiceMock.Object.GetAll().Result.Count);
            Assert.IsTrue(result.All(a => a.Password == string.Empty));
            Assert.IsTrue(result.All(a => a.ID == string.Empty));
        }

        [TestMethod]
        public void Get_WhenCalledWithNonAdminUser_ReturnsEmptyList()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.Get().Result;

            // Assert
            Assert.AreEqual(result.Count, 0);
        }

        [TestMethod]
        public void IsAdmin_WhenCalledWithAdminUser_ReturnsOk()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);
            _adminDataBaseServiceMock.Object.Create(new AdminModel { ID = model.ID });

            // Act
            var result = controller.IsAdmin(model.UserName).Result;

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsAdmin_WhenCalledWithNotAdminUser_ReturnsFalse()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);

            // Act
            var result = controller.IsAdmin(model.UserName).Result;

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsAuthenticated_UserIsAdmin_ReturnsAdminString()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);
            _adminDataBaseServiceMock.Object.Create(new AdminModel { ID = model.ID });

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.IsAuthenticated().Result;

            // Assert
            Assert.AreEqual(result, Constants.Admin);
        }

        [TestMethod]
        public void IsAuthenticated_UserIsNotAdmin_ReturnsUserString()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.HashPassword();
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            _userDataBaseServiceMock.Object.Create(model);

            HttpContext context = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
                new(Constants.AccessToken,
                    new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            context.Request.Cookies = cookiesMock.Object;

            controller.ControllerContext = new ControllerContext { HttpContext = context };

            // Act
            var result = controller.IsAuthenticated().Result;

            // Assert
            Assert.AreEqual(result, Constants.User);
        }

        [TestMethod]
        public void Post_PasswordIsNull_ReturnsBadRequest()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);

            // Act
            var result = controller.Post(new UserModel { UserName = "xyz", Password = null }).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_UserNameIsNull_ReturnsBadRequest()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);

            // Act
            var result = controller.Post(new UserModel { UserName = null, Password = "xyz" }).Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Post_WhenCalledWithValidData_CallsCreateReturnsOk()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };

            var contextMock = new Mock<HttpContext>();
            var httpResponseMock = new Mock<HttpResponse>();
            httpResponseMock.Setup(x => x.Cookies).Returns(_cookiesMock.Object);
            contextMock.SetupGet(x => x.Response).Returns(httpResponseMock.Object);

            controller.ControllerContext = new ControllerContext { HttpContext = contextMock.Object };

            // Act
            var result = controller.Post(model).Result;

            // Assert
            _userDataBaseServiceMock.Verify(service => service.Create(It.IsAny<UserModel>()), Times.Once);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void Refresh_WhenCalledWithValidData_ReturnsOk()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();
            
            var contextMock = new Mock<HttpContext>();
            var defaultHttpContext = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            defaultHttpContext.Request.Cookies = cookiesMock.Object;

            contextMock.SetupGet(x => x.Request).Returns(defaultHttpContext.Request);

            var httpResponseMock = new Mock<HttpResponse>();
            httpResponseMock.Setup(x => x.Cookies).Returns(_cookiesMock.Object);
            contextMock.SetupGet(x => x.Response).Returns(httpResponseMock.Object);

            controller.ControllerContext = new ControllerContext { HttpContext = contextMock.Object};

            _ = controller.Post(model).Result;

            cookies.Add(
                new(Constants.AccessToken, new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            );
            cookies.Add(
                new(Constants.RefreshToken, model.RefreshToken!)
            );

            // Act
            var result = controller.Refresh().Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _cookiesMock.Verify(x => x.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Exactly(4));
        }

        [TestMethod]
        public void Refresh_WhenCalledWithInValidData_ReturnsBadRequest()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            var contextMock = new Mock<HttpContext>();
            var defaultHttpContext = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            defaultHttpContext.Request.Cookies = cookiesMock.Object;

            contextMock.SetupGet(x => x.Request).Returns(defaultHttpContext.Request);

            var httpResponseMock = new Mock<HttpResponse>();
            httpResponseMock.Setup(x => x.Cookies).Returns(_cookiesMock.Object);
            contextMock.SetupGet(x => x.Response).Returns(httpResponseMock.Object);

            controller.ControllerContext = new ControllerContext { HttpContext = contextMock.Object };

            _ = controller.Post(model).Result;

            cookies.Add(
                new(Constants.AccessToken, new JwtAuthenticationManager(_userDataBaseServiceMock.Object).Authenticate(new AuthInformation(model.UserName, "xyz")).Result)
            );
            cookies.Add(
                new(Constants.RefreshToken, string.Empty)
            );

            // Act
            var result = controller.Refresh().Result;

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestResult));
            _cookiesMock.Verify(x => x.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Exactly(2));
        }

        [TestMethod]
        public void LogOut_ReturnsOk_AppendsCookies()
        {
            // Arrange
            var controller = new UserController(_userDataBaseServiceMock.Object, _adminDataBaseServiceMock.Object);
            var model = new UserModel { UserName = "xyz", Password = "xyz" };
            model.ID = new BsonObjectIdGenerator().GenerateId(_userDataBaseServiceMock.Object, model).ToString();

            var contextMock = new Mock<HttpContext>();
            var defaultHttpContext = new DefaultHttpContext();

            var cookies = new List<KeyValuePair<string, string>>
            {
            };

            Mock<IRequestCookieCollection> cookiesMock = new Mock<IRequestCookieCollection>();
            cookiesMock.Setup(x => x.GetEnumerator()).Returns(() => cookies.GetEnumerator());

            defaultHttpContext.Request.Cookies = cookiesMock.Object;

            contextMock.SetupGet(x => x.Request).Returns(defaultHttpContext.Request);

            var httpResponseMock = new Mock<HttpResponse>();
            httpResponseMock.Setup(x => x.Cookies).Returns(_cookiesMock.Object);
            contextMock.SetupGet(x => x.Response).Returns(httpResponseMock.Object);

            controller.ControllerContext = new ControllerContext { HttpContext = contextMock.Object };

            // Act
            var result = controller.LogOut();

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            _cookiesMock.Verify(x => x.Append(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CookieOptions>()), Times.Exactly(2));
        }

        #endregion
    }
}