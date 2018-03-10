using System.Collections.Generic;
using System.Threading.Tasks;

namespace R2.Aspect.Postprocessing
{
    public class RequestPostprocessingDecorator<TRequest, TResponse> : RequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly IEnumerable<IPostprocessor<TRequest, TResponse>> _postprocessors;

        public RequestPostprocessingDecorator(
            IRequestHandler<TRequest, TResponse> inner,
            IEnumerable<IPostprocessor<TRequest, TResponse>> postprocessors)
        {
            _inner = inner;
            _postprocessors = postprocessors;
        }

        public override async Task<TResponse> HandleAsync(TRequest request)
        {
            var response = await _inner.HandleAsync(request);

            foreach (var postprocessor in _postprocessors)
            {
                await postprocessor.ProcessAsync(request, response);
            }

            return response;
        }
    }
}