using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    public class Product
    {
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string SellerId { get; set; } = string.Empty;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative number")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0.0m;

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative number")]
        public int Quantity { get; set; }
        public bool Available { get; set; }
        public  DateOnly? ExpireDate { get; set; }

        [Display(Name = "Category")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

       // public ICollection<ProductOrder> ProductOrder { get; set; } = new List<ProductOrder>();



    }
}
