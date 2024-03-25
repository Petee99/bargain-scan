// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IShoppingCart.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    #region Imports

    using MobileApp.Events;

    #endregion

    public interface IShoppingCart
    {
        #region Public Properties

        public double Total { get; }

        event EventHandler<ItemsChangedEventArgs> ItemsChanged;

        public Guid ShoppingCartId { get; }

        public IEnumerable<IShopItem> Items { get; }

        public string Description { get; set; }

        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        bool AddItem(IShopItem item);

        bool RemoveItem(IShopItem item);

        #endregion
    }
}