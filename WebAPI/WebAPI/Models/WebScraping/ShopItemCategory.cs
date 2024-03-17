// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItemCategory.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.WebScraping

{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;

    using WebAPI.Properties;

    #region Imports

    using System.Text.Json.Serialization;

    using WebAPI.Interfaces;

    #endregion

    public class ShopItemCategory(string name, string link, string parent) : IShopItemCategory
    {
        #region Public Properties

        public string Parent { get; set; } = parent;

        [JsonIgnore]
        public string Link => link;

        public string Name { get; set; } = name;

        public static string CollectionName => Constants.ShopItemCategoryCollectionName;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}
