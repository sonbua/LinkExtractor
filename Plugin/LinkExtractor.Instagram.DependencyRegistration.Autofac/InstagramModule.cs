using System.Reflection;
using Autofac;
using R2;
using R2.Aspect.Caching;
using R2.Aspect.Postprocessing;
using R2.Aspect.Preprocessing;
using R2.Aspect.Validation;
using R2.DependencyRegistration.Autofac;
using Module = Autofac.Module;

namespace LinkExtractor.Instagram.DependencyRegistration.Autofac
{
    public class InstagramModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var targetAssembly = Assembly.GetAssembly(typeof(InstagramRequest));

            builder
                .RegisterAssemblyTypes(targetAssembly)
                .AsClosedTypesOf(typeof(IPreprocessor<>))
                .As<IPreprocessor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(targetAssembly)
                .AsClosedTypesOf(typeof(IValidator<>))
                .As<IValidator>()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(targetAssembly)
                .BasedOn(typeof(IValidationRule<>))
                .AsSelf()
                .As<IValidationRule>()
                .InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(targetAssembly)
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

            builder
                .RegisterAssemblyTypes(targetAssembly)
                .AsClosedTypesOf(
                    openGenericServiceType: typeof(ICommandHandler<>),
                    serviceKey: "commandHandler")
                .As<ICommandHandler>()
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
        }
    }
}