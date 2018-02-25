using System.Reflection;
using Autofac;
using LinkExtractor.Core;
using LinkExtractor.Core.Aspect.Validation;
using LinkExtractor.Core.DependencyRegistration;
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
                .AsClosedTypesOf(typeof(IRequestHandler<,>), "requestHandler")
                .As<IRequestHandler>()
                .InstancePerLifetimeScope();
            builder
                .RegisterGenericDecorator(
                    typeof(RequestValidationDecorator<,>),
                    typeof(IRequestHandler<,>),
                    "requestHandler")
                .InstancePerLifetimeScope();
        }
    }
}