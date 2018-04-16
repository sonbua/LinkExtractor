using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteFromQueryTypeName : IRouteHandler
    {
        public bool CanHandle(Type queryType) => typeof(IQuery).IsAssignableFrom(queryType);

        public IEnumerable<string> Handle(Type queryType)
        {
            yield return queryType.Name;
        }
    }
}