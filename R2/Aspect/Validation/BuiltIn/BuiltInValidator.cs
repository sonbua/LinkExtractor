using System;

namespace R2.Aspect.Validation.BuiltIn
{
    public class BuiltInValidator<TRequest> : RuleBasedValidator<TRequest>
    {
        public BuiltInValidator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            AddRule<RequestMustBeNotNullRule<TRequest>>();
            AddRule<DataAnnotationValidationMustPassRule<TRequest>>();
        }
    }
}