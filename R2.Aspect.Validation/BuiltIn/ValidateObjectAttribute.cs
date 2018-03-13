using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace R2.Aspect.Validation.BuiltIn
{
    public class ValidateObjectAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // If the property is required, it should be validated with RequiredAttribute
                return ValidationResult.Success;
            }

            var results = new List<ValidationResult>();
            var context = new ValidationContext(value);

            var isValid = Validator.TryValidateObject(value, context, results, validateAllProperties: true);

            return isValid
                ? ValidationResult.Success
                : new CompositeValidationResult(results);
        }
    }
}