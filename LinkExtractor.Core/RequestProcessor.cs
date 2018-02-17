using System.Threading.Tasks;
using Castle.Windsor;

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
            // LIFESTYLE: handlers are registered with scoped lifestyle
            var requestHandler = _container.Resolve<IRequestHandler<TRequest, TResponse>>();

            return await requestHandler.HandleAsync(request);
        }
    }
}