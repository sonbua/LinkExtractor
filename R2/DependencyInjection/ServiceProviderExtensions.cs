using System;
using EnsureThat;

namespace R2.DependencyInjection
{
    public static class ServiceProviderExtensions
    {
        public static T GetService<T>(this IServiceProvider serviceProvider)
        {
            EnsureArg.IsNotNull(serviceProvider, nameof(serviceProvider));

            return (T) serviceProvider.GetService(typeof(T));
        }
    }
}