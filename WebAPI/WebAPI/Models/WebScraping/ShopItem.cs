// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItem.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Models.WebScraping

{
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Bson;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    using WebAPI.Properties;

    #region Imports

    using WebAPI.Enums;
    using WebAPI.Interfaces;
    #endregion

    public class ShopItem(string name, IShopItemCategory? category, string price, Shop shop) : IShopItem
    {
        private string categoryName = String.Empty;
        private string subCategoryName = String.Empty;


        #region Public Properties

        [BsonRepresentation(BsonType.String)]
        [JsonConverter(typeof(StringEnumConverter))]
        public Shop Shop { get; set; } = shop;
        
        [BsonRepresentation(BsonType.String)]
        [BsonElement("Category")]
        public string CategoryName
        {
            get => Category?.Parent ?? categoryName;
            set => categoryName = value;
        }

        public string Name { get; set; } = name;

        public string Price { get; set; } = price;

        [BsonRepresentation(BsonType.String)]
        [BsonElement("SubCategory")]
        public string SubCategoryName
        {
            get => Category?.Name ?? subCategoryName;
            set => subCategoryName = value;
        }

        [BsonIgnore]
        public IShopItemCategory Category { get; set; } = category;

        [BsonIgnore]
        public static string CollectionName => Constants.ShopItemCollectionName;

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ID { get; set; }

        #endregion
    }
}