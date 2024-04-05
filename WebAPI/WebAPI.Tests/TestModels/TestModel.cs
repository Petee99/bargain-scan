// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Tests.TestModels
{
    #region Imports

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using WebAPI.Interfaces;

    #endregion

    public class TestModel : IDataModel
    {
        #region Public Properties

        public static string CollectionName => nameof(TestModel);
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}