// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;

    using CommunityToolkit.Maui.Alerts;

    using MobileApp.Interfaces;
    using MobileApp.Models;

    #endregion

    public class MainPageViewModel : PropertyChangedBase
    {
        #region Constants and Private Fields
        
        private ObservableCollection<IShopItem> _items = new();

        private string _title;

        #endregion

        #region Constructors and Destructors

        public MainPageViewModel(IDataService dataService)
        {
            AddToShoppingCartCommand = new Command<IShopItem>(item =>
            {
                dataService.UserProfile.ActiveShoppingCart.AddItem(item);
            });
        }

        #endregion

        #region Public Properties

        public Command<IShopItem> AddToShoppingCartCommand { get; }

        public ObservableCollection<IShopItem> ShopItems
        {
            get => _items;
            set
            {
                _items = value;
                Title = _items.FirstOrDefault()?.SubCategory;
                OnPropertyChanged();
            }
        }

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}