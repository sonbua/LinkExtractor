using System;
using System.Linq;
using System.Threading.Tasks;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using LinkExtractor.Core;
using LinkExtractor.Core.Windsor;
using LinkExtractor.Instagram.Windsor;
using Xunit;

namespace LinkExtractor.Instagram.Tests
{
    public class InstagramRequestHandlerTest : IDisposable
    {
        private readonly IWindsorContainer _container;
        private readonly IDisposable _scope;

        public InstagramRequestHandlerTest()
        {
            _container = new WindsorContainer();

            _container.Install(
                new CoreInstaller(),
                new InstagramInstaller()
            );

            _scope = _container.BeginScope();
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
            var processor = _container.Resolve<IRequestProcessor>();

            // act
            var response = await processor.ProcessAsync<InstagramRequest, InstagramResponse>(request);

            // assert
            Assert.Single(response.Media);
            Assert.NotEmpty(response.Media[0].DisplayResources);
        }

        [Theory]
        [InlineData("https://www.instagram.com/p/BfVLhdLBnlm/")]
        public async Task GivenAValidInstagramUrlWithMultipleImages_ReturnsAllMediaWithCorrectLinks(
            string url)
        {
            // arrange
            var request = new InstagramRequest {Url = url};
            var processor = _container.Resolve<IRequestProcessor>();

            // act
            var response = await processor.ProcessAsync<InstagramRequest, InstagramResponse>(request);

            // assert
            Assert.NotEqual(expected: 1, actual: response.Media.Length);
            Assert.True(response.Media.All(x => x.DisplayResources.Any()));
        }
    }
}