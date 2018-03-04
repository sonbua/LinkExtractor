using System.Threading.Tasks;

namespace R2
{
    public abstract class BaseCommandHandler<TCommand>
        : BaseRequestHandler<TCommand, Nothing<TCommand>>,
            ICommandHandler<TCommand>
    {
        public override async Task<Nothing<TCommand>> HandleAsync(TCommand request)
        {
            await HandleCommandAsync(request);

            return Nothing<TCommand>.Instance;
        }

        protected abstract Task HandleCommandAsync(TCommand command);
    }
}