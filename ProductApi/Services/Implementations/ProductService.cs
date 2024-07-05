using Microsoft.Extensions.Logging;
using ProductApi.Models;
using ProductApi.Repositories.Interfaces;
using ProductApi.Services.Interfaces;

namespace ProductApi.Services.Implementations
{
    public class ProductService : IProductService
    {

        private readonly IRepository<Product> _productRepository;
        private readonly ILogger<ProductService> _logger;
        public ProductService(IRepository<Product> productRepository, ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            _logger.LogInformation($"Retrieving product with id {id}");
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            _logger.LogInformation("Retrieving all products");
            return await _productRepository.GetAllAsync();
        }

        public async Task AddProductAsync(Product product)
        {
            _logger.LogInformation("Adding a new product");
            product.Id = Guid.NewGuid();
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateProductAsync(Guid id, Product product)
        {
            _logger.LogInformation($"Updating product with id {product.Id}");
            product.Id = id;
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductAsync(Product product)
        {
            _logger.LogInformation($"Deleting product with id {product.Id}");
            await _productRepository.DeleteAsync(product);
            
        }
    }
}
