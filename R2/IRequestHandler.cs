using System.Threading.Tasks;

namespace R2
{
    public interface IRequestHandler
    {
        Task<object> HandleAsync(object request);
    }

    public interface IRequestHandler<TRequest, TResponse> : IRequestHandler
        where TResponse : IResponse<TRequest>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
}