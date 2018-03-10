using System.Collections.Generic;

namespace R2.Aspect.Preprocessing
{
    public class CommandPreprocessingDecorator<TCommand>
        : RequestPreprocessingDecorator<TCommand, VoidReturn>,
            ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public CommandPreprocessingDecorator(
            IEnumerable<IPreprocessor<TCommand>> preprocessors,
            ICommandHandler<TCommand> inner)
            : base(preprocessors, inner)
        {
        }
    }
}