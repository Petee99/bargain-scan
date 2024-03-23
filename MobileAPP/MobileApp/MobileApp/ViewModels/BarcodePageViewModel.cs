// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodePageViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;
    using MobileApp.Views;

    #endregion

    public class BarcodePageViewModel
    {
        #region Constants and Private Fields

        private const string PageTitle = "Vonalkód beolvasása";

        private readonly IEventAggregator _eventAggregator;

        private readonly IResolverService _resolverService;

        private string _barCode;

        #endregion

        #region Constructors and Destructors

        public BarcodePageViewModel(IResolverService resolverService, IEventAggregator eventAggregator)
        {
            _resolverService = resolverService;
            _eventAggregator = eventAggregator;
            BarCode = "asd";
        }

        #endregion

        #region Public Properties

        public string BarCode
        {
            get => _barCode;
            set
            {
                if (_barCode is { Length: <= 0 })
                {
                    return;
                }

                _barCode = value;
                Task.Run(InvokePopUp);
            }
        }

        public string Title => PageTitle;

        #endregion

        #region Private Methods

        private async Task HidePopUp()
        {
            await Shell.Current.Navigation.PopModalAsync();
        }

        private async Task InvokePopUp()
        {
            var popupview = _resolverService.Resolve<BarcodePopupView>() as Page;
            await Shell.Current.Navigation.PushAsync(popupview);
            _eventAggregator.Publish(new EventMessageBase(this, EventType.BarcodeRead));
        }

        #endregion
    }
}