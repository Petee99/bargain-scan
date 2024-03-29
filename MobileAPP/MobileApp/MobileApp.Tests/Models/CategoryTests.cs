// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Models
{
    #region Imports

    using MobileApp.Interfaces;
    using MobileApp.Models;

    using Moq;

    #endregion

    [TestClass]
    public class CategoryTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void CreateSubCategory_SubCategoryParentIsCategory_IconPathMatchesParentIconPath()
        {
            // Arrange
            var category = new Category("Test");

            // Act
            ISubCategory subCategory = category.GetOrCreateSubCategory("SubTest");

            // Assert
            Assert.AreEqual(category, subCategory.Parent);
            Assert.AreEqual(category.IconPath, subCategory.IconPath);
        }

        [TestMethod]
        public void GetOrCreateSubCategory_CategoryContainsSubcategoryWithName_ReturnsTheSameCategory()
        {
            // Arrange
            var category = new Category(string.Empty);

            // Act
            ISubCategory subCategory = category.GetOrCreateSubCategory(string.Empty);

            // Assert
            Assert.AreEqual(subCategory, category.GetOrCreateSubCategory(subCategory.Title));
            Assert.AreEqual(1, category.SubCategories.Count());
        }

        [TestMethod]
        public void GetOrCreateSubCategory_CategoryDoesNotContainSubcategoryWithName_ReturnsDifferentCategory()
        {
            // Arrange
            var category = new Category(string.Empty);

            // Act
            ISubCategory subCategory = category.GetOrCreateSubCategory(string.Empty);

            // Assert
            Assert.AreNotEqual(subCategory, category.GetOrCreateSubCategory("Test"));
            Assert.AreEqual(2, category.SubCategories.Count());
        }

        [TestMethod]
        public void IconPath_ReturnsCategoryNameWithoutDiacritics_EndingInDotPng()
        {
            // Arrange
            var category = new Category("Fruits and Vegetables");

            // Assert
            Assert.AreEqual("fruitsandvegetables.png", category.IconPath);
        }

        [TestMethod]
        public void RemoveSubCategory_SubCategoryIsContainedWithinCategory_ReturnsTrue()
        {
            // Arrange
            var category = new Category(string.Empty);

            // Act
            bool removed = category.RemoveSubCategory(category.GetOrCreateSubCategory(string.Empty));

            // Assert
            Assert.IsTrue(removed);
            Assert.AreEqual(0, category.SubCategories.Count());
        }

        [TestMethod]
        public void RemoveSubCategory_SubCategoryIsNotContainedWithinCategory_ReturnsFalse()
        {
            // Arrange
            var category = new Category(string.Empty);
            category.GetOrCreateSubCategory("Test");

            // Act
            bool removed = category.RemoveSubCategory(new Mock<ISubCategory>().Object);

            // Assert
            Assert.IsFalse(removed);
            Assert.AreEqual(1, category.SubCategories.Count());
        }

        #endregion
    }
}