using ShoppingApp.Attributes;
using ShoppingApp.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingApp.ViewModels
{
    public class ProductFormViewModel : ProductViewModel
    {
       
  

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative number")]
        public int Quantity { get; set; }


        [AllowedExtensionAttribute(MyFileSettings.AllowedExtensions)]
        [MaxFileSizeAttribute(MyFileSettings.MaxFileSizeInBytes)]
        public IFormFile Cover { get; set; } = default!;

    }
}
