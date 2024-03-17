// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IHandle.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IHandle<in T>
    {
        #region Public Methods and Operators

        void Handle(T message);

        #endregion
    }
}