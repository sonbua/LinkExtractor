using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace R2.Routing
{
    public class UploadRouteTable : IRouteTable
    {
        private static ConcurrentDictionary<string, RouteEntry> _table;

        private readonly IEnumerable<IUpload> _uploadComponents;
        private readonly IRouteHandler _routeHandler;

        public UploadRouteTable(IEnumerable<IUpload> uploadComponents, IRouteHandler routeHandler)
        {
            _uploadComponents = uploadComponents;
            _routeHandler = routeHandler;
        }

        public ConcurrentDictionary<string, RouteEntry> Table => _table ?? (_table = InitializeTable());

        private ConcurrentDictionary<string, RouteEntry> InitializeTable()
        {
            IEnumerable<string> ThrowNotSupported(Type request) =>
                ThrowNotSupportedRouteHandler.Instance.Handle(request, null);

            var routeEntries =
                from component in _uploadComponents
                let componentType = component.GetType()
                let serviceType = componentType.GetInterfaces().Single(
                    i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IUpload<>)
                )
                let resultType = serviceType.GenericTypeArguments[0]
                let handlerType = typeof(IUploadHandler<,>).MakeGenericType(componentType, resultType)
                from routePath in _routeHandler.Handle(componentType, ThrowNotSupported)
                select new RouteEntry
                {
                    RoutePath = routePath.ToLower(),
                    RequestType = componentType,
                    HandlerType = handlerType
                };

            return new ConcurrentDictionary<string, RouteEntry>(
                routeEntries.ToDictionary(routeEntry => routeEntry.RoutePath, routeEntry => routeEntry)
            );
        }
    }
}