using MobileApp.Interfaces;
using MobileApp.Models;

namespace MobileApp.ViewModels;

public class UserProfileViewModel : PropertyChangedBase
{
    private readonly IUserProfile _profile;

    private IShoppingCart _activeShoppingCart;

    public IShoppingCart ActiveShoppingCart => _activeShoppingCart ?? GetDefaultShoppingCart();

    public string Name
    {
        get => _profile.Name;
        set
        {
            _profile.Name = value;
            OnPropertyChanged();
        }
    }

    public IEnumerable<IShoppingCart> ShoppingCarts => _profile.ShoppingCarts;

    public UserProfileViewModel(IUserProfile profile)
    {
        _profile = profile;
    }

    private IShoppingCart GetDefaultShoppingCart()
    {
        return _profile.ShoppingCarts.FirstOrDefault() ?? _profile.CreateShoppingCart();
    }
}