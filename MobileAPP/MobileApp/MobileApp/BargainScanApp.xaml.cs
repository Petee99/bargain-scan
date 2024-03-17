// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BargainScanApp.xaml.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp
{
    #region Imports

    using MobileApp.Views;

    #endregion

    public partial class BargainScanApp
    {
        #region Constructors and Destructors

        public BargainScanApp(IResolverService resolverService)
        {
            InitializeComponent();
            MainPage = resolverService.Resolve<AppShellView>() as Page;
        }

        #endregion
    }
}