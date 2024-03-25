// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IUserProfile.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IUserProfile
    {
        #region Public Properties

        event EventHandler ActiveShoppingCartChanged;

        event EventHandler ShoppingCartsChanged;

        IEnumerable<IShoppingCart> ShoppingCarts { get; }

        public IShoppingCart ActiveShoppingCart { get; }

        string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        bool ActivateShoppingCart(IShoppingCart shoppingCart);

        bool RemoveShoppingCart(IShoppingCart shoppingCart);

        IShoppingCart CreateShoppingCart();

        #endregion
    }
}