using System;
using System.Reflection;

namespace R2
{
    public static class RequestUtils
    {
        /// <summary>
        /// Attachs file to an object. A property named "File" of type <see cref="IFile"/> on the <paramref name="target"/> object must be present.
        /// </summary>
        /// <param name="target">The object to be assigned a file to.</param>
        /// <param name="file">The file to assign to File property on the <paramref name="target"/> object.</param>
        public static void AttachFileToObject(object target, IFile file)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var targetType = target.GetType();

            var targetPropertyInfo =
                targetType.GetProperty(
                    "File",
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty
                );

            if (targetPropertyInfo == null || !typeof(IFile).IsAssignableFrom(targetPropertyInfo.PropertyType))
            {
                throw new NotSupportedException(
                    $"A property named \"File\" of type {typeof(IFile)} on the {nameof(target)} object must be present."
                );
            }

            targetPropertyInfo.SetValue(target, file);
        }
    }
}