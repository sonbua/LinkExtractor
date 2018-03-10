using System.Collections.Generic;

namespace R2.Aspect.Validation
{
    public class QueryValidationDecorator<TQuery, TResult>
        : RequestValidationDecorator<TQuery, TResult>,
            IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        public QueryValidationDecorator(
            IEnumerable<IValidator<TQuery>> validators,
            IQueryHandler<TQuery, TResult> inner)
            : base(validators, inner)
        {
        }
    }
}