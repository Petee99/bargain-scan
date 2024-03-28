// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodeReadMessage.cs" owner="Peter Mako">
//   Thesis work by Peter Mako for Obuda University / Business Informatics MSc. 2024
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MobileApp.Events
{
    using MobileApp.Enums;

    public class BarcodeMessage : EventMessageBase
    {
        public BarcodeMessage(object sender, EventType eventType, string barCode) : base(sender, eventType)
        {
            BarCode = barCode;
        }

        public string BarCode { get; }
    }
}