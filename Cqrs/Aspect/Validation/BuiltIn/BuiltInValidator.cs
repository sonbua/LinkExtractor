using System;

namespace Cqrs.Aspect.Validation.BuiltIn
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