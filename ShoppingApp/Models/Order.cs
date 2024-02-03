using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        public bool Current { get; set; } = false;

        [ForeignKey("ApplicationUser")]
        public virtual string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser? ApplicationUser { get; set; }

        public int NumberOfItems { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal TotalPrice { get; set; } = 0.0m;

        public ICollection<ProductOrder> Product_Orders { get; set; } = new List<ProductOrder>();


    }
}
