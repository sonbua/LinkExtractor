using System.Collections.Generic;
using System.Threading.Tasks;

namespace R2.Aspect.Preprocessing
{
    public class RequestPreprocessingDecorator<TRequest, TResponse> : RequestHandler<TRequest, TResponse>
        where TResponse : IResponse<TRequest>
    {
        private readonly IEnumerable<IPreprocessor<TRequest>> _preprocessors;
        private readonly IRequestHandler<TRequest, TResponse> _inner;

        public RequestPreprocessingDecorator(
            IEnumerable<IPreprocessor<TRequest>> preprocessors,
            IRequestHandler<TRequest, TResponse> inner)
        {
            _preprocessors = preprocessors;
            _inner = inner;
        }

        public override async Task<TResponse> HandleAsync(TRequest request)
        {
            foreach (var preprocessor in _preprocessors)
            {
                await preprocessor.ProcessAsync(request);
            }

            return await _inner.HandleAsync(request);
        }
    }
}