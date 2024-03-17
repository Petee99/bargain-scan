// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShopItem.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    #region Imports

    using MobileApp.Enums;

    #endregion

    public interface IShopItem
    {
        #region Public Properties
        
        Shop Shop { get; set; }

        string Category { get; set; }

        string IconPath { get; set; }

        string Name { get; set; }

        string Price { get; set; }

        string ShopIconPath { get; }

        string SubCategory { get; set; }

        #endregion
    }
}