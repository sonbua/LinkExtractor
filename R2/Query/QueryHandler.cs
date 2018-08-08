namespace R2
{
    public abstract class QueryHandler<TQuery, TResult>
        : RequestHandler<TQuery, TResult>,
            IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
    }
}