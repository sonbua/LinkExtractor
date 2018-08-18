using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class ThrowNotSupportedRouteHandler : IRouteHandler
    {
        static ThrowNotSupportedRouteHandler()
        {
        }

        private ThrowNotSupportedRouteHandler()
        {
        }

        public static IRouteHandler Instance { get; } = new ThrowNotSupportedRouteHandler();

        public IEnumerable<string> Handle(Type request, Func<Type, IEnumerable<string>> next)
        {
            throw new NotSupportedException($"Cannot handle this route: {request}");
        }
    }
}