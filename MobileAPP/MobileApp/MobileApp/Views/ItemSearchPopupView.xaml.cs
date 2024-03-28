// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemSearchPopupView.xaml.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Views
{
    #region Imports

    using MobileApp.Enums;
    using MobileApp.Events;
    using MobileApp.Interfaces;
    using MobileApp.ViewModels;

    #endregion

    public partial class ItemSearchPopupView : IHandle<EventMessageBase>
    {
        #region Constants and Private Fields

        private readonly IEventAggregator _eventAggregator;

        #endregion

        #region Constructors and Destructors

        public ItemSearchPopupView(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.Subscribe(this);

            InitializeComponent();
        }

        #endregion

        #region Public Methods and Operators

        public void Handle(EventMessageBase message)
        {
            if (message.Sender is not ItemSearchPopupViewModel || message.EventType is not EventType.PopupCloseInitiated)
            {
                return;
            }

            _eventAggregator.UnSubscribe(this);
            CloseAsync();
        }

        #endregion
    }
}