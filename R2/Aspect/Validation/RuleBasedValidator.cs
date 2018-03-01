using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R2.DependencyInjection;

namespace R2.Aspect.Validation
{
    public abstract class RuleBasedValidator<TRequest> : IValidator<TRequest>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<IValidationRule<TRequest>> _validationRules;

        protected RuleBasedValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
            var rule = _serviceProvider.GetService<TRule>();

            _validationRules.Add(rule);
        }
    }
}