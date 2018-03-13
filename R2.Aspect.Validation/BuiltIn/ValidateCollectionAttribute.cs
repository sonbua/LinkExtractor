using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace R2.Aspect.Validation.BuiltIn
{
    public class ValidateCollectionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // If the property is required, it should be validated with RequiredAttribute
                return ValidationResult.Success;
            }

            if (!(value is IEnumerable enumerable))
            {
                return new ValidationResult($"This property {validationContext.DisplayName} is not enumerable.");
            }

            var results = new List<ValidationResult>();

            foreach (var item in enumerable)
            {
                var context = new ValidationContext(item);

                Validator.TryValidateObject(item, context, results, validateAllProperties: true);
            }

            return results.Any()
                ? new CompositeValidationResult(results)
                : ValidationResult.Success;
        }
    }
}