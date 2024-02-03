using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.ViewModels
{
    public class ResetPasswordSellerViewModel
    {
        public string SellerName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
    }
}
