using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brick.IO
{
    public static class VirtualPathUtils
    {
        public static TimeSpan MaxRetryOnTimeoutException { get; } = TimeSpan.FromSeconds(10);

        public static Stack<string> TokenizeVirtualPath(this string virtualPath, IVirtualPathProvider pathProvider)
        {
            if (pathProvider == null)
            {
                throw new ArgumentNullException(nameof(pathProvider));
            }

            return TokenizeVirtualPath(virtualPath, pathProvider.VirtualPathSeparator);
        }

        public static Stack<string> TokenizeVirtualPath(this string virtualPath, string virtualPathSeparator)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                return new Stack<string>();
            }

            var tokens = virtualPath.Split(new[] {virtualPathSeparator}, StringSplitOptions.RemoveEmptyEntries);

            return new Stack<string>(tokens.Reverse());
        }

        public static void SleepBackOffMultiplier(this int i)
        {
            var nextTryInMillisecond = (2 ^ i) * 50;

            Task.Delay(nextTryInMillisecond).Wait();
        }
    }
}