// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ResolverService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using MobileApp.Interfaces;

    #endregion

    public class ResolverService : IResolverService
    {
        #region Constants and Private Fields

        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Constructors and Destructors

        public ResolverService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods and Operators

        public object Resolve<TView>() where TView : class
        {
            return _serviceProvider.GetService(typeof(TView));
        }

        #endregion
    }
}