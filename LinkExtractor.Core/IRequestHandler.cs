using System.Threading.Tasks;

namespace LinkExtractor.Core
{
    public interface IRequestHandler
    {
    }

    public interface IRequestHandler<TRequest, TResponse> : IRequestHandler
        where TResponse : IResponse<TRequest>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}