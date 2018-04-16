using System.Collections.Concurrent;

namespace R2.Routing
{
    public interface IRouteTable
    {
        ConcurrentDictionary<string, RouteEntry> Table { get; }
    }
}