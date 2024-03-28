// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BargainScanApp.xaml.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp
{
    #region Imports

    using MobileApp.Interfaces;
    using MobileApp.Views;

    #endregion

    public partial class BargainScanApp
    {
        #region Constants and Private Fields

        private readonly IDataService _dataService;

        #endregion

        #region Constructors and Destructors

        public BargainScanApp(IResolverService resolverService, IDataService dataService)
        {
            InitializeComponent();
            _dataService = dataService;
            MainPage = resolverService.Resolve<AppShellView>() as Page;
        }

        #endregion

        #region Methods and Operators

        protected override void CleanUp()
        {
            _dataService.SaveApplicationData();
            base.CleanUp();
        }

        protected override void OnSleep()
        {
            _dataService.SaveApplicationData();
            base.OnSleep();
        }

        #endregion
    }
}