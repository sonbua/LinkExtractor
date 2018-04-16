namespace R2
{
    public interface IQuery
    {
    }

    public interface IQuery<TResult> : IRequest<TResult>, IQuery
    {
    }
}