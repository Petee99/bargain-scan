// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;

    using MobileApp.Interfaces;
    using MobileApp.Models;

    #endregion

    public class MainPageViewModel : PropertyChangedBase
    {
        #region Constants and Private Fields

        private ObservableCollection<IShopItem> _items = new();

        private string _title;

        #endregion

        #region Public Properties

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