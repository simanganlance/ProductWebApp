using System.ComponentModel.DataAnnotations;

namespace ProductApi.DTOs
{
    /// <summary>
    /// Represents Product DTO.
    /// </summary>
    public class ProductDto : ProductCreateUpdateDto
    {
        /// <summary>
        /// The unique identifier for the product.
        /// </summary>]
        public Guid Id { get; set; }
    }
}
