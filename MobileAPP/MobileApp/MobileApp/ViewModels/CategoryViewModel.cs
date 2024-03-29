// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Windows.Input;

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;

    #endregion

    public class CategoryViewModel
    {
        #region Constants and Private Fields

        private readonly ICategory _category;

        private readonly IEventAggregator _eventAggregator;

        private readonly IList<CategoryViewModel> _subCategories;

        private bool _isExpanded;

        #endregion

        #region Constructors and Destructors

        public CategoryViewModel(ICategory category, IEventAggregator eventAggregator)
        {
            _category = category;
            _eventAggregator = eventAggregator;
            _subCategories = CreateSubCategoryViewModels(category.SubCategories);
            SelectCommand = new Command(SelectCategory);
        }

        #endregion

        #region Public Properties

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                IsExpandedChanged?.Invoke(this, EventArgs.Empty);
                _isExpanded = value;
            }
        }

        public event EventHandler IsExpandedChanged;

        public ICommand SelectCommand { get; }

        public IEnumerable<CategoryViewModel> SubCategories => _subCategories;

        public string IconPath => _category.IconPath;

        public string Title => _category.Title;

        #endregion

        #region Private Methods

        private IList<CategoryViewModel> CreateSubCategoryViewModels(IEnumerable<ICategory> subCategories)
        {
            return subCategories.Select(subCategory => new CategoryViewModel(subCategory, _eventAggregator)).ToList();
        }

        private void SelectCategory()
        {
            _eventAggregator.Publish(new EventMessageBase(_category, EventType.CategorySelected));
        }

        #endregion
    }
}