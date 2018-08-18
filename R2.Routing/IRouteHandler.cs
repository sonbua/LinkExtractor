using System;
using System.Collections.Generic;
using ResponsibilityChain;

namespace R2.Routing
{
    public interface IRouteHandler : IHandler<Type, IEnumerable<string>>
    {
    }
}