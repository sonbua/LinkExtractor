﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cqrs.Aspect.Validation
{
    public class RequestValidationDecorator<TRequest, TResponse> : BaseRequestHandler<TRequest, TResponse>
        where TResponse : IResponse<TRequest>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IRequestHandler<TRequest, TResponse> _inner;

        public RequestValidationDecorator(
            IEnumerable<IValidator<TRequest>> validators,
            IRequestHandler<TRequest, TResponse> inner)
        {
            _validators = validators;
            _inner = inner;
        }

        public override async Task<TResponse> HandleAsync(TRequest request)
        {
            foreach (var validator in _validators)
            {
                await validator.ValidateAsync(request);
            }

            return await _inner.HandleAsync(request);
        }
    }
}