// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEventAggregator.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Interfaces
{
    public interface IEventAggregator
    {
        #region Public Methods and Operators

        void Publish<T>(T messageToPublish);

        void Subscribe(object subscriber);

        void UnSubscribe(object subscriber);

        #endregion
    }
}