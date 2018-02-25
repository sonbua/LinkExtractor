﻿using System.Threading.Tasks;

namespace LinkExtractor.Core
{
    public abstract class BaseRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TResponse : IResponse<TRequest>
    {
        public abstract Task<TResponse> HandleAsync(TRequest request);

        public async Task<object> HandleAsync(object request)
        {
            return await HandleAsync((TRequest) request);
        }
    }
}