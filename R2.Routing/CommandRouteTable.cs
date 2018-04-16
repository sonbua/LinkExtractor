using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ResponsibilityChain;

namespace R2.Routing
{
    public class CommandRouteTable : IRouteTable
    {
        private static ConcurrentDictionary<string, RouteEntry> _table;

        private readonly IEnumerable<ICommand> _commandComponents;
        private readonly IChainHandler<Type, IEnumerable<string>> _routeHandler;

        public CommandRouteTable(
            IEnumerable<ICommand> commandComponents,
            IChainHandler<Type, IEnumerable<string>> routeHandler)
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
                where componentType.Name.EndsWith("Command")
                from routePath in _routeHandler.Handle(componentType)
                select new RouteEntry
                {
                    RoutePath = routePath,
                    RequestType = componentType,
                    HandlerType = typeof(ICommandHandler<>).MakeGenericType(componentType)
                };

            return new ConcurrentDictionary<string, RouteEntry>(
                routeEntries.ToDictionary(routeEntry => routeEntry.RoutePath, routeEntry => routeEntry)
            );
        }
    }
}