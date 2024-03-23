// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFuzzySearch.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IFuzzySearch<T> where T : INamed
    {
        #region Public Methods and Operators
        IEnumerable<T> Search(string searchTerm);

        void AddItem(T item);

        #endregion
    }
}