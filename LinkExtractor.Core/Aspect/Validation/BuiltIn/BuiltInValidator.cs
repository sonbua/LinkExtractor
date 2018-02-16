namespace LinkExtractor.Core.Aspect.Validation.BuiltIn
{
    public class BuiltInValidator<TRequest> : RuleBasedValidator<TRequest>
    {
        public BuiltInValidator()
        {
            AddRule(typeof(RequestMustNotBeNullRule<TRequest>));
            AddRule(typeof(DataAnnotationValidationMustPassRule<>));
        }
    }
}