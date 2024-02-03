using System.ComponentModel.DataAnnotations;

namespace ShoppingApp.Attributes
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;
        public MaxFileSizeAttribute(int MaxFileSize)
        {
            _maxFileSize = MaxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;
            if (file is not null)
            {
               
                if (file.Length > _maxFileSize)
                {
                    return new ValidationResult($"Maximum allowed size is {_maxFileSize} bytes");
                }
            }
            return ValidationResult.Success;
        }
    }
}
