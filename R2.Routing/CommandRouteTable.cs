using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace R2.Routing
{
    public class CommandRouteTable : IRouteTable
    {
        private static ConcurrentDictionary<string, RouteEntry> _table;

        private readonly IEnumerable<ICommand> _commandComponents;
        private readonly IRouteHandler _routeHandler;

        public CommandRouteTable(IEnumerable<ICommand> commandComponents, IRouteHandler routeHandler)
        {
            _commandComponents = commandComponents;
            _routeHandler = routeHandler;
        }

        public ConcurrentDictionary<string, RouteEntry> Table => _table ?? (_table = InitializeTable());

        private ConcurrentDictionary<string, RouteEntry> InitializeTable()
        {
            var routeEntries =
                from component in _commandComponents
                let componentType = component.GetType()
                from routePath in _routeHandler.Handle(componentType)
                select new RouteEntry
                {
                    RoutePath = routePath.ToLower(),
                    RequestType = componentType,
                    HandlerType = typeof(ICommandHandler<>).MakeGenericType(componentType)
                };

            return new ConcurrentDictionary<string, RouteEntry>(
                routeEntries.ToDictionary(routeEntry => routeEntry.RoutePath, routeEntry => routeEntry)
            );
        }
    }
}