using System;

namespace R2.Routing
{
    public class RouteEntry
    {
        public string RoutePath { get; set; }

        public Type RequestType { get; set; }

        public Type HandlerType { get; set; }
    }
}