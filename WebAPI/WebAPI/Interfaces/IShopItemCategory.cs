// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShopItemCategory.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    #region Imports

    using System.Text.Json.Serialization;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    #endregion

    public interface IShopItemCategory
    {
        #region Public Properties

        public static string CollectionName { get; } = null!;

        [JsonIgnore]
        string Link { get; }

        string Name { get; set; }

        string Parent { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}