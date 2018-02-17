using System.Threading.Tasks;

namespace LinkExtractor.Core
{
    // TODO: in need?
    public interface IRequestHandler
    {
    }

    public interface IRequestHandler<TRequest, TResponse> : IRequestHandler
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}