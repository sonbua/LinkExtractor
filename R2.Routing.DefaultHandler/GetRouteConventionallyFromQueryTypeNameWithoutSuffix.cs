using System;
using System.Collections.Generic;
using ResponsibilityChain;

namespace R2.Routing
{
    public class GetRouteConventionallyFromQueryTypeNameWithoutSuffix : IRouteHandler
    {
        public bool CanHandle(Type queryType) =>
            typeof(IQuery).IsAssignableFrom(queryType)
            && queryType.Name.EndsWith("Query");

        public IEnumerable<string> Handle(Type queryType)
        {
            yield return queryType.Name.Substring(0, queryType.Name.Length - 5);
        }
    }
}