using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.ViewModels
{
    public class SellerViewModel
    {
        public string Name { get; set; } = string.Empty;

        public string Id { get; set; } = string.Empty;
        public bool active { get; set; } = true;
        
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;   
        
        public string Address { get; set; } = string.Empty;


    }
}
