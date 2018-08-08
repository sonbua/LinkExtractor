using System.Collections.Generic;

namespace R2.Aspect.Preprocessing
{
    public class UploadPreprocessingDecorator<TUpload, TResult>
        : RequestPreprocessingDecorator<TUpload, TResult>,
            IUploadHandler<TUpload, TResult>
        where TUpload : IUpload<TResult>
    {
        public UploadPreprocessingDecorator(
            IEnumerable<IPreprocessor<TUpload>> preprocessors,
            IUploadHandler<TUpload, TResult> inner)
            : base(preprocessors, inner)
        {
        }
    }
}