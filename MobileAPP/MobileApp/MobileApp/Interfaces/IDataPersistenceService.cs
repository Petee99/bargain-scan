// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataPersistenceService.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IDataPersistenceService
    {
        #region Public Methods and Operators

        bool TrySaveLocalData(string fileName, string data);

        Task<string> TryLoadLocalData(string fileName);

        #endregion
    }
}