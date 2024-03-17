// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShopItem.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using System.Text.Json.Serialization;

    using MobileApp.Enums;
    using MobileApp.Interfaces;

    #endregion

    public class ShopItem : IShopItem
    {
        #region Public Properties
        
        [JsonPropertyName("shop")]
        public Shop Shop { get; set; }

        [JsonPropertyName("categoryName")]
        public string Category { get; set; }

        public string IconPath { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("price")]
        public string Price { get; set; }

        public string ShopIconPath => $"{Shop.ToString().ToLower()}.png";

        [JsonPropertyName("subCategoryName")]
        public string SubCategory { get; set; }
        
        #endregion
    }
}