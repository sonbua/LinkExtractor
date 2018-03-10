using System.Collections.Generic;

namespace R2.Aspect.Preprocessing
{
    public class QueryPreprocessingDecorator<TQuery, TResult>
        : RequestPreprocessingDecorator<TQuery, TResult>,
            IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public QueryPreprocessingDecorator(
            IEnumerable<IPreprocessor<TQuery>> preprocessors,
            IQueryHandler<TQuery, TResult> inner)
            : base(preprocessors, inner)
        {
        }
    }
}