// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodePageViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using CommunityToolkit.Maui.Views;

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;
    using MobileApp.Models;
    using MobileApp.Views;

    #endregion

    public class BarcodePageViewModel : PropertyChangedBase, IHandle<BarcodeMessage>
    {
        #region Constants and Private Fields

        private readonly IDataService _dataService;

        private readonly IEventAggregator _eventAggregator;

        private string _barCode;

        #endregion

        #region Constructors and Destructors

        public BarcodePageViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            AssignBarcodeCommand = new Command(AssignBarcode);
            AddItemToActiveShoppingCarCommand = new Command<IShopItem>(item => _dataService.UserProfile.ActiveShoppingCart.AddItem(item));
        }

        #endregion

        #region Public Properties

        public bool IsAssignBarcodePossible { get; private set; }

        public ICommand AssignBarcodeCommand { get; }

        public ICommand AddItemToActiveShoppingCarCommand { get; }

        public ObservableCollection<IShopItem> Items { get; } = new();

        #endregion

        #region Public Methods and Operators

        public void Handle(BarcodeMessage message)
        {
            if (message.EventType is not EventType.BarcodeRead || message.BarCode is not { Length: > 0 })
            {
                return;
            }

            FlushBarCode();
            _barCode = message.BarCode;
            SearchBarcode();
        }

        #endregion

        #region Private Methods

        private async void AssignBarcode()
        {
            if (_barCode is not { Length: > 0 })
            {
                FlushBarCode();
                return;
            }

            var popupView = new ItemSearchPopupView(_eventAggregator)
            {
                BindingContext = new ItemSearchPopupViewModel(_dataService, _eventAggregator)
            };

            await Shell.Current.CurrentPage.ShowPopupAsync(popupView);

            if (popupView.BindingContext is not ItemSearchPopupViewModel { SelectedItem: not null } viewModel)
            {
                return;
            }

            _dataService.TryUpdateBarCode(viewModel.SelectedItem, _barCode);
            FlushBarCode();
        }
        
        private void FlushBarCode()
        {
            _barCode = null;
            IsAssignBarcodePossible = false;
            OnPropertyChanged(nameof(IsAssignBarcodePossible));
        }

        private async void SearchBarcode()
        {
            if (_barCode is not { Length: > 0 })
            {
                FlushBarCode();
                return;
            }

            Items.Clear();

            foreach (IShopItem item in await _dataService.GetShopItemsByBarcode(_barCode))
            {
                Items.Add(item);
            }

            IsAssignBarcodePossible = !Items.Any();
            OnPropertyChanged(nameof(IsAssignBarcodePossible));
        }

        #endregion
    }
}