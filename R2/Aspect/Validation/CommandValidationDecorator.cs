using System.Collections.Generic;

namespace R2.Aspect.Validation
{
    public class CommandValidationDecorator<TCommand>
        : RequestValidationDecorator<TCommand, VoidReturn>,
            ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public CommandValidationDecorator(
            IEnumerable<IValidator<TCommand>> validators,
            ICommandHandler<TCommand> inner)
            : base(validators, inner)
        {
        }
    }
}