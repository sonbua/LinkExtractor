using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteFromQueryTypeName : IRouteHandler
    {
        public IEnumerable<string> Handle(Type queryType, Func<Type, IEnumerable<string>> next) =>
            IsQueryType(queryType)
                ? new[] {queryType.Name}
                : next.Invoke(queryType);

        private static bool IsQueryType(Type queryType) =>
            typeof(IQuery).IsAssignableFrom(queryType);
    }
}