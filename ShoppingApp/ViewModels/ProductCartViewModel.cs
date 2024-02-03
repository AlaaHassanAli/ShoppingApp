using ShoppingApp.Attributes;
using ShoppingApp.Settings;
using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.ViewModels
{
    public class ProductCartViewModel : ProductViewModel
    {
        public int Id { get; set; }
        public string CurrentCover { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "Quantity should be greater than 0")]
        public int Quantity { get; set; }

        [AllowedExtensionAttribute(MyFileSettings.AllowedExtensions)]
        [MaxFileSizeAttribute(MyFileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;

    }
}
