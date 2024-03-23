// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageView.xaml.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2023
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Views
{
    #region Imports

    using Camera.MAUI.ZXingHelper;

    using MobileApp.ViewModels;

    using ZXing;

    #endregion

    public partial class BarcodePageView
    {
        #region Constructors and Destructors

        public BarcodePageView()
        {
            InitializeComponent();
            qrView.BarCodeOptions = new BarcodeDecodeOptions()
            {
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.CODE_39 }
            };
            BindingContext = new BarcodePageViewModel();
        }

        #endregion

        private void OnCamerasLoaded(object sender, EventArgs args)
        {
            if (qrView.Cameras.Count <= 0)
            {
                return;
            }

            qrView.Camera = qrView.Cameras[0];
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await qrView.StopCameraAsync();
                await qrView.StartCameraAsync();
            });
        }

        private void OnBarcodeDetected(object sender, BarcodeEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                barcodeResult.Text = $"{args.Result[0].BarcodeFormat}: {args.Result[0].Text}";
            });
        }
    }
}