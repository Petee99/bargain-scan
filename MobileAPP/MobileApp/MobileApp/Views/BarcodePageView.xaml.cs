// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodePageView.xaml.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Views
{
    #region Imports

    #endregion

    public partial class BarcodePageView
    {
        #region Constructors and Destructors

        public BarcodePageView()
        {
            InitializeComponent();
            /*
            qrView.BarCodeOptions = new BarcodeDecodeOptions
            {
                PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.CODE_39 }
            };
        }

        #endregion

        #region Private Methods

        private void OnBarcodeDetected(object sender, BarcodeEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() => { barcodeResult.Text = $"{args.Result[0].Text}"; });
        }

        private void OnCamerasLoaded(object sender, EventArgs args)
        {
            if (qrView.Cameras.Count <= 0)
            {
                return;
            }

            qrView.Camera = qrView.Cameras[0];
            MainThread.BeginInvokeOnMainThread(RefreshCameras);
        }

        private async void RefreshCameras()
        {
            await qrView.StopCameraAsync();
            await qrView.StartCameraAsync();
            */
        }

        #endregion
    }
}