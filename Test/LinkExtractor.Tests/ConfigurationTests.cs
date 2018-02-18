using System;
using System.Diagnostics;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Handlers;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Castle.Windsor.Diagnostics;
using LinkExtractor.Core;
using LinkExtractor.Core.Windsor;
using LinkExtractor.Instagram.Windsor;
using Xunit;

namespace LinkExtractor.Tests
{
    public class ConfigurationTests
    {
        public ConfigurationTests()
        {
            _container = new WindsorContainer();

            _container.Install(
                new CoreInstaller(),
                new InstagramInstaller()
            );

            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
        }

        private readonly IWindsorContainer _container;

        [Fact]
        public void EnsuresNoPotentiallyMisconfiguredComponents()
        {
            // Inspect the container for problems
            var key = SubSystemConstants.DiagnosticsKey;
            var host = (IDiagnosticsHost) _container.Kernel.GetSubSystem(key);
            var diagnostic = host.GetDiagnostic<IPotentiallyMisconfiguredComponentsDiagnostic>();
            var handlers = diagnostic.Inspect();

            // Iterate over the problems, writing messages to the console
            foreach (var handler in handlers)
            {
                var problem = (IExposeDependencyInfo) handler;

                var message = new StringBuilder();
                var inspector = new DependencyInspector(message);

                problem.ObtainDependencyDetails(inspector);

                Trace.WriteLine(message.ToString());
            }

            // Fail the test if there are any problems
            Assert.Empty(handlers);
        }

        [Fact]
        public void EnsuresAllRequestHandlersCanBeResolvedSuccessfully()
        {
            // arrange

            // act
            using (_container.BeginScope())
            {
                _container.ResolveAll<IRequestHandler>();
            }

            // assert
        }
    }
}