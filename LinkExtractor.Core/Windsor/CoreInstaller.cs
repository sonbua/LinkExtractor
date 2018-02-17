using System.Reflection;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LinkExtractor.Core.Aspect.Validation;
using LinkExtractor.Core.Aspect.Validation.BuiltIn;

namespace LinkExtractor.Core.Windsor
{
    public class CoreInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // processor
            container.Register(
                Component
                    .For<IRequestProcessor>()
                    .ImplementedBy<RequestValidationDecorator>()
                    .LifestyleSingleton(),
                Component
                    .For<IRequestProcessor>()
                    .ImplementedBy<RequestProcessor>()
                    .LifestyleSingleton()
            );
            
            // validators
            container.Register(
                Component
                    .For(typeof(IValidator<>))
                    .ImplementedBy(typeof(BuiltInValidator<>))
                    .LifestyleTransient()
            );

            // validation rules
            container.Register(
                Classes
                    .FromAssembly(Assembly.GetCallingAssembly())
                    .BasedOn(typeof(IValidationRule<>))
                    .WithServiceSelf()
                    .LifestyleTransient()
            );
        }
    }
}