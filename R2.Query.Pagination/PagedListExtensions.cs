using System.Collections.Generic;
using System.Linq;

namespace R2
{
    public static class PagedListExtensions
    {
        public static IPagedList<TItem> ToPagedList<TItem>(
            this IQueryable<TItem> queryable,
            int pageNumber,
            int pageSize) =>
            new AnonymousPagedList<TItem>(queryable, pageNumber, pageSize);

        public static IPagedList<TItem> ToPagedList<TItem>(
            this IEnumerable<TItem> enumerable,
            int pageNumber,
            int pageSize) =>
            new AnonymousPagedList<TItem>(enumerable, pageNumber, pageSize);

        private class AnonymousPagedList<TItem> : PagedList<TItem>
        {
            public AnonymousPagedList(IQueryable<TItem> queryable, int pageNumber, int pageSize)
                : base(queryable, pageNumber, pageSize)
            {
            }

            public AnonymousPagedList(IEnumerable<TItem> enumerable, int pageNumber, int pageSize)
                : base(enumerable, pageNumber, pageSize)
            {
            }
        }
    }
}