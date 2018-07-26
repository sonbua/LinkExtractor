﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac.Builder;
using Autofac.Features.Scanning;

namespace R2.DependencyRegistration.Autofac.Extensions
{
    public static class RegistrationBuilderExtensions
    {
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

            if (!baseType.IsGenericTypeDefinition)
            {
                registration.ActivatorData.Filters.Add(type => baseType.IsAssignableFrom(type));

                return registration;
            }

            registration
                .ActivatorData
                .Filters
                .Add(IsClosedTypeOf(baseType));

            return registration;
        }

        private static Func<Type, bool> IsClosedTypeOf(Type openGenericType)
        {
            return
                type =>
                {
                    if (type.IsGenericTypeDefinition)
                    {
                        return false;
                    }

                    return TypeExtensions.TypesAssignableFrom(type)
                        .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == openGenericType);
                };
        }
    }

    internal static class TypeExtensions
    {
        public static IEnumerable<Type> TypesAssignableFrom(Type type)
        {
            return
                type.GetTypeInfo().ImplementedInterfaces
                    .Concat(Traverse.Across(type, t => t.GetTypeInfo().BaseType));
        }
    }

    internal static class Traverse
    {
        public static IEnumerable<T> Across<T>(T first, Func<T, T> next)
            where T : class
        {
            for (var item = first; (object) item != null; item = next(item))
            {
                yield return item;
            }
        }
    }
}