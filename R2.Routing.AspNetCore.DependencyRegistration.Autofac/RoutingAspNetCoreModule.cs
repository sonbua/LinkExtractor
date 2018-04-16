using Autofac;

namespace R2.Routing.AspNetCore.DependencyRegistration.Autofac
{
    public class RoutingAspNetCoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<GetRouteFromRouteAttribute>()
                .AsSelf()
                .SingleInstance();
        }
    }
}