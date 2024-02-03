using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    public class OrdersHistory
    {
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; } = string.Empty;
        public virtual ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Order> Orders_History { get; set; } = new List<Order>();
    }
}
