using System;
using System.Threading.Tasks;

namespace R2
{
    public interface IRequestProcessor
    {
        Task<TResult> ProcessQueryAsync<TResult>(IQuery<TResult> query);

        Task<object> ProcessQueryAsync(object query, Type queryHandlerType);

        Task ProcessCommandAsync<TCommand>(TCommand command)
            where TCommand : ICommand;

        Task ProcessCommandAsync(object command, Type commandHandlerType);

        Task<object> ProcessUpload(object upload, Type uploadHandlerType);
    }
}