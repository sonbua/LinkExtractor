using Autofac;

namespace R2.Routing.DependencyRegistration.Autofac
{
    public class RoutingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Note: IRequestProcessor is registered as InstancePerLifetimeScope
            builder
                .RegisterType<RouteProcessor>()
                .As<IRouteProcessor>()
                .InstancePerLifetimeScope();

            builder
                .RegisterType<CommandRouteTable>()
                .SingleInstance();
            builder
                .RegisterType<QueryRouteTable>()
                .SingleInstance();
        }
    }
}