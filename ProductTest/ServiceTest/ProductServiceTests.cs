using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Models;
using ProductApi.Repositories.Interfaces;
using ProductApi.Services.Implementations;
using Xunit;

namespace ProductTest.ServiceTest
{
    public class ProductServiceTests
    {
        
        private readonly Mock<IRepository<Product>> _productRepositoryMock;
        private readonly Mock<ILogger<ProductService>> _loggerMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productRepositoryMock = new Mock<IRepository<Product>>();
            _loggerMock = new Mock<ILogger<ProductService>>();
            _productService = new ProductService(_productRepositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Test Product" };
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productRepositoryMock.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _productService.GetProductByIdAsync(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
        {
            new Product { Id = Guid.NewGuid(), Name = "Test Product 1" },
            new Product { Id = Guid.NewGuid(), Name = "Test Product 2" }
        };
            _productRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

            // Act
            var result = await _productService.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count());
        }

        [Fact]
        public async Task AddProductAsync_ShouldAddProduct()
        {
            // Arrange
            var product = new Product { Name = "New Product" };

            // Act
            await _productService.AddProductAsync(product);

            // Assert
            _productRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        [Fact]
        public async Task AddProductAsync_ShouldThrowException_WhenProductIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _productService.AddProductAsync(null));
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldUpdateProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Updated Product" };

            // Act
            await _productService.UpdateProductAsync(productId, product);

            // Assert
            _productRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Id == productId)), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldDeleteProduct()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "Product to delete" };

            // Act
            await _productService.DeleteProductAsync(product);

            // Assert
            _productRepositoryMock.Verify(repo => repo.DeleteAsync(It.Is<Product>(p => p.Id == product.Id)), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldThrowException_WhenProductIsNull()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _productService.DeleteProductAsync(null));
        }
        

    }
}
