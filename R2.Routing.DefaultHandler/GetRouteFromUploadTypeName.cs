using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteFromUploadTypeName : IRouteHandler
    {
        public bool CanHandle(Type uploadType) => typeof(IUpload).IsAssignableFrom(uploadType);

        public IEnumerable<string> Handle(Type uploadType)
        {
            yield return uploadType.Name;
        }
    }
}