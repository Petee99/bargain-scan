// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemsChangedEventArgs.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Events
{
    #region Imports

    using MobileApp.Enums;
    using MobileApp.Interfaces;

    #endregion

    public class ItemsChangedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        public ItemsChangedEventArgs(EventType eventType, IShopItem changedItem)
        {
            ChangedItem = changedItem;
            EventType = eventType;
        }

        #endregion

        #region Public Properties

        public EventType EventType { get; }

        public IShopItem ChangedItem { get; }

        #endregion
    }
}