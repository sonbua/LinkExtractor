using System;
using System.Threading.Tasks;
using Cqrs.DependencyInjection;

namespace Cqrs
{
    public class RequestProcessor : IRequestProcessor
    {
        private readonly IServiceProvider _serviceProvider;

        public RequestProcessor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse<TRequest>
        {
            var requestHandler = _serviceProvider.GetService<IRequestHandler<TRequest, TResponse>>();

            return await requestHandler.HandleAsync(request);
        }

        public async Task<object> ProcessAsync(object request, Type requestHandlerType)
        {
            var requestHandler = (IRequestHandler) _serviceProvider.GetService(requestHandlerType);

            return await requestHandler.HandleAsync(request);
        }
    }
}