// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShopItem.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace WebAPI.Interfaces
{
    #region Imports

    using System.Text.Json.Serialization;

    using WebAPI.Enums;

    #endregion

    public interface IShopItem
    {
        #region Public Properties

        [JsonIgnore]
        public IShopItemCategory Category { get; set; }

        public Shop Shop { get; set; }

        public string CategoryName { get; }
        
        public string Name { get; set; }

        [JsonIgnore]
        public string Price { get; set; }

        public string SubCategoryName { get; }

        #endregion
    }
}