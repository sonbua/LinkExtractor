using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using EnsureThat;

namespace R2
{
    public static class RequestUtils
    {
        /// <summary>
        /// Attachs file to an object. A property named "Files" of type <see cref="IReadOnlyList{IFile}"/> on the <paramref name="target"/> object must be present.
        /// </summary>
        /// <param name="target">The object to be assigned a file to.</param>
        /// <param name="files">The files to be assigned to Files property on the <paramref name="target"/> object.</param>
        public static void AttachFilesToRequestObject(IUpload target, IList<IFile> files)
        {
            EnsureArg.IsNotNull(target, nameof(target));
            EnsureArg.IsNotNull(files, nameof(files));
            EnsureArg.HasItems(files, nameof(files), options => options.WithMessage("File collection is empty."));

            var targetTypeInfo = target.GetType().GetTypeInfo();

            var filesPropertyInfo = targetTypeInfo.GetProperty(nameof(IUpload.Files));

            filesPropertyInfo.SetValue(target, new ReadOnlyCollection<IFile>(files));
        }
    }
}