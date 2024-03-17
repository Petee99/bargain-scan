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

        IEnumerable<IShoppingCart> ShoppingCarts { get; }

        string Name { get; set; }

        #endregion

        #region Public Methods and Operators

        bool RemoveShoppingCart(IShoppingCart shoppingCart);

        IShoppingCart CreateShoppingCart();

        #endregion
    }
}