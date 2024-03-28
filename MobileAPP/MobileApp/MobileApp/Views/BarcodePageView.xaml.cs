// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodePageView.xaml.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Views
{
    #region Imports

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;

    using ZXing.Net.Maui;

    #endregion

    public partial class BarcodePageView
    {
        #region Constants and Private Fields

        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors and Destructors

        public BarcodePageView(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            RefreshView();
        }

        #endregion

        #region Methods and Operators

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
        }

        #endregion

        #region Private Methods

        private void OnBarcodeDetected(object sender, BarcodeDetectionEventArgs e)
        {
            _eventAggregator.Publish(new BarcodeMessage(this, EventType.BarcodeRead, e.Results[0].Value));
        }

        private void RefreshView()
        {
            InitializeComponent();

            BarcodeReader.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormat.Ean13,
                AutoRotate = true,
                Multiple = false
            };

            BarcodeReader.BarcodesDetected += OnBarcodeDetected;
        }
        
        #endregion
    }
}