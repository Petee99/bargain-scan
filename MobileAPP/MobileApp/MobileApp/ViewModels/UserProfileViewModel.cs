// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserProfileViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    using CommunityToolkit.Maui.Alerts;
    using CommunityToolkit.Maui.Core.Extensions;
    using CommunityToolkit.Maui.Views;

    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Properties;
    using MobileApp.Views;

    #endregion

    public class UserProfileViewModel : PropertyChangedBase
    {
        #region Constants and Private Fields

        private readonly IDataService _dataService;

        private readonly IUserProfile _profile;

        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors and Destructors

        public UserProfileViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _profile = dataService.UserProfile;
            _profile.ActiveShoppingCartChanged += OnActiveShoppingCartChanged;
            _profile.ShoppingCartsChanged += OnShoppingCartsChanged;
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            ActiveShoppingCart = new ShoppingCartViewModel(_profile.ActiveShoppingCart);
            ActiveShoppingCart.PropertyChanged += OnActiveShoppingCartPropertyChanged;
            SubstituteItemCommand = new Command<IShopItem>(SubstituteItem);
            AddShoppingCartCommand = new Command(() =>
            {
                _profile.CreateShoppingCart();
                OnPropertyChanged(nameof(ShoppingCarts));
                Shell.Current.CurrentPage.DisplaySnackbar(Resources.ShoppingCartAddedAlert);
            });
        }

        #endregion

        #region Public Properties

        public ICommand AddShoppingCartCommand { get; }

        public ICommand SubstituteItemCommand { get; }

        public ObservableCollection<IShoppingCart> ShoppingCarts => _profile.ShoppingCarts.ToObservableCollection();

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

        private async void SubstituteItem(IShopItem item)
        {
            var popupView = new ItemSearchPopupView(_eventAggregator)
            {
                BindingContext = new ItemSearchPopupViewModel(_dataService, _eventAggregator)
            };

            await Shell.Current.CurrentPage.ShowPopupAsync(popupView);

            if (popupView.BindingContext is not ItemSearchPopupViewModel { SelectedItem: not null } viewModel)
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

            ActiveShoppingCart.Dispose();
            ActiveShoppingCart.PropertyChanged -= OnActiveShoppingCartPropertyChanged;
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