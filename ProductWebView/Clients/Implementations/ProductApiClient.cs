using ProductWebView.Clients.Interfaces;
using ProductWebView.Models;

namespace ProductWebView.Clients.Implementations
{
    public class ProductApiClient : IProductApiClient
    {
        private readonly HttpClient _httpClient;

        public ProductApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<Product>>("products");
            return response;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            var response = await _httpClient.GetFromJsonAsync<Product>($"products/{id}");
            return response;
        }

        public async Task AddProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("products", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(Guid id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"products/{id}", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
