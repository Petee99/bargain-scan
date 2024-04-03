// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItem.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.WebScraping
{
    #region Imports

    #region Imports

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using WebAPI.Enums;
    using WebAPI.Interfaces;
    using WebAPI.Properties;

    #endregion

    #endregion

    public class ShopItem(string name, IShopItemCategory? category, string price, Shop shop) : IShopItem
    {
        #region Constants and Private Fields

        private string _categoryName = string.Empty;

        private string _subCategoryName = string.Empty;

        #endregion

        #region Public Properties

        [BsonIgnore]
        public IShopItemCategory Category { get; set; } = category!;

        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Shop Shop { get; set; } = shop;

        [BsonRepresentation(BsonType.String)]
        [BsonElement("Category")]
        public string CategoryName
        {
            get => Category?.Parent ?? _categoryName;
            set => _categoryName = value;
        }

        [BsonIgnore]
        public static string CollectionName => Constants.ShopItemCollectionName;

        public string Name { get; set; } = name;

        public string Price { get; set; } = price;

        [BsonRepresentation(BsonType.String)]
        [BsonElement("SubCategory")]
        public string SubCategoryName
        {
            get => Category?.Name ?? _subCategoryName;
            set => _subCategoryName = value;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}