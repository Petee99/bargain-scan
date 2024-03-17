// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EventMessageBase.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Events
{
    #region Imports

    using MobileApp.Enums;

    #endregion

    public class EventMessageBase
    {
        #region Constructors and Destructors

        public EventMessageBase(object sender, EventType eventType)
        {
            Sender = sender;
            EventType = eventType;
        }

        #endregion

        #region Public Properties

        public EventType EventType { get; }

        public object Sender { get; }

        #endregion
    }
}