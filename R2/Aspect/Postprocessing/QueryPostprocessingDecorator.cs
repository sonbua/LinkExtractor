using System.Collections.Generic;

namespace R2.Aspect.Postprocessing
{
    public class QueryPostprocessingDecorator<TQuery, TResult>
        : RequestPostprocessingDecorator<TQuery, TResult>,
            IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public QueryPostprocessingDecorator(
            IQueryHandler<TQuery, TResult> inner,
            IEnumerable<IPostprocessor<TQuery, TResult>> postprocessors)
            : base(inner, postprocessors)
        {
        }
    }
}