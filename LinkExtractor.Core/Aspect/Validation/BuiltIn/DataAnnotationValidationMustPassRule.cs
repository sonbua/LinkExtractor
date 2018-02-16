using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace LinkExtractor.Core.Aspect.Validation.BuiltIn
{
    public class DataAnnotationValidationMustPassRule<TRequest> : IValidationRule<TRequest>
    {
        public Task TestAsync(TRequest request)
        {
            var validationContext = new ValidationContext(request, null, null);
            var validationResults = new List<ValidationResult>();

            var requestIsValid =
                Validator.TryValidateObject(request, validationContext, validationResults, validateAllProperties: true);

            if (requestIsValid)
            {
                return Task.CompletedTask;
            }

            throw new CompositeValidationException(validationResults);
        }
    }
}