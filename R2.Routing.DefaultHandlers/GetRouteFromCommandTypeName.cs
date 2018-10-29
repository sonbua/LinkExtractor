using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteFromCommandTypeName : IRouteHandler
    {
        public IEnumerable<string> Handle(Type commandType, Func<Type, IEnumerable<string>> next) =>
            IsCommandType(commandType)
                ? new[] {commandType.Name}
                : next.Invoke(commandType);

        private static bool IsCommandType(Type commandType) =>
            typeof(ICommand).IsAssignableFrom(commandType);
    }
}