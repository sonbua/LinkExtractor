namespace R2
{
    public interface IUploadHandler
    {
    }

    public interface IUploadHandler<TUpload, TResult> : IRequestHandler<TUpload, TResult>, IUploadHandler
        where TUpload : IRequest<TResult>
    {
    }
}