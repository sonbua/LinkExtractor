using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using LinkExtractor.Core.IoC;

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
            var validatorType = typeof(IValidator<TRequest>);

            using (var scope = Container.BeginCustomScope())
            {
                var validators =
                    ((IValidator[]) _container.ResolveAll(validatorType))
                    .Select(x => x.TrackedBy(scope))
                    .ToArray();

                foreach (var validator in validators)
                {
                    await validator.ValidateAsync(request);
                }
            }

            return await _inner.ProcessAsync<TRequest, TResponse>(request);
        }
    }
}