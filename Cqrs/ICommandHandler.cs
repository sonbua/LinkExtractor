namespace Cqrs
{
    public interface ICommandHandler
    {
    }

    public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Nothing<TCommand>>, ICommandHandler
    {
    }
}