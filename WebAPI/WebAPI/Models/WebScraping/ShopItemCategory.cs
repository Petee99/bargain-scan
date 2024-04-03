// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItemCategory.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.WebScraping
{
    #region Imports

    using System.Text.Json.Serialization;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using WebAPI.Interfaces;
    using WebAPI.Properties;

    #endregion

    public class ShopItemCategory(string name, string link, string parent) : IShopItemCategory
    {
        #region Public Properties

        public static string CollectionName => Constants.ShopItemCategoryCollectionName;

        [JsonIgnore]
        public string Link => link;

        public string Name { get; set; } = name;

        public string Parent { get; set; } = parent;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}