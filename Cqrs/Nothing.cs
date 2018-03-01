namespace Cqrs
{
    public class Nothing<TCommand> : IResponse<TCommand>
    {
        public static Nothing<TCommand> Instance = new Nothing<TCommand>();
    }
}