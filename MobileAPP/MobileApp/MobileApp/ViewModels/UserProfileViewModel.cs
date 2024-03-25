// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.ComponentModel;
    using System.Windows.Input;

    using CommunityToolkit.Maui.Views;

    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Views;

    #endregion

    public class UserProfileViewModel : PropertyChangedBase
    {
        #region Constants and Private Fields

        private readonly IDataService _dataService;

        private readonly IUserProfile _profile;

        #endregion

        #region Constructors and Destructors

        public UserProfileViewModel(IDataService dataService)
        {
            _profile = dataService.UserProfile;
            _profile.ActiveShoppingCartChanged += OnActiveShoppingCartChanged;
            _profile.ShoppingCartsChanged += OnShoppingCartsChanged;
            _dataService = dataService;
            ActiveShoppingCart = new ShoppingCartViewModel(_profile.ActiveShoppingCart);
            ActiveShoppingCart.PropertyChanged += OnActiveShoppingCartPropertyChanged;
            AddShoppingCartCommand = new Command(() => _profile.CreateShoppingCart());
            ExchangeItemCommand = new Command<IShopItem>(ExchangeItem);
        }

        #endregion

        #region Public Properties

        public ICommand AddShoppingCartCommand { get; }

        public ICommand ExchangeItemCommand { get; }

        public IEnumerable<IShoppingCart> ShoppingCarts => _profile.ShoppingCarts;

        public ShoppingCartViewModel ActiveShoppingCart { get; set; }

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

        private async void ExchangeItem(IShopItem item)
        {
            var popupView = new BarcodePopupView
            {
                BindingContext = new BarcodePopupViewModel(_dataService)
            };

            await Shell.Current.CurrentPage.ShowPopupAsync(popupView);

            if (popupView.BindingContext is not BarcodePopupViewModel { SelectedItem: not null } viewModel)
            {
                return;
            }

            int index = ActiveShoppingCart.Items.IndexOf(item);
            ActiveShoppingCart.Items.RemoveAt(index);
            ActiveShoppingCart.Items.Insert(index, viewModel.SelectedItem);
        }

        private void OnActiveShoppingCartChanged(object sender, EventArgs e)
        {
            if (sender is not IUserProfile)
            {
                return;
            }

            ActiveShoppingCart = new ShoppingCartViewModel(_profile.ActiveShoppingCart);
            OnPropertyChanged(nameof(ActiveShoppingCart));
        }

        private void OnActiveShoppingCartPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(nameof(ActiveShoppingCart));
        }

        private void OnShoppingCartsChanged(object sender, EventArgs e)
        {
            if (sender is IUserProfile)
            {
                OnPropertyChanged(nameof(ShoppingCarts));
            }
        }

        #endregion
    }
}