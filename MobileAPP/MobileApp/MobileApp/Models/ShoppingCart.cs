// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCart.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using MobileApp.Interfaces;

    #endregion

    internal class ShoppingCart : IShoppingCart
    {
        #region Constants and Private Fields

        private readonly List<IShopItem> _items = new();

        #endregion

        #region Public Properties

        public Guid ShoppingCartId { get; private set; }

        public double Total => CalculateSum();

        public IEnumerable<IShopItem> Items => _items;

        public string Description { get; set; }

        public string Name { get; set; }

        #endregion
        
        #region Public Methods and Operators

        public bool AddItem(IShopItem item)
        {
            if (_items.Contains(item))
            {
                return false;
            }

            _items.Add(item);
            return true;
        }

        public bool RemoveItem(IShopItem item)
        {
            return _items.Remove(item);
        }

        #endregion

        #region Private Methods

        private double CalculateSum()
        {
            double sum = 0;
            _items.ForEach(i => sum += double.Parse(i.Price));
            return sum;
        }

        #endregion
    }
}