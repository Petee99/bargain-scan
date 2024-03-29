// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FuzzySearchTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Models
{
    #region Imports

    using MobileApp.Interfaces;
    using MobileApp.Models;

    #endregion

    [TestClass]
    public class FuzzySearchTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void FuzzySearch_SearchForExactMatch_ReturnsExactMatch()
        {
            // Arrange
            var items = new List<NamedObject> { new("banana"), new("apple"), new("orange") };
            var fuzzySearch = new FuzzySearch<NamedObject>();
            items.ForEach(fuzzySearch.AddItem);

            // Act
            List<NamedObject> result = fuzzySearch.Search(items.First().Name).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(items.First(), result.First());
        }

        [TestMethod]
        public void FuzzySearch_SearchForNonExactMatch_ReturnsNonExactMatch()
        {
            // Arrange
            var items = new List<NamedObject> { new("banana"), new("apple"), new("orange") };
            var fuzzySearch = new FuzzySearch<NamedObject>();
            items.ForEach(fuzzySearch.AddItem);

            // Act
            List<NamedObject> result = fuzzySearch.Search("ban").ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(items.First(), result.First());
        }

        [TestMethod]
        public void FuzzySearch_SearchForNonExistingItem_ReturnsEmptyList()
        {
            // Arrange
            var items = new List<NamedObject> { new("banana"), new("apple"), new("orange") };
            var fuzzySearch = new FuzzySearch<NamedObject>();
            items.ForEach(fuzzySearch.AddItem);

            // Act
            List<NamedObject> result = fuzzySearch.Search("pear").ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        #endregion

        private class NamedObject : INamed
        {
            #region Constructors and Destructors

            public NamedObject(string name)
            {
                Name = name;
            }

            #endregion

            #region Public Properties

            public string Name { get; set; }

            #endregion
        }
    }
}