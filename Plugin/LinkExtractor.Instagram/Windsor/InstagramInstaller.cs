using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LinkExtractor.Core;
using LinkExtractor.Core.Aspect.Validation;

namespace LinkExtractor.Instagram.Windsor
{
    public class InstagramInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // validators
            container.Register(
                Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .BasedOn(typeof(IValidator<>))
                    .WithServiceAllInterfaces()
                    .LifestyleScoped()
            );

            // request handlers
            container.Register(
                Classes
                    .FromAssembly(Assembly.GetExecutingAssembly())
                    .BasedOn(typeof(IRequestHandler<,>))
                    .WithServiceAllInterfaces()
                    .LifestyleScoped()
            );
        }
    }
}