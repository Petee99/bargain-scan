// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ServiceMockProvider.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Mocks
{
    #region Imports

    using MobileApp.Interfaces;

    using Moq;

    #endregion

    public class ServiceMockProvider
    {
        #region Public Methods and Operators

        public static Mock<IDataPersistenceService> GetDataPersistenceServiceMock()
        {
            Mock<IDataPersistenceService> dataPersistenceServiceMock = new();
            return dataPersistenceServiceMock;
        }

        public static Mock<IOnlineDataService> GetOnlineDataServiceMock()
        {
            Mock<IOnlineDataService> onlineDataServiceMock = new();
            return onlineDataServiceMock;
        }

        public static Mock<IDataService> GetDataServiceMock()
        {
            Mock<IDataService> dataServiceMock = new();
            dataServiceMock.Setup(x => x.GetMainCategories())
                .Returns(Task.FromResult<IEnumerable<ICategory>>(Array.Empty<ICategory>()));
            dataServiceMock.Setup(x => x.GetShopItems(It.IsAny<ICategory>()))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(Array.Empty<IShopItem>()));
            dataServiceMock.Setup(x => x.GetShopItemsByBarcode(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<IShopItem>>(Array.Empty<IShopItem>()));
            return dataServiceMock;
        }

        public static Mock<IEventAggregator> GetEventAggregatorMock()
        {
            Mock<IEventAggregator> eventAggregatorMock = new();
            return eventAggregatorMock;
        }

        public static Mock<IResolverService> GetResolverServiceMock()
        {
            Mock<IResolverService> resolverServiceMock = new();
            return resolverServiceMock;
        }

        #endregion
    }
}