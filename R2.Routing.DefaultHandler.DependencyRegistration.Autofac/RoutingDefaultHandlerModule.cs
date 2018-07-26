using System.Reflection;
using Autofac;
using R2.DependencyRegistration.Autofac.Extensions;
using Module = Autofac.Module;

namespace R2.Routing.DefaultHandler.DependencyRegistration.Autofac
{
    public class RoutingDefaultHandlerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var targetAssembly = Assembly.GetAssembly(typeof(GetRouteConventionallyFromCommandTypeNameWithoutSuffix));

            builder
                .RegisterAssemblyTypes(targetAssembly)
                .BasedOn(typeof(IRouteHandler))
                .AsSelf()
                .SingleInstance();
        }
    }
}