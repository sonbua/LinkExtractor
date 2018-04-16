using System;
using System.Collections.Generic;
using ResponsibilityChain;

namespace R2.Routing
{
    public class GetRouteConventionallyFromCommandTypeNameWithoutSuffix : IRouteHandler
    {
        public bool CanHandle(Type commandType) =>
            typeof(ICommand).IsAssignableFrom(commandType)
            && commandType.Name.EndsWith("Command");

        public IEnumerable<string> Handle(Type commandType)
        {
            yield return commandType.Name.Substring(0, commandType.Name.Length - 7);
        }
    }
}