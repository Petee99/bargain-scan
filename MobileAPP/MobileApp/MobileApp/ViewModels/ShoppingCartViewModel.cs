// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ShoppingCartViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using CommunityToolkit.Maui.Alerts;

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Properties;

    #endregion

    public class ShoppingCartViewModel : PropertyChangedBase, IDisposable
    {
        #region Constants and Private Fields

        private readonly IShoppingCart _shoppingCart;

        #endregion

        #region Constructors and Destructors

        public ShoppingCartViewModel(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
            _shoppingCart.ItemsChanged += OnItemsChanged;
            RemoveItemCommand = new Command<IShopItem>(item => _shoppingCart.RemoveItem(item));
        }

        #endregion

        #region Public Properties

        public double Total => _shoppingCart.Total;

        public ICommand RemoveItemCommand { get; }

        public ObservableCollection<IShopItem> Items { get; } = new();

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

        public void Dispose()
        {
            _shoppingCart.ItemsChanged -= OnItemsChanged;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Private Methods

        private void OnItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            if (Items.Contains(e.ChangedItem) && e.EventType is EventType.ItemRemoved)
            {
                Items.Remove(e.ChangedItem);
            }

            if (!Items.Contains(e.ChangedItem) && e.EventType is EventType.ItemAdded)
            {
                Items.Add(e.ChangedItem);
            }

            ShowConfirmation(e.EventType);
            OnPropertyChanged(nameof(Total));
        }

        private static void ShowConfirmation(EventType eventType)
        {
            if (Shell.Current?.CurrentPage is not { } currentPage)
            {
                return;
            }

            switch (eventType)
            {
                case EventType.ItemAdded:
                    currentPage.DisplaySnackbar(Resources.ItemAddedAlert);
                    break;
                case EventType.ItemRemoved:
                    currentPage.DisplaySnackbar(Resources.ItemRemovedAlert);
                    break;
            }
        }

        #endregion
    }
}