using ProductWebView.Models;

namespace ProductWebView.Clients.Interfaces
{
    public interface IProductApiClient
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Guid id, Product product);
        Task DeleteProductAsync(Guid id);
    }
}
