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

        private IShoppingCart _activeShoppingCart;

        #endregion

        #region Public Properties

        public event EventHandler ActiveShoppingCartChanged;

        public event EventHandler ShoppingCartsChanged;

        public IEnumerable<IShoppingCart> ShoppingCarts => _shoppingCarts;

        public IShoppingCart ActiveShoppingCart => _activeShoppingCart ??= GetDefaultShoppingCart();

        public string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        public bool ActivateShoppingCart(IShoppingCart shoppingCart)
        {
            if (!_shoppingCarts.Contains(shoppingCart))
            {
                return false;
            }

            _activeShoppingCart = shoppingCart;
            ActiveShoppingCartChanged?.Invoke(this, EventArgs.Empty);
            return true;
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

        public bool RemoveShoppingCart(IShoppingCart shoppingCart)
        {
            return _shoppingCarts.Remove(shoppingCart);
        }

        public IShoppingCart CreateShoppingCart()
        {
            var cart = new ShoppingCart { Name = "Kosár#" + _shoppingCarts.Count };
            AddShoppingCart(cart);
            return cart;
        }

        #endregion

        #region Private Methods

        private IShoppingCart GetDefaultShoppingCart()
        {
            return ShoppingCarts.FirstOrDefault() ?? CreateShoppingCart();
        }

        #endregion
    }
}