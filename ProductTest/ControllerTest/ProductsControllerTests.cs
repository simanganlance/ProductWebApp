using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ProductApi.Controllers;
using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Services.Interfaces;
using Xunit;

namespace ProductTest.ControllerTest
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProductsController>> _loggerMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProductsController>>();
            _controller = new ProductsController(_productServiceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllProducts_ShouldReturnOk_WhenProductsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Product 2" }
            };
            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = Guid.NewGuid(), Name = "Product 1" },
                new ProductDto { Id = Guid.NewGuid(), Name = "Product 2" }
            };
            _productServiceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductDto>>(products)).Returns(productDtos);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(productDtos.Count, returnValue.Count);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnOk_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Product 1" };
            var productDto = new ProductDto { Id = productId, Name = "Product 1" };
            _productServiceMock.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(productDto);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(productId, returnValue.Id);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productServiceMock.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnCreatedAtAction_WhenProductIsValid()
        {
            // Arrange
            var productDto = new ProductCreateUpdateDto { Name = "New Product" };
            var product = new Product { Id = Guid.NewGuid(), Name = "New Product" };
            var createdProductDto = new ProductDto { Id = product.Id, Name = "New Product" };
            _mapperMock.Setup(m => m.Map<Product>(productDto)).Returns(product);
            _mapperMock.Setup(m => m.Map<ProductDto>(product)).Returns(createdProductDto);

            // Act
            var result = await _controller.CreateProduct(productDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<ProductDto>(createdAtActionResult.Value);
            Assert.Equal(product.Id, returnValue.Id);
        }

        [Fact]
        public async Task CreateProduct_ShouldReturnBadRequest_WhenProductIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.CreateProduct(new ProductCreateUpdateDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNoContent_WhenProductIsValid()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductCreateUpdateDto { Name = "Updated Product" };
            var product = new Product { Id = productId, Name = "Updated Product" };
            _productServiceMock.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.UpdateProduct(productId, productDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var productDto = new ProductCreateUpdateDto { Name = "Updated Product" };
            _productServiceMock.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.UpdateProduct(productId, productDto);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnBadRequest_WhenProductIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.UpdateProduct(Guid.NewGuid(), new ProductCreateUpdateDto());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Product to delete" };
            _productServiceMock.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _productServiceMock.Setup(s => s.GetProductByIdAsync(productId)).ReturnsAsync((Product)null);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
