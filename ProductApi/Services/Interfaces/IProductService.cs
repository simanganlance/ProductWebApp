using ProductApi.Models;

namespace ProductApi.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Guid id, Product product);
        Task DeleteProductAsync(Product product);
    }
}
