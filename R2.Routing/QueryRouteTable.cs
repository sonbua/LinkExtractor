using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace R2.Routing
{
    public class QueryRouteTable : IRouteTable
    {
        private static ConcurrentDictionary<string, RouteEntry> _table;

        private readonly IEnumerable<IQuery> _queryComponents;
        private readonly IRouteHandler _routeHandler;

        public QueryRouteTable(IEnumerable<IQuery> queryComponents, IRouteHandler routeHandler)
        {
            _queryComponents = queryComponents;
            _routeHandler = routeHandler;
        }

        public ConcurrentDictionary<string, RouteEntry> Table => _table ?? (_table = InitializeTable());

        private ConcurrentDictionary<string, RouteEntry> InitializeTable()
        {
            var routeEntries =
                from component in _queryComponents
                let componentType = component.GetType()
                where componentType.Name.EndsWith("Query")
                let serviceType = componentType.GetInterfaces().Single(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>)
                )
                let resultType = serviceType.GenericTypeArguments[0]
                let handlerType = typeof(IQueryHandler<,>).MakeGenericType(componentType, resultType)
                from routePath in _routeHandler.Handle(componentType)
                select new RouteEntry
                {
                    RoutePath = routePath,
                    RequestType = componentType,
                    HandlerType = handlerType
                };

            return new ConcurrentDictionary<string, RouteEntry>(
                routeEntries.ToDictionary(routeEntry => routeEntry.RoutePath, routeEntry => routeEntry)
            );
        }
    }
}