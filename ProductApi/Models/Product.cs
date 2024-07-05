using System.ComponentModel.DataAnnotations;

namespace ProductApi.Models
{
    /// <summary>
    /// Represents a product entity.
    /// </summary>
    public class Product
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
