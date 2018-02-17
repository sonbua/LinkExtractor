using System.Threading.Tasks;
using Castle.Windsor;
using LinkExtractor.Core.IoC;

namespace LinkExtractor.Core
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IWindsorContainer _container;

        public RequestProcessor(IWindsorContainer container)
        {
            _container = container;
        }

        public async Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest<TResponse>
        {
            var requestHandlerType = typeof(IRequestHandler<TRequest, TResponse>);

            using (var scope = Container.BeginCustomScope())
            {
                var requestHandler =
                    (IRequestHandler<TRequest, TResponse>) _container.Resolve(requestHandlerType).TrackedBy(scope);

                return await requestHandler.HandleAsync(request);
            }
        }
    }
}