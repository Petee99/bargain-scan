// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IResolverService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IResolverService
    {
        #region Public Methods and Operators

        object Resolve<TView>() where TView : class;

        #endregion
    }
}