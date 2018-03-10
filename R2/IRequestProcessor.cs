using System;
using System.Threading.Tasks;

namespace R2
{
    public interface IRequestProcessor
    {
        Task<TResult> ProcessQueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQuery<TResult>;

        Task<object> ProcessQueryAsync(object query, Type queryHandlerType);

        Task ProcessCommandAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task ProcessCommandAsync(object command);
    }
}