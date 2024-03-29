// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyChangedBaseTests.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Tests.Models
{
    #region Imports

    using MobileApp.Models;

    #endregion

    [TestClass]
    public class PropertyChangedBaseTests
    {
        #region Public Methods and Operators

        [TestMethod]
        public void RaisePropertyChanged_RaisesEvent()
        {
            // Arrange
            var propertyChanged = false;
            var sut = new TestPropertyChangedBase();
            sut.PropertyChanged += (sender, args) => propertyChanged = true;

            // Act
            sut.RaisePropertyChanged();

            // Assert
            Assert.IsTrue(propertyChanged);
        }

        #endregion

        private class TestPropertyChangedBase : PropertyChangedBase
        {
            #region Public Methods and Operators

            public void RaisePropertyChanged()
            {
                OnPropertyChanged();
            }

            #endregion
        }
    }
}