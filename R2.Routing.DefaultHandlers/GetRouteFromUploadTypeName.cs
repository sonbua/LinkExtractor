using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteFromUploadTypeName : IRouteHandler
    {
        public IEnumerable<string> Handle(Type uploadType, Func<Type, IEnumerable<string>> next) =>
            IsUploadType(uploadType)
                ? new[] {uploadType.Name}
                : next.Invoke(uploadType);

        private static bool IsUploadType(Type uploadType) =>
            typeof(IUpload).IsAssignableFrom(uploadType);
    }
}