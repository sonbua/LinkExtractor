using System;
using System.Threading.Tasks;

namespace R2
{
    public interface IRequestProcessor
    {
        Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse<TRequest>;

        Task<object> ProcessAsync(object request, Type requestHandlerType);

        Task ProcessCommandAsync<TCommand>(TCommand command);

        Task ProcessCommandAsync(object command);
    }
}