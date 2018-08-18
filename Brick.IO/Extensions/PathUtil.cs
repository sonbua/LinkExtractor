using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brick.IO
{
    public static class PathUtil
    {
        public static string CombineWith(this string path, params string[] thesePaths)
        {
            if (path == null)
            {
                path = string.Empty;
            }

            if (thesePaths.Length == 1 && thesePaths[0] == null)
            {
                return path;
            }

            var sanitizedPath = path.Length <= 1 ? path : path.TrimEnd('/', '\\');
            var builder = new StringBuilder(sanitizedPath);

            AppendPaths(builder, thesePaths);

            return builder.ToString();
        }

        private static void AppendPaths(StringBuilder builder, IEnumerable<string> paths)
        {
            foreach (var path in paths.Sanitize())
            {
                if (builder.Length > 0 && builder[builder.Length - 1] != '/')
                {
                    builder.Append("/");
                }

                builder.Append(path);
            }
        }

        private static IEnumerable<string> Sanitize(this IEnumerable<string> paths) =>
            from path in paths
            where !string.IsNullOrEmpty(path)
            select path.Replace('\\', '/').TrimStart('/');
    }
}