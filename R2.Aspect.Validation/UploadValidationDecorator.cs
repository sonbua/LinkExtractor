using System.Collections.Generic;

namespace R2.Aspect.Validation
{
    public class UploadValidationDecorator<TUpload, TResult>
        : RequestValidationDecorator<TUpload, TResult>,
            IUploadHandler<TUpload, TResult>
        where TUpload : IUpload<TResult>
    {
        public UploadValidationDecorator(
            IEnumerable<IValidator<TUpload>> validators,
            IUploadHandler<TUpload, TResult> inner)
            : base(validators, inner)
        {
        }
    }
}