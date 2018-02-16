using System;
using System.Collections.Generic;
using Castle.Core.Logging;

namespace LinkExtractor.Core.IoC
{
    public class CustomScope : IDisposable
    {
        private const string _RELEASE_COMPONENT_FAILED = "Could not release component: {0}";

        private readonly IList<object> _components;

        public CustomScope()
        {
            _components = new List<object>();
        }

        public void Dispose()
        {
            var container = Container.Instance;

            foreach (var component in _components)
            {
                try
                {
                    container.Release(component);
                }
                catch (Exception exception)
                {
                    // LIFESTYLE: Castle.Core.Logging.ILogger is registered with singleton lifestyle by default, so there is no need to release it.
                    var logger = container.Resolve<ILogger>();

                    logger.Error(string.Format(_RELEASE_COMPONENT_FAILED, component), exception);
                }
            }
        }

        internal void Track<TComponent>(TComponent component)
        {
            _components.Add(component);
        }
    }
}