namespace R2
{
    public abstract class BaseCommandHandler<TCommand> 
        : BaseRequestHandler<TCommand, Nothing<TCommand>>,
            ICommandHandler<TCommand>
    {
    }
}