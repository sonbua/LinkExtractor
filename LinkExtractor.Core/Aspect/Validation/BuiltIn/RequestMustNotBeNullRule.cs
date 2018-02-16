using System;
using System.Threading.Tasks;

namespace LinkExtractor.Core.Aspect.Validation.BuiltIn
{
    public class RequestMustNotBeNullRule<TRequest> : IValidationRule<TRequest>
    {
        public Task TestAsync(TRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return Task.CompletedTask;
        }
    }
}