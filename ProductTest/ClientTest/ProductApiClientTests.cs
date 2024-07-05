using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using ProductWebView.Clients.Implementations;
using ProductWebView.Models;
using Xunit;

namespace ProductTest.ClientTest
{
    public class ProductApiClientTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly ProductApiClient _apiClient;

        public ProductApiClientTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:5001/api/")
            };
            _apiClient = new ProductApiClient(_httpClient);
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnProducts_WhenProductsExist()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Product 1" },
                new Product { Id = Guid.NewGuid(), Name = "Product 2" }
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(products)
                });

            // Act
            var result = await _apiClient.GetAllProductsAsync();

            // Assert
            Assert.Equal(products.Count, result.Count());
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _apiClient.GetAllProductsAsync());
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Product 1" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = JsonContent.Create(product)
                });

            // Act
            var result = await _apiClient.GetProductByIdAsync(productId);

            // Assert
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _apiClient.GetProductByIdAsync(productId));
        }

        [Fact]
        public async Task AddProductAsync_ShouldCallApiAndEnsureSuccess_WhenProductIsValid()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "New Product" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.Created
                });

            // Act
            await _apiClient.AddProductAsync(product);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Post &&
                    req.RequestUri == new Uri("https://localhost:5001/api/products")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task AddProductAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange
            var product = new Product { Id = Guid.NewGuid(), Name = "New Product" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _apiClient.AddProductAsync(product));
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldCallApiAndEnsureSuccess_WhenProductIsValid()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Updated Product" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            // Act
            await _apiClient.UpdateProductAsync(productId, product);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Put &&
                    req.RequestUri == new Uri($"https://localhost:5001/api/products/{productId}")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var product = new Product { Id = productId, Name = "Updated Product" };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _apiClient.UpdateProductAsync(productId, product));
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldCallApiAndEnsureSuccess_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NoContent
                });

            // Act
            await _apiClient.DeleteProductAsync(productId);

            // Assert
            _handlerMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Delete &&
                    req.RequestUri == new Uri($"https://localhost:5001/api/products/{productId}")),
                ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange
            var productId = Guid.NewGuid();
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _apiClient.DeleteProductAsync(productId));
        }
    }
}
