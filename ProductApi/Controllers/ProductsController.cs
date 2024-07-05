using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.DTOs;
using ProductApi.Models;
using ProductApi.Services.Interfaces;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, IMapper mapper, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>A list of products.</returns>\
        /// <response code="200">Returns the list of products.</response>
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("GET /products called");
            var products = await _productService.GetAllProductsAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productDtos);
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product with the specified ID.</returns>
        /// <response code="200">Returns the product with the specified ID.</response>
        /// <response code="404">If the product is not found.</response>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            _logger.LogInformation($"GET /products/{id} called");
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productDto">The product to add.</param>
        /// <returns>The newly created product.</returns>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If the product is invalid.</response>
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateUpdateDto productDto)
        {
            _logger.LogInformation("POST /products called");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var product = _mapper.Map<Product>(productDto);
            await _productService.AddProductAsync(product);
            var createdProductDto = _mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, createdProductDto);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="productDto">The updated product data.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the update is successful.</response>
        /// <response code="400">If the product is invalid.</response>
        /// <response code="404">If the product is not found.</response>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductCreateUpdateDto productDto)
        {
            _logger.LogInformation($"PUT /products/{id} called");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            var product = _mapper.Map(productDto, existingProduct);
            await _productService.UpdateProductAsync(id, product);
            return NoContent();
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>No content.</returns>
        /// <response code="204">If the delete is successful.</response>
        /// <response code="404">If the product is not found.</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            _logger.LogInformation($"DELETE /products/{id} called");
            var existingProduct = await _productService.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            await _productService.DeleteProductAsync(existingProduct);
            return NoContent();
        }
    }
}
