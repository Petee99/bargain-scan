// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DataBaseService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Services
{
    #region Imports

    using MongoDB.Bson;
    using MongoDB.Driver;

    using WebAPI.Interfaces;
    using WebAPI.Properties;

    #endregion

    public class DataBaseService<T> : IDataService<T>
    {
        #region Public Methods and Operators

        public Task Create(T model)
        {
            IMongoCollection<T> collection = GetCollection();

            return collection.InsertOneAsync(model);
        }

        public Task CreateMany(IEnumerable<T> models)
        {
            IMongoCollection<T> collection = GetCollection();

            return collection.InsertManyAsync(models);
        }

        public Task Delete(string id)
        {
            IMongoCollection<T> collection = GetCollection();

            FilterDefinition<T> filter =
                Builders<T>.Filter.Eq(Constants.IdPropertyString, new BsonObjectId(new ObjectId(id)));

            return collection.DeleteOneAsync(filter);
        }

        public Task Update(T model)
        {
            IMongoCollection<T> collection = GetCollection();

            FilterDefinition<T> filter = GetIdFilter(model);

            return collection.ReplaceOneAsync(filter, model, new ReplaceOptions { IsUpsert = true });
        }

        public async Task<List<T>> GetAll()
        {
            IMongoCollection<T> collection = GetCollection();
            IAsyncCursor<T> results = await collection.FindAsync(_ => true);

            return results.ToList();
        }

        public async Task<T?> GetById(string id)
        {
            IMongoCollection<T> collection = GetCollection();

            IAsyncCursor<T> results =
                await collection.FindAsync(Builders<T>.Filter.Eq(Constants.IdPropertyString,
                    new BsonObjectId(new ObjectId(id))));

            var resultList = results.ToList();

            if (resultList.Count < 1) return default;

            return resultList.First();
        }

        public async Task<T> GetById(T model)
        {
            IMongoCollection<T> collection = GetCollection();

            FilterDefinition<T> filter = GetIdFilter(model);

            IAsyncCursor<T> results = await collection.FindAsync(filter);

            return results.ToList().First();
        }

        #endregion

        #region Private Methods

        private static FilterDefinition<T> GetIdFilter(T model)
        {
            string id = string.Empty;

            if (model is IDataModel dataModel) id = dataModel.ID ?? id;

            return Builders<T>.Filter.Eq(Constants.IdPropertyString, new BsonObjectId(new ObjectId(id)));
        }

        private static IMongoCollection<T> GetCollection()
        {
            var settings = MongoClientSettings.FromConnectionString(Constants.ConnectionString);
            var client = new MongoClient(settings);

            IMongoDatabase db = client.GetDatabase(Constants.DatabaseName);

            string? collectionName = typeof(T).GetProperty(Constants.CollectionPropertyString)?.GetValue(null)?.ToString();

            return db.GetCollection<T>(collectionName);
        }

        #endregion
    }
}