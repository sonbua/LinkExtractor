using System;
using System.Threading.Tasks;

namespace LinkExtractor.Core
{
    public interface IRequestProcessor
    {
        Task<TResponse> ProcessAsync<TRequest, TResponse>(TRequest request)
            where TResponse : IResponse<TRequest>;

        Task<object> ProcessAsync(object request, Type requestHandlerType);
    }
}