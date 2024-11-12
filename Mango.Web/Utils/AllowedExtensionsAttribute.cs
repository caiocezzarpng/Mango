using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Utils
{
    public class AllowedExtensionsAttribute(string[] extensions) : ValidationAttribute
    {
        private readonly string[] _extensions = extensions;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not FormFile file) return ValidationResult.Success;
            var extension = Path.GetExtension(file.FileName);
            return !_extensions.Contains((extension.ToLower())) ? new ValidationResult("This photo extension is not allowed!") : ValidationResult.Success;
        }
    }
}
