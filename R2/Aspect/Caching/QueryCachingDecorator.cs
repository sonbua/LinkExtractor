using System.Runtime.Caching;

namespace R2.Aspect.Caching
{
    public class QueryCachingDecorator<TQuery, TResult>
        : RequestCachingDecorator<TQuery, TResult>,
            IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public QueryCachingDecorator(MemoryCache memoryCache, IQueryHandler<TQuery, TResult> inner)
            : base(memoryCache, inner)
        {
        }
    }
}