// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MongoDbService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports

    using MongoDB.Driver;

    using WebAPI.Interfaces;
    using WebAPI.Properties;

    #endregion

    public class MongoDbService : IMongoDbService
    {
        #region Constants and Private Fields

        private IMongoDatabase? _mongoDatabase;

        #endregion

        #region Public Methods and Operators

        public IMongoDatabase GetDatabase()
        {
            var settings = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable(Constants.MongoDbKeyVariable));
            var client = new MongoClient(settings);

            return _mongoDatabase ??= client.GetDatabase(Environment.GetEnvironmentVariable(Constants.DatabaseNameVariable));
        }

        #endregion
    }
}