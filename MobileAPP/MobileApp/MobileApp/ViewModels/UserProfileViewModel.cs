// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using MobileApp.Interfaces;
    using MobileApp.Models;

    #endregion

    public class UserProfileViewModel : PropertyChangedBase
    {
        #region Constants and Private Fields

        private readonly IUserProfile _profile;

        private IShoppingCart _activeShoppingCart;

        #endregion

        #region Constructors and Destructors

        public UserProfileViewModel(IUserProfile profile)
        {
            _profile = profile;
        }

        #endregion

        #region Public Properties

        public IEnumerable<IShoppingCart> ShoppingCarts => _profile.ShoppingCarts;

        public IShoppingCart ActiveShoppingCart => _activeShoppingCart ??= GetDefaultShoppingCart();

        public string Name
        {
            get => _profile.Name;
            set
            {
                _profile.Name = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private Methods

        private IShoppingCart GetDefaultShoppingCart()
        {
            return _profile.ShoppingCarts.FirstOrDefault() ?? _profile.CreateShoppingCart();
        }

        #endregion
    }
}