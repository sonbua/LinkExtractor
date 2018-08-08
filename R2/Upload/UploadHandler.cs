namespace R2
{
    public abstract class UploadHandler<TUpload, TResult>
        : RequestHandler<TUpload, TResult>,
            IUploadHandler<TUpload, TResult>
        where TUpload : IRequest<TResult>
    {
    }
}