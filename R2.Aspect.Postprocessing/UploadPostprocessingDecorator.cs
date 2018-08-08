using System.Collections.Generic;

namespace R2.Aspect.Postprocessing
{
    public class UploadPostprocessingDecorator<TUpload, TResult>
        : RequestPostprocessingDecorator<TUpload, TResult>,
            IUploadHandler<TUpload, TResult>
        where TUpload : IUpload<TResult>
    {
        public UploadPostprocessingDecorator(
            IUploadHandler<TUpload, TResult> inner,
            IEnumerable<IPostprocessor<TUpload, TResult>> postprocessors)
            : base(inner, postprocessors)
        {
        }
    }
}