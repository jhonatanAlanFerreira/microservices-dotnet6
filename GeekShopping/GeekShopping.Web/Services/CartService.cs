using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;

namespace GeekShopping.Web.Services
{
    public class CartService : ICartService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/cart";

        public CartService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<CartViewModel> FindCartByUserId(string userId)
        {
            var response = await _client.GetFromJsonAsync<CartViewModel>($"{BasePath}/find-cart/{userId}");
            return response;
        }

        public async Task<CartViewModel> AddItemToCart(CartViewModel model)
        {
            var response = await _client.PostAsJsonAsync($"{BasePath}/add-cart", model);;
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<CartViewModel>();
            throw new Exception("Something went wrong when calling API");
        }

        public async Task<CartViewModel> UpdateCart(CartViewModel model)
        {
            var response = await _client.PutAsJsonAsync($"{BasePath}/update-cart", model);
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<CartViewModel>();
            throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> RemoveFromCart(long cartId)
        {
            var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();
            throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> ApplyCoupon(CartViewModel model)
        {
            var response = await _client.PostAsJsonAsync($"{BasePath}/apply-coupon", model);

            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();
            throw new Exception("Something went wrong when calling API");
        }
        public async Task<bool> RemoveCoupon(string userId)
        {
            var response = await _client.DeleteAsync($"{BasePath}/remove-coupon/{userId}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();
            throw new Exception("Something went wrong when calling API");
        }

        public async Task<CartViewModel> Checkout(CartHeaderViewModel cartHeader)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ClearCart(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
