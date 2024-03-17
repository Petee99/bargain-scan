// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.ComponentModel;

    using MobileApp.Interfaces;
    using MobileApp.Models;

    #endregion

    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region Constants and Private Fields

        private ObservableCollection<IShopItem> _items = new();

        #endregion

        #region Public Properties

        public ObservableCollection<IShopItem> ShopItems
        {
            get => _items;
            set
            {
                _items = value;
                Title = _items.First().SubCategory;
                OnPropertyChanged(nameof(ShopItems));
            }
        }

        public string Title { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Methods and Operators

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
        }

        #endregion
    }
}