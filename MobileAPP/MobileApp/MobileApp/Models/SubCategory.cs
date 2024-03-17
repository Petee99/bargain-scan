// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubCategory.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using MobileApp.Interfaces;

    #endregion

    public class SubCategory : Category, ISubCategory
    {
        #region Constructors and Destructors

        public SubCategory(string title) : base(title)
        {
        }

        #endregion
    }
}