using System;
using System.Collections.Generic;
using ResponsibilityChain;

namespace R2.Routing
{
    public interface IRouteHandler : IChainHandler<Type, IEnumerable<string>>
    {
    }
}