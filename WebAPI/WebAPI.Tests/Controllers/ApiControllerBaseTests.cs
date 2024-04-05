// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApiControllerBaseTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Controllers
{
    #region Imports

    using Moq;

    using WebAPI.Controllers;
    using WebAPI.Interfaces;
    using WebAPI.Tests.TestModels;

    #endregion

    [TestClass]
    public class ApiControllerBaseTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void Delete_DataServiceDeleteCalled()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<TestModel>>();
            var controller = new ApiControllerBase<TestModel>(dataService.Object);

            // Act
            var result = controller.Delete("id").Result;

            // Assert
            dataService.Verify(service => service.Delete("id"), Times.Once);
        }

        [TestMethod]
        public void Get_DataServiceGetAllCalled()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<TestModel>>();
            var controller = new ApiControllerBase<TestModel>(dataService.Object);

            // Act
            var result = controller.Get().Result;

            // Assert
            dataService.Verify(service => service.GetAll(), Times.Once);
        }

        [TestMethod]
        public void Post_DataServiceCreateCalled()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<TestModel>>();
            var controller = new ApiControllerBase<TestModel>(dataService.Object);
            var item = new TestModel();

            // Act
            var result = controller.Post(item).Result;

            // Assert
            dataService.Verify(service => service.Create(item), Times.Once);
        }

        [TestMethod]
        public void Put_DataServiceUpdateCalled()
        {
            // Arrange
            var dataService = new Mock<IDataBaseService<TestModel>>();
            var controller = new ApiControllerBase<TestModel>(dataService.Object);
            var item = new TestModel();

            // Act
            var result = controller.Put(item).Result;

            // Assert
            dataService.Verify(service => service.Update(item), Times.Once);
            dataService.Verify(service => service.GetByModel(item), Times.Once);
        }

        #endregion
    }
}