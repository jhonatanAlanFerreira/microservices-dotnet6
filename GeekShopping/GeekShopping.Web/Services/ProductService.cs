using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;

namespace GeekShopping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;
        public const string BasePath = "api/v1/product";

        public ProductService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<IEnumerable<ProductViewModel>> FindAllProducts()
        {
            var response = await _client.GetFromJsonAsync<List<ProductViewModel>>(BasePath);
            return response;
        }

        public async Task<ProductViewModel> FindProductById(long id)
        {
            var response = await _client.GetFromJsonAsync<ProductViewModel>($"{BasePath}/{id}");
            return response;
        }

        public async Task<ProductViewModel> CreateProduct(ProductViewModel product)
        {
            var response = await _client.PostAsJsonAsync(BasePath, product);
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<ProductViewModel>();
            throw new Exception("Something went wrong when calling API");
        }

        public async Task<ProductViewModel> UpdateProduct(ProductViewModel product)
        {
            var response = await _client.PutAsJsonAsync(BasePath, product);
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<ProductViewModel>();
            throw new Exception("Something went wrong when calling API");
        }

        public async Task<bool> DeleteByIdProduct(long id)
        {
            var response = await _client.DeleteAsync($"{BasePath}/{id}");
            if (response.IsSuccessStatusCode) return await response.Content.ReadFromJsonAsync<bool>();
            throw new Exception("Something went wrong when calling API");
        }

    }
}
