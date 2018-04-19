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
            LoadRequestProcessor(builder);

            LoadCache(builder);

            LoadBuiltInPreprocessor(builder);

            LoadBuiltInValidatorAndValidationRules(builder);

            LoadQueryHandlerDecorators(builder);

            LoadCommandHandlerDecorators(builder);

            LoadRequestContext(builder);
        }

        private static void LoadRequestProcessor(ContainerBuilder builder)
        {
            builder
                .RegisterType<RequestProcessor>()
                .As<IRequestProcessor>()
                .InstancePerLifetimeScope();
        }

        private static void LoadCache(ContainerBuilder builder)
        {
            builder
                .RegisterInstance(new MemoryCache("ResponseCache"))
                .SingleInstance();
        }

        private static void LoadBuiltInPreprocessor(ContainerBuilder builder)
        {
            builder
                .RegisterGeneric(typeof(TrimStringPreprocessor<>))
                .As(typeof(IPreprocessor<>))
                .SingleInstance();
        }

        private static void LoadBuiltInValidatorAndValidationRules(ContainerBuilder builder)
        {
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
        }

        private static void LoadQueryHandlerDecorators(ContainerBuilder builder)
        {
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(QueryValidationDecorator<,>),
                    decoratedServiceType: typeof(IQueryHandler<,>),
                    fromKey: "queryHandler",
                    toKey: "queryValidation")
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(QueryPreprocessingDecorator<,>),
                    decoratedServiceType: typeof(IQueryHandler<,>),
                    fromKey: "queryValidation",
                    toKey: "queryPreprocessing")
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(QueryPostprocessingDecorator<,>),
                    decoratedServiceType: typeof(IQueryHandler<,>),
                    fromKey: "queryPreprocessing",
                    toKey: "queryPostprocessing")
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    decoratorType: typeof(QueryCachingDecorator<,>),
                    decoratedServiceType: typeof(IQueryHandler<,>),
                    fromKey: "queryPostprocessing")
                .InstancePerLifetimeScope();
        }

        private static void LoadCommandHandlerDecorators(ContainerBuilder builder)
        {
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
        }

        private static void LoadRequestContext(ContainerBuilder builder)
        {
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