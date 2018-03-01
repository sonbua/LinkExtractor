using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using LinkExtractor.Instagram.DependencyRegistration;
using R2;
using R2.DependencyRegistration.Autofac;
using Xunit;

namespace LinkExtractor.Instagram.Tests
{
    public class InstagramRequestHandlerTest : IDisposable
    {
        private readonly IContainer _container;
        private readonly ILifetimeScope _scope;

        public InstagramRequestHandlerTest()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>();

            builder.RegisterModule<R2Module>()
                .RegisterModule<InstagramModule>();

            _container = builder.Build();

            _scope = _container.BeginLifetimeScope();
        }

        public void Dispose()
        {
            _scope?.Dispose();
            _container?.Dispose();
        }

        [Theory]
        [InlineData("https://www.instagram.com/p/BfUzAEvnqFk/")]
        [InlineData("http://www.instagram.com/p/BfUzAEvnqFk/")]
        [InlineData("https://instagram.com/p/BfUzAEvnqFk/")]
        [InlineData("http://instagram.com/p/BfUzAEvnqFk/")]
        public async Task GivenAValidInstagramUrlWithSingleImage_ReturnsSingleImageWithCorrectLinks(string url)
        {
            // arrange
            var request = new InstagramRequest {Url = url};
            var processor = _scope.Resolve<IRequestProcessor>();

            // act
            var response = await processor.ProcessAsync<InstagramRequest, InstagramResponse>(request);

            // assert
            Assert.Single(response.Media);
            Assert.NotEmpty(response.Media[0].DisplayResources);
        }

        [Theory]
        [InlineData("https://www.instagram.com/p/BfVLhdLBnlm/")]
        public async Task GivenAValidInstagramUrlWithMultipleImages_ReturnsAllMediaWithCorrectLinks(string url)
        {
            // arrange
            var request = new InstagramRequest {Url = url};
            var processor = _scope.Resolve<IRequestProcessor>();

            // act
            var response = await processor.ProcessAsync<InstagramRequest, InstagramResponse>(request);

            // assert
            Assert.NotEqual(expected: 1, actual: response.Media.Length);
            Assert.True(response.Media.All(x => x.DisplayResources.Any()));
        }

        [Theory]
        [InlineData("https://www.instagram.com/p/BfUtyjyhgCK/")]
        public async Task GivenAValidInstagramUrlWithVideo_ReturnsVideoLink(string url)
        {
            // arrange
            var request = new InstagramRequest {Url = url};
            var processor = _scope.Resolve<IRequestProcessor>();

            // act
            var response = await processor.ProcessAsync<InstagramRequest, InstagramResponse>(request);

            // assert
            Assert.Single(response.Media);
            Assert.True(response.Media[0].IsVideo);
            Assert.NotEmpty(response.Media[0].VideoUrl);
            Assert.NotEmpty(response.Media[0].DisplayResources);
        }

        [Theory]
        [InlineData("https://www.instagram.com/p/BfUtyjyhgCK/")]
        public async Task GivenAFakeCommand_ReturnsNothing(string url)
        {
            // arrange
            object command = new FakeCommand();
            var processor = _scope.Resolve<IRequestProcessor>();

            // act
            //await processor.ProcessCommandAsync(command);
            await processor.ProcessCommandAsync(command);

            // assert
        }
    }
}