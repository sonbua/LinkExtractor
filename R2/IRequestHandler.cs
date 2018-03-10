using System.Threading.Tasks;

namespace R2
{
    public interface IRequestHandler
    {
        Task<object> HandleAsync(object request);
    }

    public interface IRequestHandler<TRequest, TResponse> : IRequestHandler
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}