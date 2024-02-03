using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Address { get; set; } = string.Empty;

        public bool Active { get; set; } = true;

        public ICollection<Product> Products { get; set; } = new List<Product>();

       

    }
}
