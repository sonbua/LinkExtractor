using System;
using Cqrs.Aspect.Validation;

namespace LinkExtractor.Instagram.Validation
{
    public class InstagramRequestValidator : RuleBasedValidator<InstagramRequest>
    {
        public InstagramRequestValidator(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}