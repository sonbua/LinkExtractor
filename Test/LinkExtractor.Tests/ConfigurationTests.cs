using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cqrs;
using Cqrs.Aspect.Preprocessing;
using Cqrs.Aspect.Validation;
using Cqrs.DependencyRegistration.Autofac;
using LinkExtractor.Instagram.DependencyRegistration;
using Xunit;

namespace LinkExtractor.Tests
{
    public class ConfigurationTests : IDisposable
    {
        public ConfigurationTests()
        {
            var builder = new ContainerBuilder();

            builder
                .RegisterType<AutofacServiceProvider>()
                .As<IServiceProvider>()
                .InstancePerLifetimeScope();

            builder.RegisterModule<CoreModule>()
                .RegisterModule<InstagramModule>();

            _container = builder.Build();

            _scope = _container.BeginLifetimeScope();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            _container?.Dispose();
        }

        private readonly IContainer _container;
        private readonly ILifetimeScope _scope;

        [Fact]
        public void EnsuresAllPreprocessorsCanBeResolvedSuccessfully()
        {
            // arrange

            // act
            _scope.Resolve<IEnumerable<IPreprocessor>>();

            // assert
        }

        [Fact]
        public void EnsuresAllValidatorsCanBeResolvedSuccessfully()
        {
            // arrange

            // act
            _scope.Resolve<IEnumerable<IValidator>>();

            // assert
        }

        [Fact]
        public void EnsuresAllValidationRulesCanBeResolvedSuccessfully()
        {
            // arrange

            // act
            _scope.Resolve(typeof(IEnumerable<IValidationRule>));

            // assert
        }

        [Fact]
        public void EnsuresAllRequestHandlersCanBeResolvedSuccessfully()
        {
            // arrange

            // act
            _scope.Resolve<IEnumerable<IRequestHandler>>();

            // assert
        }
    }
}