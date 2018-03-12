using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace R2.Aspect.Validation.BuiltIn
{
    public class CompositeValidationResult : ValidationResult
    {
        public CompositeValidationResult(IList<ValidationResult> results)
            : base(results[0])
        {
            Results = results;
        }

        public IList<ValidationResult> Results { get; }
    }
}