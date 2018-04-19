using System.Reflection;
using Autofac;
using R2;
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
                .RegisterAssemblyTypes(targetAssembly)
                .AsClosedTypesOf(
                    openGenericServiceType: typeof(ICommandHandler<>),
                    serviceKey: "commandHandler")
                .As<ICommandHandler>()
                .InstancePerLifetimeScope();
        }
    }
}