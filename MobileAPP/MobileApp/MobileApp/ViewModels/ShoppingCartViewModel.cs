// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using MobileApp.Interfaces;
    using MobileApp.Models;

    #endregion

    public class ShoppingCartViewModel : PropertyChangedBase
    {
        #region Constants and Private Fields

        private readonly IShoppingCart _shoppingCart;

        #endregion

        #region Constructors and Destructors

        public ShoppingCartViewModel(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        #endregion

        #region Public Properties

        public double Total => _shoppingCart.Total;

        public IEnumerable<IShopItem> Items => _shoppingCart.Items;

        public string Description
        {
            get => _shoppingCart.Description;
            set
            {
                _shoppingCart.Description = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _shoppingCart.Name;
            set
            {
                _shoppingCart.Name = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods and Operators

        public bool AddItem(IShopItem item)
        {
            if (!_shoppingCart.AddItem(item))
            {
                return false;
            }

            FireItemsChanged();
            return true;
        }

        public bool RemoveItem(IShopItem item)
        {
            if (!_shoppingCart.RemoveItem(item))
            {
                return false;
            }

            FireItemsChanged();
            return true;
        }

        #endregion

        #region Private Methods

        private void FireItemsChanged()
        {
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Total));
        }

        #endregion
    }
}