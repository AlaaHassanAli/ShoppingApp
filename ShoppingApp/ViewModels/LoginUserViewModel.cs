using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.ViewModels
{
    public class LoginUserViewModel
    {
        public string UserName { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }

    }
}
