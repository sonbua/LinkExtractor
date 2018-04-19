using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Caching;
using Autofac;
using Autofac.Builder;
using Autofac.Features.Scanning;
using R2.Aspect.Caching;
using R2.Aspect.Postprocessing;
using R2.Aspect.Preprocessing;
using R2.Aspect.Preprocessing.BuiltIn;
using R2.Aspect.Validation;
using R2.Aspect.Validation.BuiltIn;
using Module = Autofac.Module;

namespace R2.DependencyRegistration.Autofac
{
    public class R2Module : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<RequestProcessor>()
                .As<IRequestProcessor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterInstance(new MemoryCache("ResponseCache"))
                .SingleInstance();

            builder
                .RegisterGeneric(typeof(TrimStringPreprocessor<>))
                .As(typeof(IPreprocessor<>))
                .SingleInstance();

            builder
                .RegisterGeneric(typeof(BuiltInValidator<>))
                .As(typeof(IValidator<>))
                .SingleInstance();

            builder
                .RegisterGeneric(typeof(RequestMustBeNotNullRule<>))
                .SingleInstance();
            builder
                .RegisterGeneric(typeof(DataAnnotationValidationMustPassRule<>))
                .SingleInstance();

            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(RequestValidationDecorator<,>),
                    decoratedServiceType: typeof(IRequestHandler<,>),
                    fromKey: "requestHandler",
                    toKey: "requestValidation")
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(RequestPreprocessingDecorator<,>),
                    decoratedServiceType: typeof(IRequestHandler<,>),
                    fromKey: "requestValidation",
                    toKey: "requestPreprocessing")
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(RequestPostprocessingDecorator<,>),
                    decoratedServiceType: typeof(IRequestHandler<,>),
                    fromKey: "requestPreprocessing",
                    toKey: "requestPostprocessing")
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(RequestCachingDecorator<,>),
                    decoratedServiceType: typeof(IRequestHandler<,>),
                    fromKey: "requestPostprocessing")
                .InstancePerLifetimeScope();

            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(CommandValidationDecorator<>),
                    decoratedServiceType: typeof(ICommandHandler<>),
                    fromKey: "commandHandler",
                    toKey: "commandValidation")
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(CommandPreprocessingDecorator<>),
                    decoratedServiceType: typeof(ICommandHandler<>),
                    fromKey: "commandValidation")
                .InstancePerLifetimeScope();

            builder
                .RegisterType<RequestContext>()
                .As<IRequestContext>()
                .InstancePerLifetimeScope();
        }
    }

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