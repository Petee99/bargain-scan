// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodePopupViewModel.cs" owner="Peter Mako">
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

    public class ItemSearchPopupViewModel : PropertyChangedBase
    {
        #region Constants and Private Fields

        private const int MaxPresentedItems = 10;
        
        private readonly IDataService _dataService;

        private readonly IEventAggregator _eventAggregator;

        private IShopItem _selectedItem;

        #endregion

        #region Constructors and Destructors

        public ItemSearchPopupViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            StartSearchCommand = new Command(() => Task.Run(SearchItemsBySearchTerm));
            AddItemToActiveShoppingCarCommand = new Command<IShopItem>(item => _dataService.UserProfile.ActiveShoppingCart.AddItem(item));
            SelectItemCommand = new Command<IShopItem>(item => SelectedItem = item);
        }

        #endregion

        #region Public Properties

        public ICommand AddItemToActiveShoppingCarCommand { get; }

        public ICommand SelectItemCommand { get; }

        public ICommand StartSearchCommand { get; }

        public IShopItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                _eventAggregator.Publish(new EventMessageBase(this, EventType.PopupCloseInitiated));
                OnPropertyChanged();
            }
        }

        public ObservableCollection<IShopItem> SearchResults { get; } = new();

        public string SearchInput { get; set; }

        #endregion

        #region Private Methods

        private void AddItems(IEnumerable<IShopItem> items)
        {
            foreach (IShopItem item in items)
            {
                SearchResults.Add(item);

                if (SearchResults.Count >= MaxPresentedItems)
                {
                    break;
                }
            }
        }

        private void SearchItemsBySearchTerm()
        {
            SearchResults.Clear();

            if (_dataService.GetShopItemsBySearchTerm(SearchInput).Result is { } items)
            {
                AddItems(items);
            }
        }

        #endregion
    }
}