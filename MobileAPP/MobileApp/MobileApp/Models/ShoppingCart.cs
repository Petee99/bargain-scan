// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCart.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using System.Text.RegularExpressions;

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;

    #endregion

    internal class ShoppingCart : IShoppingCart
    {
        #region Constants and Private Fields

        private readonly List<IShopItem> _items = new();

        #endregion

        #region Public Properties

        public double Total => CalculateSum();

        public event EventHandler<ItemsChangedEventArgs> ItemsChanged;

        public Guid ShoppingCartId { get; }

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
            ItemsChanged?.Invoke(this, new ItemsChangedEventArgs(EventType.ItemAdded, item));
            return true;
        }

        public bool RemoveItem(IShopItem item)
        {
            if (!_items.Contains(item))
            {
                return false;
            }

            _items.Remove(item);
            ItemsChanged?.Invoke(this, new ItemsChangedEventArgs(EventType.ItemRemoved, item));
            return true;
        }

        #endregion

        #region Private Methods

        private double CalculateSum()
        {
            double sum = 0;
            _items.ForEach(i => sum += double.Parse(Regex.Replace(i.Price, @"\D", "")));
            return sum;
        }

        #endregion
    }
}