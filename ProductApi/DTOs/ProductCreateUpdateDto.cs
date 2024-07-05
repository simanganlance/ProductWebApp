using System.ComponentModel.DataAnnotations;

namespace ProductApi.DTOs
{
    /// <summary>
    /// Represents Product Create and Update DTO.
    /// </summary>
    public class ProductCreateUpdateDto
    {
        /// <summary>
        /// The name of the product. Required.
        /// </summary>
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The price of the product. Must be a positive value.
        /// </summary>
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }
    }
}
