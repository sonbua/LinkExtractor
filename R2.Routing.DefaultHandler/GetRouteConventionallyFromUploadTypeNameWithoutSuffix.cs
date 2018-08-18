using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteConventionallyFromUploadTypeNameWithoutSuffix : IRouteHandler
    {
        public IEnumerable<string> Handle(Type uploadType, Func<Type, IEnumerable<string>> next) =>
            IsUploadType(uploadType)
                ? new[] {uploadType.Name.Substring(0, uploadType.Name.Length - 6)}
                : next.Invoke(uploadType);

        private static bool IsUploadType(Type uploadType) =>
            typeof(IUpload).IsAssignableFrom(uploadType)
            && uploadType.Name.EndsWith("Upload");
    }
}