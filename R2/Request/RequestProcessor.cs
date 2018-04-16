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

        public async Task<TResult> ProcessQueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>
        {
            var queryHandler = _serviceProvider.GetService<IRequestHandler<TQuery, TResult>>();

            return await queryHandler.HandleAsync(query);
        }

        public async Task<object> ProcessQueryAsync(object query, Type queryHandlerType)
        {
            var queryHandler = (IRequestHandler) _serviceProvider.GetService(queryHandlerType);

            return await queryHandler.HandleAsync(query);
        }

        public async Task ProcessCommandAsync<TCommand>(TCommand command)
            where TCommand : ICommand
        {
            var commandHandler = _serviceProvider.GetService<ICommandHandler<TCommand>>();

            await commandHandler.HandleAsync(command);
        }

        public async Task ProcessCommandAsync(object command, Type commandHandlerType)
        {
            var commandHandler = (IRequestHandler) _serviceProvider.GetService(commandHandlerType);

            await commandHandler.HandleAsync(command);
        }
    }
}