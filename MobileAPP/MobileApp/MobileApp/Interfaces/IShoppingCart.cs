// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShoppingCart.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;

namespace MobileApp.Interfaces
{
    public interface IShoppingCart
    {
        bool AddItem(IShopItem item);

        bool RemoveItem(IShopItem item);

        public Guid ShoppingCartId { get; }

        public double Total { get; }

        public IEnumerable<IShopItem> Items { get; }

        public string Description { get; set; }

        public string Name { get; set; }

    }
}