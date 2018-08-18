using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using EnsureThat;

namespace R2
{
    public static class RequestUtil
    {
        /// <summary>
        /// Attachs files to <see cref="IUpload.Files"/> property of an <see cref="IUpload"/> object;
        /// </summary>
        /// <param name="target">The object, that files are being attached to.</param>
        /// <param name="files">The files to be attached to <see cref="IUpload.Files"/> property on the <paramref name="target"/> object.</param>
        public static void AttachFilesToRequestObject(IUpload target, IList<IFile> files)
        {
            EnsureArg.IsNotNull(target, nameof(target));
            EnsureArg.IsNotNull(files, nameof(files));
            EnsureArg.HasItems(files, optsFn: options => options.WithMessage("File collection is empty."));

            var targetTypeInfo = target.GetType().GetTypeInfo();

            var filesPropertyInfo = targetTypeInfo.GetProperty(nameof(IUpload.Files));

            filesPropertyInfo.SetValue(target, new ReadOnlyCollection<IFile>(files));
        }
    }
}