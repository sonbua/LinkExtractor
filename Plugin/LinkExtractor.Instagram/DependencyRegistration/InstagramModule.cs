using System.Reflection;
using Autofac;
using Cqrs;
using Cqrs.Aspect.Caching;
using Cqrs.Aspect.Postprocessing;
using Cqrs.Aspect.Preprocessing;
using Cqrs.Aspect.Validation;
using Cqrs.DependencyRegistration;
using Module = Autofac.Module;

namespace LinkExtractor.Instagram.DependencyRegistration
{
    public class InstagramModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            builder
                .RegisterAssemblyTypes(thisAssembly)
                .AsClosedTypesOf(typeof(IPreprocessor<>))
                .As<IPreprocessor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(thisAssembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .As<IValidator>()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(thisAssembly)
                .BasedOn(typeof(IValidationRule<>))
                .AsSelf()
                .As<IValidationRule>()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(thisAssembly)
                .AsClosedTypesOf(
                    openGenericServiceType: typeof(IRequestHandler<,>),
                    serviceKey: "requestHandler")
                .As<IRequestHandler>()
                .InstancePerLifetimeScope();
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
        }
    }
}