namespace R2
{
    /// <summary>
    /// <para>Represents a subset of a collection of objects that can be individually accessed via <see cref="Items"/> property and containing metadata about the superset collection of objects this subset was created from.</para>
    /// <para>This is a customized version of the original X.PagedList library that fits the need of our problem domain.</para>
    /// <para>Reference: https://github.com/kpi-ua/X.PagedList</para>
    /// </summary>
    /// <typeparam name="TItem">The type of object the collection should contain.</typeparam>
    public interface IPagedList<TItem>
    {
        TItem[] Items { get; }

        /// <summary>
        /// Total number of subsets within the superset.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Total number of objects contained within the superset.
        /// </summary>
        int TotalItemCount { get; }

        /// <summary>
        /// One-based index of this subset within the superset.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Maximum size of any individual subset.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Returns true if this is NOT the first subset within the superset.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Returns true if this is NOT the last subset within the superset.
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Returns true if this is the first subset within the superset.
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// Returns true if this is the last subset within the superset.
        /// </summary>
        bool IsLastPage { get; }

        /// <summary>
        /// One-based index of the first item in the paged subset.
        /// </summary>
        int FirstItemOnPage { get; }

        /// <summary>
        /// One-based index of the last item in the paged subset.
        /// </summary>
        int LastItemOnPage { get; }
    }
}