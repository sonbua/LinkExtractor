using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteConventionallyFromQueryTypeNameWithoutSuffix : IRouteHandler
    {
        public IEnumerable<string> Handle(Type queryType, Func<Type, IEnumerable<string>> next) =>
            IsQueryType(queryType)
                ? new[] {queryType.Name.Substring(0, queryType.Name.Length - 5)}
                : next.Invoke(queryType);

        private static bool IsQueryType(Type queryType) =>
            typeof(IQuery).IsAssignableFrom(queryType)
            && queryType.Name.EndsWith("Query");
    }
}