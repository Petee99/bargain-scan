// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShopItem.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using WebAPI.Enums;

namespace WebAPI.Interfaces
{
    using System.Text.Json.Serialization;

    public interface IShopItem
    {
        public string Name { get; set; }

        public Shop Shop { get; set; }

        [JsonIgnore]
        public IShopItemCategory Category { get; set; }

        [JsonIgnore]
        public string Price { get; set; }

        public string SubCategoryName { get; }

        public string CategoryName { get; }
    }
}