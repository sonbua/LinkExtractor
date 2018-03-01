using System.Collections.Generic;

namespace Cqrs.Aspect.Validation
{
    public class CommandValidationDecorator<TCommand>
        : RequestValidationDecorator<TCommand, Nothing<TCommand>>,
            ICommandHandler<TCommand>
    {
        public CommandValidationDecorator(
            IEnumerable<IValidator<TCommand>> validators,
            ICommandHandler<TCommand> inner)
            : base(validators, inner)
        {
        }
    }
}