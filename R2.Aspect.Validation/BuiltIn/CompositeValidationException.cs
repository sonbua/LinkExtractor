using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace R2.Aspect.Validation.BuiltIn
{
    public class CompositeValidationException : ValidationException
    {
        public CompositeValidationException(List<ValidationResult> validationResults)
            : base(validationResults[0], validatingAttribute: null, value: null)
        {
            ValidationResults = validationResults;
        }

        public List<ValidationResult> ValidationResults { get; }
    }
}