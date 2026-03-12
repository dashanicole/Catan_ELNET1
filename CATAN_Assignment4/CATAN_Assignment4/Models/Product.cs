using System.ComponentModel.DataAnnotations;

namespace CATAN_Assignment4.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        public decimal? Price { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        public int? Quantity { get; set; }
    }
}
