// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdminModel .cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.DataModels
{
    #region Imports

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using WebAPI.Interfaces;
    using WebAPI.Properties;

    #endregion

    public class AdminModel : IDataModel
    {
        #region Public Properties

        public static string CollectionName => Constants.AdminCollectionName;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}