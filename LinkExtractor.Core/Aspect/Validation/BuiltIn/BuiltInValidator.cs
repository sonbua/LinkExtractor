namespace LinkExtractor.Core.Aspect.Validation.BuiltIn
{
    public class BuiltInValidator<TRequest> : RuleBasedValidator<TRequest>
    {
        public BuiltInValidator()
        {
            AddRule<RequestMustBeNotNullRule<TRequest>>();
            AddRule<DataAnnotationValidationMustPassRule<TRequest>>();
        }
    }
}