// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICategory.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface ICategory
    {
        #region Public Properties

        IEnumerable<ISubCategory> SubCategories { get; }

        string IconPath { get; }

        string Title { get; }

        #endregion

        #region Public Methods and Operators

        bool RemoveSubCategory(ISubCategory category);

        ISubCategory GetOrCreateSubCategory(string subCategoryTitle);

        #endregion
    }
}