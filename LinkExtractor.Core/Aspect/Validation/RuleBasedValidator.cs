using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.Windsor;
using LinkExtractor.Core.IoC;

namespace LinkExtractor.Core.Aspect.Validation
{
    public abstract class RuleBasedValidator<TRequest> : IValidator<TRequest>
    {
        private const string _INVALID_VALIDATION_RULE_TYPE = "Validation rule type {0} is not of type {1}.";

        private readonly IWindsorContainer _container;
        private readonly List<IValidationRule<TRequest>> _validationRules;

        protected RuleBasedValidator()
        {
            _container = Container.Instance;
            _validationRules = new List<IValidationRule<TRequest>>();
        }

        public async Task ValidateAsync(TRequest request)
        {
            foreach (var rule in _validationRules)
            {
                await rule.TestAsync(request);
            }
        }

        protected void AddRule<TRule>()
            where TRule : IValidationRule<TRequest>
        {
            var rule = _container.Resolve<TRule>();

            _validationRules.Add(rule);
        }

        protected void AddRule(Type ruleType)
        {
            RequireOfTypeIValidationRule(ruleType);

            var rule = (IValidationRule<TRequest>) _container.Resolve(ruleType);

            _validationRules.Add(rule);
        }

        private static void RequireOfTypeIValidationRule(Type ruleType)
        {
            var baseRuleType = typeof(IValidationRule<TRequest>);

            if (!baseRuleType.IsAssignableFrom(ruleType))
            {
                throw new ComponentRegistrationException(
                    string.Format(_INVALID_VALIDATION_RULE_TYPE, ruleType, baseRuleType)
                );
            }
        }
    }
}