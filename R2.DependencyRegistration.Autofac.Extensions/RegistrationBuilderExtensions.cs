using System;
using System.Reflection;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace R2.DependencyRegistration.Autofac.Extensions
{
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
            if (registration == null)
            {
                throw new ArgumentNullException(nameof(registration));
            }

            if (baseType == null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            if (baseType.GetTypeInfo().IsGenericTypeDefinition)
            {
                registration.ActivatorData.Filters.Add(type => type.IsClosedTypeOf(baseType));

                return registration;
            }

            registration.ActivatorData.Filters.Add(IsAssignableTo(baseType));

            return registration;
        }

        private static Func<Type, bool> IsAssignableTo(Type baseType)
        {
            return type => baseType.GetTypeInfo().IsAssignableFrom(type.GetTypeInfo());
        }
    }
}