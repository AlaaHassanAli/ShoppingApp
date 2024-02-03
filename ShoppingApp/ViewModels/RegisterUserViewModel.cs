using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.ViewModels
{
    public class RegisterUserViewModel
    {
        public string UserName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
