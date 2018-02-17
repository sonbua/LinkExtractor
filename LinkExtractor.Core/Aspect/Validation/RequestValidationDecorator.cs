using System.Threading.Tasks;
using Castle.Windsor;

namespace LinkExtractor.Core.Aspect.Validation
{
    public class RequestValidationDecorator : IRequestProcessor
    {
        private readonly IRequestProcessor _inner;
        private readonly IWindsorContainer _container;

        public RequestValidationDecorator(IRequestProcessor inner, IWindsorContainer container)
        {
            _inner = inner;
            _container = container;
        }

        public async Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            // LIFESTYLE: validators are registered with scoped lifestyle
            var validators = _container.ResolveAll<IValidator<TRequest>>();

            foreach (var validator in validators)
            {
                await validator.ValidateAsync(request);
            }

            return await _inner.ProcessAsync<TRequest, TResponse>(request);
        }
    }
}