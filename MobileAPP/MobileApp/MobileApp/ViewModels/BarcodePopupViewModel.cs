using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.Events;
using MobileApp.Enums;
using MobileApp.Interfaces;

namespace MobileApp.ViewModels
{
    public class BarcodePopupViewModel : IHandle<EventMessageBase>
    {
        private readonly IDataService _dataService;

        public BarcodePopupViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }

        public string Title => "Termék adatok";

        public bool IsVisible { get; set; }

        public bool IsSearchVisible { get; private set; }

        public string SearchInput { get; set; }

        public ObservableCollection<IShopItem> SearchResults { get; private set; }

        public void Handle(EventMessageBase message)
        {
            if (message is { EventType: EventType.BarcodeRead, Sender: BarcodePageViewModel readerViewModel })
            {
                Task.Run(ShowPopup);
            }
        }

        private void ShowPopup()
        {
            IsVisible = true;
        }

        private Task<bool> GetItemByBarcode()
        {
            if(_dataService.)
        }


    }
}
