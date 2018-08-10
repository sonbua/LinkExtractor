using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace R2
{
    public abstract class PagedList<TItem> : IPagedList<TItem>
    {
        private const string _PAGE_NUMBER_BELOW_1 = "Page number cannot be below 1.";
        private const string _PAGE_SIZE_LESS_THAN_1 = "Page size cannot be less than 1.";

        /// <summary>
        /// This constructor is meant to support ToPagedListAsync extension methods, and should only be exposed as <c>protected internal</c>.
        /// </summary>
        protected internal PagedList()
        {
        }

        protected PagedList(IPagedList<TItem> list)
        {
            PageCount = list.PageCount;
            TotalItemCount = list.TotalItemCount;
            PageNumber = list.PageNumber;
            PageSize = list.PageSize;
            HasPreviousPage = list.HasPreviousPage;
            HasNextPage = list.HasNextPage;
            IsFirstPage = list.IsFirstPage;
            IsLastPage = list.IsLastPage;
            FirstItemOnPage = list.FirstItemOnPage;
            LastItemOnPage = list.LastItemOnPage;
            Items = list.Items;
        }

        protected PagedList(IQueryable<TItem> queryable, int pageNumber, int pageSize)
        {
            EnsureArg.IsNotNull(queryable, nameof(queryable));
            EnsureArg.IsGte(pageNumber, 1, optsFn: options => options.WithMessage(_PAGE_NUMBER_BELOW_1));
            EnsureArg.IsGte(pageSize, 1, optsFn: options => options.WithMessage(_PAGE_SIZE_LESS_THAN_1));

            TotalItemCount = queryable.Count();
            PageSize = pageSize;
            PageCount =
                TotalItemCount > 0
                    ? (int) Math.Ceiling(TotalItemCount / (double) PageSize)
                    : 0;
            PageNumber = pageNumber > PageCount ? PageCount : pageNumber;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber == PageCount;
            FirstItemOnPage = (PageNumber - 1) * PageSize + 1;

            var assumedLastItemOnPage = FirstItemOnPage + PageSize - 1;

            LastItemOnPage =
                assumedLastItemOnPage > TotalItemCount
                    ? TotalItemCount
                    : assumedLastItemOnPage;

            Items = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
        }

        protected PagedList(IEnumerable<TItem> enumerable, int pageNumber, int pageSize)
        {
            EnsureArg.IsGte(pageNumber, 1, optsFn: options => options.WithMessage(_PAGE_NUMBER_BELOW_1));
            EnsureArg.IsGte(pageSize, 1, optsFn: options => options.WithMessage(_PAGE_SIZE_LESS_THAN_1));

            var items = enumerable?.ToArray() ?? new TItem[0];

            TotalItemCount = items.Length;
            PageSize = pageSize;
            PageCount =
                TotalItemCount > 0
                    ? (int) Math.Ceiling(TotalItemCount / (double) PageSize)
                    : 0;
            PageNumber = pageNumber > PageCount ? PageCount : pageNumber;
            HasPreviousPage = PageNumber > 1;
            HasNextPage = PageNumber < PageCount;
            IsFirstPage = PageNumber == 1;
            IsLastPage = PageNumber == PageCount;
            FirstItemOnPage = (PageNumber - 1) * PageSize + 1;

            var assumedLastItemOnPage = FirstItemOnPage + PageSize - 1;

            LastItemOnPage =
                assumedLastItemOnPage > TotalItemCount
                    ? TotalItemCount
                    : assumedLastItemOnPage;

            Items = items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToArray();
        }

        public TItem[] Items { get; protected set; }

        public int PageCount { get; protected set; }

        public int TotalItemCount { get; protected set; }

        public int PageNumber { get; protected set; }

        public int PageSize { get; protected set; }

        public bool HasPreviousPage { get; protected set; }

        public bool HasNextPage { get; protected set; }

        public bool IsFirstPage { get; protected set; }

        public bool IsLastPage { get; protected set; }

        public int FirstItemOnPage { get; protected set; }

        public int LastItemOnPage { get; protected set; }
    }
}