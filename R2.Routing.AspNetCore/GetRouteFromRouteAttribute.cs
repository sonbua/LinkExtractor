using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace R2.Routing.AspNetCore
{
    public class GetRouteFromRouteAttribute : IRouteHandler
    {
        public IEnumerable<string> Handle(Type requestType, Func<Type, IEnumerable<string>> next) =>
            HasRouteAttribute(requestType, out var routeAttributes)
                ? HandleImpl(routeAttributes)
                : next.Invoke(requestType);

        private static bool HasRouteAttribute(Type requestType, out IEnumerable<RouteAttribute> routeAttributes)
        {
            routeAttributes = requestType.GetCustomAttributes<RouteAttribute>(inherit: true);

            return routeAttributes.Any();
        }

        private static IEnumerable<string> HandleImpl(IEnumerable<RouteAttribute> routeAttributes)
        {
            foreach (var routeAttribute in routeAttributes)
            {
                if (routeAttribute.Prefix == string.Empty)
                {
                    yield return routeAttribute.Template;
                }
                else
                {
                    yield return string.Join("/", routeAttribute.Prefix, routeAttribute.Template);
                }
            }
        }
    }
}