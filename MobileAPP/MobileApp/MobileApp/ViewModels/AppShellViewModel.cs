// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppShellViewModel.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.ViewModels
{
    #region Imports

    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Windows.Input;

    using CommunityToolkit.Maui.Core.Extensions;

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;

    #endregion

    public class AppShellViewModel : INotifyPropertyChanged, IHandle<EventMessageBase>
    {
        #region Constants and Private Fields

        private readonly IDataService _dataService;

        private readonly IEventAggregator _eventAggregator;

        private ObservableCollection<CategoryViewModel> _categories = new();

        #endregion

        #region Constructors and Destructors

        public AppShellViewModel(IDataService dataService, IEventAggregator eventAggregator)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);
            ToggleCategoryCommand = new Command<CategoryViewModel>(ToggleCategory);
            Task.Run(LoadCategories);
        }

        #endregion

        #region Public Properties

        public ICommand ToggleCategoryCommand { get; private set; }

        public ObservableCollection<CategoryViewModel> Categories
        {
            get => _categories;
            private set
            {
                _categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Methods and Operators

        public void Handle(EventMessageBase message)
        {
            if (message is { EventType: EventType.CategorySelected, Sender: ICategory category })
            {
                Task.Run(() => SelectCategory(category));
            }
        }

        #endregion

        #region Methods and Operators

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Private Methods

        private async Task LoadCategories()
        {
            ObservableCollection<CategoryViewModel> categoryViewModels = new();

            foreach (var category in await _dataService.GetMainCategories())
            {
                categoryViewModels.Add(new CategoryViewModel(category, _eventAggregator));
            }

            Categories = categoryViewModels;
        }

        private async Task SelectCategory(ICategory category)
        {
            if (category is not ISubCategory subCategory ||
                Shell.Current.CurrentPage?.BindingContext is not MainPageViewModel viewModel)
            {
                return;
            }

            Shell.Current.FlyoutIsPresented = false;

            viewModel.ShopItems = (await _dataService.GetShopItems(subCategory)).ToObservableCollection();
        }

        private void ToggleCategory(CategoryViewModel category)
        {
            if (category is null)
            {
                return;
            }

            category.IsActive = !category.IsActive;
            OnPropertyChanged(nameof(Categories));
        }

        #endregion
    }
}