using System.Threading.Tasks;

namespace R2
{
    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> HandleAsync(TRequest request);

        public async Task<object> HandleAsync(object request)
        {
            return await HandleAsync((TRequest) request);
        }
    }
}