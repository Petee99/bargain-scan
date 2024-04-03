// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserModelTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Models.DataModels
{
    #region Imports

    using WebAPI.Models.DataModels;

    #endregion

    [TestClass]
    public class UserModelTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void ClearSensitiveData_WhenCalled_ClearsPasswordAndId()
        {
            // Arrange
            var userModel = new UserModel { Password = "password", ID = "id" };

            // Act
            userModel.ClearSensitiveData();

            // Assert
            Assert.AreEqual(string.Empty, userModel.Password);
            Assert.AreEqual(string.Empty, userModel.ID);
        }

        [TestMethod]
        public void HashPassword_WhenCalled_HashesPassword()
        {
            // Arrange
            var userModel = new UserModel { Password = "password" };

            // Act
            userModel.HashPassword();

            // Assert
            Assert.AreNotEqual("password", userModel.Password);
        }

        [TestMethod]
        public void IsValidPassword_WhenCalledWithInvalidPassword_ReturnsFalse()
        {
            // Arrange
            var userModel = new UserModel { Password = "password" };
            userModel.HashPassword();
            
            // Act
            var result = userModel.IsValidPassword("invalid");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsValidPassword_WhenCalledWithValidPassword_ReturnsTrue()
        {
            // Arrange
            var userModel = new UserModel { Password = "password" };
            userModel.HashPassword();

            // Act
            var result = userModel.IsValidPassword("password");

            // Assert
            Assert.IsTrue(result);
        }

        #endregion
    }
}