using Microsoft.AspNetCore.Mvc.Rendering;
using ShoppingApp.Attributes;
using ShoppingApp.Settings;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.ViewModels
{
    public class ProductViewModel
    {
        public string SellerId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; } = Enumerable.Empty<SelectListItem>();

        [MaxLength(2500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative number")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0.0m;

        public bool Available { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? ExpireDate { get; set; }

       

    }
}
