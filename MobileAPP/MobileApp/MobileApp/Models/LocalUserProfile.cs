// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalUserProfile.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using MobileApp.Interfaces;

    #endregion

    internal class LocalUserProfile : IUserProfile
    {
        #region Constants and Private Fields

        private readonly List<IShoppingCart> _shoppingCarts = new();

        #endregion

        #region Public Properties

        public IEnumerable<IShoppingCart> ShoppingCarts => _shoppingCarts;

        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        public bool RemoveShoppingCart(IShoppingCart shoppingCart)
        {
            return _shoppingCarts.Remove(shoppingCart);
        }

        public IShoppingCart CreateShoppingCart()
        {
            var cart = new ShoppingCart();
            AddShoppingCart(cart);
            return cart;
        }

        public bool AddShoppingCart(IShoppingCart shoppingCart)
        {
            if (_shoppingCarts.Contains(shoppingCart))
            {
                return false;
            }

            _shoppingCarts.Add(shoppingCart);
            return true;
        }

        #endregion
    }
}