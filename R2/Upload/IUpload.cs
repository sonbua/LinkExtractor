using System.Collections.Generic;

namespace R2
{
    public interface IUpload
    {
        IReadOnlyList<IFile> Files { get; }
    }

    public interface IUpload<TResult> : IRequest<TResult>, IUpload
    {
    }
}