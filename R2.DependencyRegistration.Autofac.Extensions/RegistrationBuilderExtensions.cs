using System;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using EnsureThat;

namespace R2.DependencyRegistration.Autofac.Extensions
{
    /// <summary>
    /// Note: Autofac.Util might be the more better place for this extension methods.
    /// </summary>
    public static class RegistrationBuilderExtensions
    {
        /// <summary>
        /// Why bother creating this method while Autofac.RegistrationExtensions.AsClosedTypesOf extension method has been provided?
        /// AsClosedTypesOf method deals with open generic service types only.
        /// This <see cref="BasedOn{TLimit,TScanningActivatorData,TRegistrationStyle}"/> method deals with both open and closed types.
        /// </summary>
        /// <param name="registration"></param>
        /// <param name="baseType"></param>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TScanningActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle>
            BasedOn<TLimit, TScanningActivatorData, TRegistrationStyle>(
                this IRegistrationBuilder<TLimit, TScanningActivatorData, TRegistrationStyle> registration,
                Type baseType)
            where TScanningActivatorData : ScanningActivatorData
        {
            EnsureArg.IsNotNull(registration, nameof(registration));
            EnsureArg.IsNotNull(baseType, nameof(baseType));

            var filter = TypesFilter(baseType);

            registration.ActivatorData.Filters.Add(filter);

            return registration;
        }

        private static Func<Type, bool> TypesFilter(Type baseType) =>
            baseType.GetTypeInfo().IsGenericTypeDefinition
                ? IsClosedTypeOf(baseType)
                : IsAssignableTo(baseType);

        private static Func<Type, bool> IsClosedTypeOf(Type baseType) =>
            type => type.IsClosedTypeOf(baseType);

        private static Func<Type, bool> IsAssignableTo(Type baseType) =>
            type => baseType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
    }
}