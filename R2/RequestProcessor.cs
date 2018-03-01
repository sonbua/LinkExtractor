using System;
using System.Threading.Tasks;
using R2.DependencyInjection;

namespace R2
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

        public async Task ProcessCommandAsync<TCommand>(TCommand command)
        {
            var requestHandler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

            await requestHandler.HandleAsync(command);
        }

        public async Task ProcessCommandAsync(object command)
        {
            var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var requestHandler = (IRequestHandler) _serviceProvider.GetService(commandHandlerType);

            await requestHandler.HandleAsync(command);
        }
    }
}