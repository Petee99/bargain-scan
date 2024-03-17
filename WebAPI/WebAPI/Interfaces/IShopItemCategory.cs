// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShopItemCategory.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;
    using System.Text.Json.Serialization;

    public interface IShopItemCategory
    {
        #region Public Properties

        string Parent { get; set; }

        [JsonIgnore]
        string Link { get; }

        string Name { get; set; }

        public static string CollectionName { get; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}