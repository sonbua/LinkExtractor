using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace R2.Routing.AspNetCore
{
    public class GetRouteFromRouteAttribute : IRouteHandler
    {
        private IEnumerable<RouteAttribute> _routeAttributes;

        public GetRouteFromRouteAttribute()
        {
            _routeAttributes = Enumerable.Empty<RouteAttribute>();
        }

        public bool CanHandle(Type requestType)
        {
            _routeAttributes = requestType.GetCustomAttributes<RouteAttribute>(inherit: true);

            return _routeAttributes.Any();
        }

        public IEnumerable<string> Handle(Type requestType)
        {
            foreach (var routeAttribute in _routeAttributes)
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