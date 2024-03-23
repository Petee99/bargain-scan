using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileApp.Interfaces;
using MobileApp.Models;

namespace MobileApp.ViewModels
{
    public class ShoppingCartViewModel : PropertyChangedBase
    {
        private readonly IShoppingCart _shoppingCart;

        public ShoppingCartViewModel(IShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }

        public IEnumerable<IShopItem> Items => _shoppingCart.Items;

        public bool AddItem(IShopItem item)
        {
            if (!_shoppingCart.AddItem(item))
            {
                return false;
            }

            FireItemsChanged();
            return true;
        }

        public bool RemoveItem(IShopItem item)
        {
            if (!_shoppingCart.RemoveItem(item))
            {
                return false;
            }

            FireItemsChanged();
            return true;
        }

        public string Name
        {
            get => _shoppingCart.Name;
            set
            {
                _shoppingCart.Name = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get => _shoppingCart.Description;
            set
            {
                _shoppingCart.Description = value;
                OnPropertyChanged();
            }
        }

        public double Total => _shoppingCart.Total;

        private void FireItemsChanged()
        {
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(Total));
        }
    }
}
