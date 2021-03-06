﻿using System;
using System.Reflection;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace R2.Aspect.Caching
{
    public class RequestCachingDecorator<TRequest, TResponse> : RequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly MemoryCache _memoryCache;
        private readonly IRequestHandler<TRequest, TResponse> _inner;

        public RequestCachingDecorator(MemoryCache memoryCache, IRequestHandler<TRequest, TResponse> inner)
        {
            _memoryCache = memoryCache;
            _inner = inner;
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