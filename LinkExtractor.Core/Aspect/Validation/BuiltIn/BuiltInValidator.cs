namespace LinkExtractor.Core.Aspect.Validation.BuiltIn
{
    public class BuiltInValidator<TRequest> : RuleBasedValidator<TRequest>
    {
        public BuiltInValidator()
        {
            AddRule<RequestMustNotBeNullRule<TRequest>>();
            AddRule<DataAnnotationValidationMustPassRule<TRequest>>();
        }
    }
}