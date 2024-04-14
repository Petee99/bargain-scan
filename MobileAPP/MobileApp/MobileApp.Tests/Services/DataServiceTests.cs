// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataServiceTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Services
{
    #region Imports

    using System.Text.Json;

    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Services;
    using MobileApp.Tests.Mocks;

    using Moq;

    #endregion

    [TestClass]
    public class DataServiceTests
    {
        #region Constants and Private Fields

        private const string ApiStorageShopItemsJson =
            """
            [{"shop":1,"categoryName":"Kenyérfélék","name":"Jókenyér Rozsos cipó","price":"529 Ft","subCategoryName":"Kenyér","category":null,"id":"65uf8fwv8vwffw8ev"},
            {"shop":2,"categoryName":"Kenyérfélék","name":"Vizes zsemle","price":"35 Ft","subCategoryName":"Zsemlefélék","category":null,"id":"65fw78w8fwbfuiwbui"},
            {"shop":3,"categoryName":"Kenyérfélék","name":"Vizse kifli","price":"25 Ft","subCategoryName":"Kiflik","category":null,"id":"897fwgbfw7ghbfd"}]
            """;

        private const string LocalStorageShopItemsJson =
            """
            [{"shop":1,"categoryName":"Tejtermék, tojás","name":"Sissy ESL félzsíros tej 2,8% 1 l","price":"329 Ft","subCategoryName":"Tejszín","category":null,"id":"65509f79f17c61a3afdbb2d5"},
            {"shop":2,"categoryName":"Tejtermék, tojás","name":"Sissy UHT félzsíros, laktózmentes tej 2,8% 1 l","price":"315 Ft","subCategoryName":"Tejszín","category":null,"id":"65509f79f17c61a3afdbb2d6"},
            {"shop":3,"categoryName":"Tejtermék, tojás","name":"My Bio BIO zabból készült ital 1 l","price":"499 Ft","subCategoryName":"Tejszín","category":null,"id":"65509f79f17c61a3afdbb2d7"}]
            """;

        private readonly Mock<IDataPersistenceService> _dataPersistenceServiceMock = ServiceMockProvider.GetDataPersistenceServiceMock();

        private readonly Mock<IOnlineDataService> _onlineDataServiceMock = ServiceMockProvider.GetOnlineDataServiceMock();

        #endregion

        #region Public Methods and Operators

        [TestInitialize]
        public void Initialize()
        {
            var userProfile = new LocalUserProfile
            {
                Name = "Test"
            };

            _onlineDataServiceMock.Setup(x => x.LoadShopItemsFromApi()).Returns(Task.FromResult(string.Empty));
            _dataPersistenceServiceMock.Setup(x => x.TryLoadLocalData(DataPersistenceService.ItemsFileName))
                .Returns(Task.FromResult(LocalStorageShopItemsJson));
            _dataPersistenceServiceMock.Setup(x => x.TryLoadLocalData(DataPersistenceService.UserProfileFileName))
                .Returns(Task.FromResult(JsonSerializer.Serialize(userProfile)));
        }

        [TestMethod]
        public async Task GetMainCategories_DataIsFromApi_ReturnCategoriesFromApiJson()
        {
            // Arrange
            _onlineDataServiceMock.Setup(x => x.LoadShopItemsFromApi()).Returns(Task.FromResult(ApiStorageShopItemsJson));
            _dataPersistenceServiceMock.Setup(x => x.TryLoadLocalData(DataPersistenceService.ItemsFileName)).Returns(Task.FromResult(string.Empty));
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetMainCategories();

            // Assert
            Assert.AreEqual("Kenyérfélék", result.First().Title);
        }

        [TestMethod]
        public async Task GetMainCategories_DataIsFromLocalStorage_ReturnCategoriesFromLocalStorageJson()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetMainCategories();

            // Assert
            Assert.AreEqual("Tejtermék, tojás", result.First().Title);
        }

        [TestMethod]
        public async Task GetShopItems_DataIsFromApi_ReturnShopItemsFromApiJson()
        {
            // Arrange
            _onlineDataServiceMock.Setup(x => x.LoadShopItemsFromApi()).Returns(Task.FromResult(ApiStorageShopItemsJson));
            _dataPersistenceServiceMock.Setup(x => x.TryLoadLocalData(DataPersistenceService.ItemsFileName)).Returns(Task.FromResult(string.Empty));
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetShopItems(dataService.GetMainCategories().Result.First());

            // Assert
            Assert.AreEqual("Jókenyér Rozsos cipó", result.First().Name);
        }

        [TestMethod]
        public async Task GetShopItems_DataIsFromLocalStorage_ReturnShopItemsFromLocalStorageJson()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetShopItems(dataService.GetMainCategories().Result.First());

            // Assert
            Assert.AreEqual("Sissy ESL félzsíros tej 2,8% 1 l", result.First().Name);
        }

        [TestMethod]
        public async Task GetShopItems_SubCategoryDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetShopItems(new SubCategory("Test", new Category("Test")));

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task GetShopItems_SubCategoryExists_ReturnsItems()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetShopItems(dataService.GetMainCategories().Result.First().SubCategories.First());

            // Assert
            Assert.AreEqual("Sissy ESL félzsíros tej 2,8% 1 l", result.First().Name);
        }

        [TestMethod]
        public async Task GetShopItemsByBarcode_ItemDoesNotExist_ReturnsEmptyList()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetShopItemsByBarcode("1111");

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task GetShopItemsByBarcode_ItemExists_ReturnsItem()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);
            var items = (await dataService.GetShopItems(dataService.GetMainCategories().Result.First())).ToList();
            dataService.TryUpdateBarCode(items.First(), "1111");

            // Act
            var result = await dataService.GetShopItemsByBarcode("1111");

            // Assert
            Assert.AreEqual(items.First(), result.First());
        }

        [TestMethod]
        public async Task GetShopItemsBySearchTerm_SearchTermDoesNotMatchItem_ReturnsEmptyList()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);

            // Act
            var result = await dataService.GetShopItemsBySearchTerm("Test");

            // Assert
            Assert.IsFalse(result.Any());
        }

        [TestMethod]
        public async Task GetShopItemsBySearchTerm_SearchTermMatchesItem_ReturnsItem()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);
            var items = (await dataService.GetShopItems(dataService.GetMainCategories().Result.First())).ToList();
            var searchTerm = items.First().Name;

            // Act
            var result = await dataService.GetShopItemsBySearchTerm(searchTerm);

            // Assert
            Assert.AreEqual(items.First(), result.First());
        }

        [TestMethod]
        public async Task SaveApplicationData_PersistenceServiceSavesData()
        {
            // Arrange
            _dataPersistenceServiceMock.Setup(x => x.TryLoadLocalData(DataPersistenceService.UserProfileFileName))
                .Returns(Task.FromResult(string.Empty));
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);
            var items = (await dataService.GetShopItems(dataService.GetMainCategories().Result.First())).ToList();
            dataService.UserProfile.Name = "Test";
            dataService.UserProfile.ActiveShoppingCart.AddItem(items.First());
            string itemData = JsonSerializer.Serialize(items);
            string userProfileData = JsonSerializer.Serialize(dataService.UserProfile);

            // Act
            dataService.SaveApplicationData();

            // Assert

            await Task.Delay(1000);
            _dataPersistenceServiceMock.Verify(x => x.TrySaveLocalData(DataPersistenceService.ItemsFileName, itemData), Times.Once);
            _dataPersistenceServiceMock.Verify(x => x.TrySaveLocalData(DataPersistenceService.UserProfileFileName,
                It.Is<string>(s => s.Contains(userProfileData))), Times.Once);
        }

        [TestMethod]
        public void TryUpdateBarCode_ItemDoesNotHaveBarcode_BarcodeIsSet()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);
            var item = dataService.GetShopItems(dataService.GetMainCategories().Result.First()).Result.First();

            // Act
            dataService.TryUpdateBarCode(item, "1111");

            // Assert
            Assert.AreEqual("1111", item.BarCode);
        }

        [TestMethod]
        public void TryUpdateBarCode_ItemHasBarcode_BarcodeIsNotSet()
        {
            // Arrange
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);
            var item = dataService.GetShopItems(dataService.GetMainCategories().Result.First()).Result.First();
            dataService.TryUpdateBarCode(item, "1111");

            // Act
            dataService.TryUpdateBarCode(item, "2222");

            // Assert
            Assert.AreEqual("1111", item.BarCode);
        }

        [TestMethod]
        public void UserProfile_NotStoredLocally_NewOneGetsCreated()
        {
            // Arrange
            _dataPersistenceServiceMock.Setup(x => x.TryLoadLocalData(DataPersistenceService.UserProfileFileName))
                .Returns(Task.FromResult(string.Empty));
            var dataService = new DataService(_onlineDataServiceMock.Object, _dataPersistenceServiceMock.Object);
            dataService.GetMainCategories().Wait();

            // Assert
            Assert.AreEqual(null, dataService.UserProfile.Name);
        }

        #endregion
    }
}