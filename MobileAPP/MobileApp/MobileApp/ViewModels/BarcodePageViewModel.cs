// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodePageViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    public class BarcodePageViewModel
    {
        private const string PageTitle = "Vonalkód beolvasása";

        private string _barCode;

        public string BarCode
        {
            get => _barCode;
            set
            {
                _barCode = value;
                InvokeItemPopUp();
            }
        }

        public string Title => PageTitle;

        private Task InvokeItemPopUp()
        {
            return Task.CompletedTask;
        } 
    }
}