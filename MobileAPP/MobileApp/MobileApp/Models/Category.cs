// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Category.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Models
{
    #region Imports

    using System.Globalization;
    using System.Text;

    using MobileApp.Interfaces;

    #endregion

    public class Category : ICategory
    {
        #region Constants and Private Fields

        private readonly List<ISubCategory> _subCategories = new();

        private string _iconPath;

        #endregion

        #region Constructors and Destructors

        public Category(string title)
        {
            Title = title;
        }

        #endregion

        #region Public Properties

        public IEnumerable<ISubCategory> SubCategories => _subCategories;

        public virtual string IconPath => _iconPath
            ??= $"{RemoveDiacritics(Title.Replace(" ", "").Replace(",", "").ToLower())}.png";

        public string Title { get; }

        #endregion

        #region Public Methods and Operators

        public ISubCategory GetOrCreateSubCategory(string subCategoryTitle)
        {
            if (_subCategories.Find(c => c.Title == subCategoryTitle) is {} subCategory)
            {
                return subCategory;
            }

            subCategory = new SubCategory(subCategoryTitle);
            _subCategories.Add(subCategory);
            return subCategory;
        }

        public bool RemoveSubCategory(ISubCategory subCategory)
        {
            return _subCategories.Remove(subCategory);
        }

        #endregion

        #region Methods and Operators

        protected static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        #endregion
    }
}