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

    public class BarcodePopupViewModel : PropertyChangedBase, IHandle<EventMessageBase>
    {
        #region Constants and Private Fields

        private const int MaxPresentedItems = 10;

        private readonly IDataService _dataService;

        private bool _isSearchVisible;

        private string _barCode;

        #endregion

        #region Constructors and Destructors

        public BarcodePopupViewModel(IDataService dataService)
        {
            _dataService = dataService;
            StartSearchCommand = new Command(() => Task.Run(SearchItemsBySearchTerm));
        }

        #endregion

        #region Public Properties

        public bool IsSearchVisible
        {
            get => _isSearchVisible;
            private set
            {
                _isSearchVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand StartSearchCommand { get; }

        public ObservableCollection<IShopItem> SearchResults { get; } = new();

        public string SearchInput { get; set; }

        public string Title => "Termékek";

        #endregion

        #region Public Methods and Operators

        public void Handle(EventMessageBase message)
        {
            if (message is not { EventType: EventType.BarcodeRead, Sender: BarcodePageViewModel readerViewModel })
            {
                return;
            }

            _barCode = readerViewModel.BarCode;
            Task.Run(GetItemsByBarcode);
        }

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

        private void GetItemsByBarcode()
        {
            SearchResults.Clear();

            if (_dataService.GetShopItemsByBarcode(_barCode).Result is not { } items)
            {
                IsSearchVisible = false;
                return;
            }

            AddItems(items);

            IsSearchVisible = SearchResults.Any();
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