using System;
using System.Reflection;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LinkExtractor.Core.Aspect.Caching
{
    public class RequestCachingDecorator<TRequest, TResponse> : BaseRequestHandler<TRequest, TResponse>
        where TResponse : IResponse<TRequest>
    {
        private readonly IRequestHandler<TRequest, TResponse> _inner;
        private readonly MemoryCache _memoryCache;

        public RequestCachingDecorator(IRequestHandler<TRequest, TResponse> inner, MemoryCache memoryCache)
        {
            _inner = inner;
            _memoryCache = memoryCache;
        }

        public override async Task<TResponse> HandleAsync(TRequest request)
        {
            var requestType = typeof(TRequest);
            var cacheableResponseAttribute = requestType.GetCustomAttribute<CacheableResponseAttribute>();

            if (cacheableResponseAttribute == null)
            {
                return await _inner.HandleAsync(request);
            }

            return await HandleCoreAsync(requestType.FullName, request, cacheableResponseAttribute.Duration);
        }

        private async Task<TResponse> HandleCoreAsync(string requestTypeFullName, TRequest request, int cacheDuration)
        {
            var cacheKey = GetCacheKey(requestTypeFullName, request);
            var cacheItem = _memoryCache.GetCacheItem(cacheKey);

            if (cacheItem != null)
            {
                return (TResponse) cacheItem.Value;
            }

            return await HandleRequestAsync(request, cacheKey, cacheDuration);
        }

        private async Task<TResponse> HandleRequestAsync(TRequest request, string cacheKey, int cacheDuration)
        {
            var response = await _inner.HandleAsync(request);

            _memoryCache.Set(
                new CacheItem(cacheKey, response),
                new CacheItemPolicy
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(cacheDuration)
                }
            );

            return response;
        }

        private string GetCacheKey(string requestTypeFullName, TRequest request) =>
            $"{requestTypeFullName}__{JsonConvert.SerializeObject(request)}";
    }
}