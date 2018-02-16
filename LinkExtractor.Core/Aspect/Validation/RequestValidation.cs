using System.Linq;
using System.Threading.Tasks;
using Castle.Windsor;
using LinkExtractor.Core.IoC;

namespace LinkExtractor.Core.Aspect.Validation
{
    public class RequestValidation<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IWindsorContainer _container;

        public RequestValidation(IRequestHandler<TRequest, TResponse> inner, IWindsorContainer container)
        {
            _inner = inner;
            _container = container;
        }

        public async Task<TResponse> HandleAsync(TRequest request)
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

            return await _inner.HandleAsync(request);
        }
    }
}