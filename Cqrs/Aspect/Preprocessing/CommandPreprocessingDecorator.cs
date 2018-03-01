using System.Collections.Generic;

namespace Cqrs.Aspect.Preprocessing
{
    public class CommandPreprocessingDecorator<TCommand>
        : RequestPreprocessingDecorator<TCommand, Nothing<TCommand>>,
            ICommandHandler<TCommand>
    {
        public CommandPreprocessingDecorator(
            IEnumerable<IPreprocessor<TCommand>> preprocessors,
            ICommandHandler<TCommand> inner)
            : base(preprocessors, inner)
        {
        }
    }
}