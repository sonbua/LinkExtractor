using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LinkExtractor.Core.Aspect.Validation.BuiltIn
{
    public class CompositeValidationException : ValidationException
    {
        public CompositeValidationException(List<ValidationResult> validationResults)
        {
            ValidationResults = validationResults;
        }

        public List<ValidationResult> ValidationResults { get; }
    }
}