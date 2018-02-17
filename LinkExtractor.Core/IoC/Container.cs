using Castle.Windsor;

namespace LinkExtractor.Core.IoC
{
    public static class Container
    {
        /// <summary>
        /// Instance per application.
        /// Only use this when you cannot inject through constructor, e.g. in attribute.
        /// </summary>
        public static IWindsorContainer Instance { get; private set; }

        public static void SetContainer(IWindsorContainer container)
        {
            Instance = container;
        }

        // TODO: in need?
        /// <summary>
        /// Creates an instance of <see cref="CustomScope"/> to track transient instances that were resolved manually.
        /// When the scope gets disposed, all tracked transient instances will get disposed too.
        /// Usage:
        /// <code>using (var scope = Container.BeginCustomScope())
        /// {
        ///     var validator =
        ///         Container.Instance
        ///             .Resolve&lt;IValidator&lt;FooRequest&gt;&gt;()
        ///             .TrackedBy(scope);
        ///     // do something with validator
        /// }</code>
        /// </summary>
        /// <returns></returns>
        public static CustomScope BeginCustomScope()
        {
            return new CustomScope();
        }

        // TODO: in need?
        public static TComponent TrackedBy<TComponent>(this TComponent component, CustomScope scope)
        {
            scope.Track(component);

            return component;
        }
    }
}