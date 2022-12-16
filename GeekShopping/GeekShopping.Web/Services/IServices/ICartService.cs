using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices
{
    public interface ICartService
    {
        Task<CartViewModel> FindCartByUserId(string userId);
        Task<CartViewModel> AddItemToCart(CartViewModel car);
        Task<CartViewModel> UpdateCart(CartViewModel cart);
        Task<bool> RemoveFromCart(long cartId);

        Task<bool> ApplyCoupon(CartViewModel cart);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);

        Task<object> Checkout(CartHeaderViewModel cartHeader);
    }
}
