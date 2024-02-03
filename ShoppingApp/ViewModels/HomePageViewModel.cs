using ShoppingApp.Models;

namespace ShoppingApp.ViewModels
{
    public class HomePageViewModel
    {
        public IEnumerable<Product> Products { get; set; } = new List<Product>();
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();

    }
}
