// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBaseServiceTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.Models.Services
{
    #region Imports

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson.Serialization.IdGenerators;
    using MongoDB.Driver;

    using Moq;

    using WebAPI.Interfaces;
    using WebAPI.Services;
    using WebAPI.Tests.TestModels;

    #endregion

    [TestClass]
    public class DataBaseServiceTests
    {
        #region Constants and Private Fields

        private readonly Mock<IAsyncCursor<TestModel>> _cursorMock = new();

        private readonly Mock<IMongoCollection<TestModel>> _mongoCollectionMock = new();

        private readonly Mock<IMongoDatabase> _mongoDatabaseMock = new();

        private readonly Mock<IMongoDbService> _mongoDbServiceMock = new();

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Initialize()
        {
            _mongoDbServiceMock.Setup(x => x.GetDatabase()).Returns(_mongoDatabaseMock.Object);
            _mongoDatabaseMock.Setup(x => x.GetCollection<TestModel>(TestModel.CollectionName, It.IsAny<MongoCollectionSettings>()))
                .Returns(_mongoCollectionMock.Object);

            _cursorMock.Setup(x => x.MoveNextAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _cursorMock.Setup(x => x.Current).Returns(new List<TestModel> { new(), new() });

            _mongoCollectionMock.Setup(
                x => x.FindAsync(It.IsAny<FilterDefinition<TestModel>>(), It.IsAny<FindOptions<TestModel, TestModel>>(),
                    It.IsAny<CancellationToken>())).Returns(Task.FromResult(_cursorMock.Object));
        }

        [TestMethod]
        public void CreateManyTest()
        {
            // Arrange
            var dataBaseService = new DataBaseService<TestModel>(_mongoDbServiceMock.Object);
            var models = new List<TestModel> { new(), new() };

            // Act
            dataBaseService.CreateMany(models);

            // Assert
            _mongoDatabaseMock.Verify(x => x.GetCollection<TestModel>(TestModel.CollectionName, It.IsAny<MongoCollectionSettings>()));
            _mongoCollectionMock.Verify(x => x.InsertManyAsync(models, It.IsAny<InsertManyOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void CreateTest()
        {
            // Arrange
            var dataBaseService = new DataBaseService<TestModel>(_mongoDbServiceMock.Object);
            var model = new TestModel();

            // Act
            dataBaseService.Create(model);

            // Assert
            _mongoDatabaseMock.Verify(x => x.GetCollection<TestModel>(TestModel.CollectionName, It.IsAny<MongoCollectionSettings>()));
            _mongoCollectionMock.Verify(x => x.InsertOneAsync(model, It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void DeleteTest()
        {
            // Arrange
            var dataBaseService = new DataBaseService<TestModel>(_mongoDbServiceMock.Object);
            var id = new BsonObjectIdGenerator().GenerateId(_mongoCollectionMock.Object, _cursorMock.Object.Current.First());

            // Act
            dataBaseService.Delete(id.ToString()!);

            // Assert
            _mongoDatabaseMock.Verify(x => x.GetCollection<TestModel>(TestModel.CollectionName, It.IsAny<MongoCollectionSettings>()));
            _mongoCollectionMock.Verify(x => x.DeleteOneAsync(It.IsAny<FilterDefinition<TestModel>>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void GetAllTest()
        {
            // Arrange
            var dataBaseService = new DataBaseService<TestModel>(_mongoDbServiceMock.Object);

            // Act
            var results = dataBaseService.GetAll();

            // Assert
            _mongoDatabaseMock.Verify(x => x.GetCollection<TestModel>(TestModel.CollectionName, It.IsAny<MongoCollectionSettings>()));
            _mongoCollectionMock.Verify(
                x => x.FindAsync(It.IsAny<FilterDefinition<TestModel>>(), It.IsAny<FindOptions<TestModel, TestModel>>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void GetByIdTest()
        {
            // Arrange
            var dataBaseService = new DataBaseService<TestModel>(_mongoDbServiceMock.Object);
            var id = new BsonObjectIdGenerator().GenerateId(_mongoCollectionMock.Object, _cursorMock.Object.Current.First());

            // Act
            var results = dataBaseService.GetById(id.ToString()!);

            // Assert
            _mongoDatabaseMock.Verify(x => x.GetCollection<TestModel>(TestModel.CollectionName, It.IsAny<MongoCollectionSettings>()));
            _mongoCollectionMock.Verify(
                x => x.FindAsync(It.IsAny<FilterDefinition<TestModel>>(), It.IsAny<FindOptions<TestModel, TestModel>>(),
                    It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestMethod]
        public void UpdateTest()
        {
            // Arrange
            var dataBaseService = new DataBaseService<TestModel>(_mongoDbServiceMock.Object);
            var model = new TestModel();

            // Act
            dataBaseService.Update(model);

            // Assert
            _mongoDatabaseMock.Verify(x => x.GetCollection<TestModel>(TestModel.CollectionName, It.IsAny<MongoCollectionSettings>()));
            _mongoCollectionMock.Verify(
                x => x.ReplaceOneAsync(It.IsAny<FilterDefinition<TestModel>>(), model, It.IsAny<ReplaceOptions>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        #endregion
    }
}