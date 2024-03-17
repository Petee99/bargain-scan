// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

#region Imports

#endregion


namespace WebAPI.Interfaces

{
    #region Imports

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    #endregion

    public interface IDataModel
    {
        #region Public Properties

        public static string? CollectionName { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}