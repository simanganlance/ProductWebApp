using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProductApi.Models;
using ProductApi.Repositories.Implementations;
using ProductApi.Repositories;
using Xunit;

namespace ProductTest.RepositoryTest
{
    public class RepositoryTests
    {
        private readonly ProductDbContext _context;
        private readonly Repository<Product> _repository;

        public RepositoryTests()
        {
            var options = new DbContextOptionsBuilder<ProductDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ProductDbContext(options);
            _repository = new Repository<Product>(_context);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description", Price = 1.99M };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Act
            var result = await _repository.GetByIdAsync(Guid.NewGuid());

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description", Price = 1.99M  },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Description = "Description", Price = 1.99M  }
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task FindAsync_ShouldReturnMatchingProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description", Price = 1.99M  },
                new Product { Id = Guid.NewGuid(), Name = "Product 2", Description = "Description", Price = 1.99M  }
            };
            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.FindAsync(p => p.Name.Contains("Product"));

            // Assert
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task AddAsync_ShouldAddProduct()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description", Price = 1.99M };

            // Act
            await _repository.AddAsync(product);
            var result = await _context.Products.FindAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product.Id, result.Id);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateProduct()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description", Price = 1.99M };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            product.Name = "Updated Product";
            await _repository.UpdateAsync(product);
            var result = await _context.Products.FindAsync(product.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated Product", result.Name);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveProduct()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "Product 1", Description = "Description", Price = 1.99M };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(product);
            var result = await _context.Products.FindAsync(product.Id);

            // Assert
            Assert.Null(result);
        }
    }
}
