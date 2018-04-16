using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteFromCommandTypeName : IRouteHandler
    {
        public bool CanHandle(Type commandType) => typeof(ICommand).IsAssignableFrom(commandType);

        public IEnumerable<string> Handle(Type commandType)
        {
            yield return commandType.Name;
        }
    }
}