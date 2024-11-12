using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utils
{
    public class MaxFileSizeAttribute(int maxFileSize) : ValidationAttribute
    {
        private readonly int _maxFileSize = maxFileSize;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not IFormFile file) return ValidationResult.Success;
            return file.Length > (_maxFileSize * 1024 * 1024) ? new ValidationResult($"Maximum allowed file size is {_maxFileSize} MB.") : ValidationResult.Success;
        }
    }
}
