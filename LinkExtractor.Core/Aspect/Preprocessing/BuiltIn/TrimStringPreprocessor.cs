using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LinkExtractor.Core.Aspect.Preprocessing.BuiltIn
{
    /// <summary>
    /// Trims all string properties on request objects automatically.
    /// To exclude a property from trimming, decorate it with <see cref="IgnoreStringTrimmingAttribute"/>.
    /// To trim a nested object, decorate it with <see cref="TrimStringInNestedObjectAttribute"/>.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class TrimStringPreprocessor<TRequest> : IPreprocessor<TRequest>
    {
        private const BindingFlags _PUBLIC_INSTANCE_PROPERTY_BINDING_FLAG =
            BindingFlags.Public | BindingFlags.Instance;

        public Task ProcessAsync(TRequest request)
        {
            ProcessCore(typeof(TRequest), request);

            return Task.CompletedTask;
        }

        private void ProcessCore(Type requestType, object request)
        {
            var allPropertyInfos = requestType.GetProperties(_PUBLIC_INSTANCE_PROPERTY_BINDING_FLAG);

            TrimStringProperties(allPropertyInfos, request);

            TrimNestedObjects(allPropertyInfos, request);
        }

        private void TrimStringProperties(PropertyInfo[] allPropertyInfos, object request)
        {
            var stringTypePropertyInfos =
                from propertyInfo in allPropertyInfos
                where propertyInfo.PropertyType == typeof(string)
                where propertyInfo.CanWrite
                where !propertyInfo.GetCustomAttributes<IgnoreStringTrimmingAttribute>().Any()
                select propertyInfo;

            foreach (var propertyInfo in stringTypePropertyInfos)
            {
                TryTrim(propertyInfo, request);
            }
        }

        private void TrimNestedObjects(PropertyInfo[] allPropertyInfos, object request)
        {
            var nestedPropertyInfos =
                from propertyInfo in allPropertyInfos
                where propertyInfo.GetCustomAttributes<TrimStringInNestedObjectAttribute>().Any()
                where propertyInfo.GetValue(request) != null
                select propertyInfo;

            foreach (var propertyInfo in nestedPropertyInfos)
            {
                ProcessCore(propertyInfo.PropertyType, propertyInfo.GetValue(request));
            }
        }

        /// <summary>
        /// Tries to trim underlying value, that is ensured to be of string type.
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <param name="request"></param>
        private void TryTrim(PropertyInfo propertyInfo, object request)
        {
            var value = propertyInfo.GetValue(request);

            if (value == null)
            {
                return;
            }

            var stringValue = (string) value;

            if (stringValue == string.Empty)
            {
                return;
            }

            // To avoid performance overhead when setting property's value via reflection, we should better check beforehand whether this is necessary.
            // If performance is the key, we could further optimize by using delegates to perform get/set.
            var trimmedStringValue = stringValue.Trim();

            if (stringValue == trimmedStringValue)
            {
                return;
            }

            propertyInfo.SetValue(request, trimmedStringValue);
        }
    }
}