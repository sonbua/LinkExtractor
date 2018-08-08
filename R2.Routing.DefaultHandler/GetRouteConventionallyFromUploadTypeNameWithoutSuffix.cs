using System;
using System.Collections.Generic;

namespace R2.Routing
{
    public class GetRouteConventionallyFromUploadTypeNameWithoutSuffix : IRouteHandler
    {
        public bool CanHandle(Type uploadType) =>
            typeof(IUpload).IsAssignableFrom(uploadType)
            && uploadType.Name.EndsWith("Upload");

        public IEnumerable<string> Handle(Type uploadType)
        {
            yield return uploadType.Name.Substring(0, uploadType.Name.Length - 6);
        }
    }
}