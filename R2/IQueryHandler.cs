namespace R2
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, TResult>, IQueryHandler
        where TQuery : IQuery<TResult>
    {
    }
}