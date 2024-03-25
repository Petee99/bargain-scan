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

    using MobileApp.Enums;
    using MobileApp.Events;
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

            OnPropertyChanged(nameof(Total));
        }

        #endregion
    }
}