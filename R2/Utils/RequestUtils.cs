using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

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
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (files == null)
            {
                throw new ArgumentNullException(nameof(files));
            }

            if (!files.Any())
            {
                throw new ArgumentException("File collection is empty.", nameof(files));
            }

            var targetTypeInfo = target.GetType().GetTypeInfo();

            var filesPropertyInfo = targetTypeInfo.GetProperty(nameof(IUpload.Files));

            filesPropertyInfo.SetValue(target, new ReadOnlyCollection<IFile>(files));
        }
    }
}