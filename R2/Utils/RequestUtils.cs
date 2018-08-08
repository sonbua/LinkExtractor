using System;
using System.Collections.Generic;
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
        /// <param name="files">The files to assign to Files property on the <paramref name="target"/> object.</param>
        public static void AttachFileToObject(object target, IList<IFile> files)
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

            var targetType = target.GetType();

            var targetPropertyInfo =
                targetType.GetProperty(nameof(IUpload<object>.Files), BindingFlags.Public | BindingFlags.Instance);

            if (targetPropertyInfo == null || typeof(IReadOnlyList<IFile>) != targetPropertyInfo.PropertyType)
            {
                throw new NotSupportedException(
                    $"A property named \"{nameof(IUpload<object>.Files)}\" of type {typeof(IReadOnlyList<IFile>)} on the {nameof(target)} object must be present."
                );
            }

            targetPropertyInfo.SetValue(target, files);
        }
    }
}