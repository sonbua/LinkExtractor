using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteConventionallyFromCommandTypeNameWithoutSuffix : IRouteHandler
    {
        public IEnumerable<string> Handle(Type commandType, Func<Type, IEnumerable<string>> next) =>
            IsCommandType(commandType)
                ? new[] {commandType.Name.Substring(0, commandType.Name.Length - 7)}
                : next.Invoke(commandType);

        private static bool IsCommandType(Type commandType) =>
            typeof(ICommand).IsAssignableFrom(commandType)
            && commandType.Name.EndsWith("Command");
    }
}