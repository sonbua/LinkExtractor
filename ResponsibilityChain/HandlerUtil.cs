using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ResponsibilityChain
{
    internal static class HandlerUtil
    {
        internal static Func<TRequest, TResponse> CreatePipelineExecutionDelegate<TRequest, TResponse>(
            this IList<IHandler<TRequest, TResponse>> handlers,
            Func<TRequest, TResponse> next)
        {
            if (next == null)
            {
                next = request => ThrowNotSupportedHandler<TRequest, TResponse>.Instance.Handle(request, null);
            }

            return (Func<TRequest, TResponse>) handlers.CreatePipelineExecutionExpression(next).Compile();
        }

        /// <code>
        /// rootRequest
        ///    => handler0.Invoke(rootRequest,
        ///       request0 => handler1.Invoke(request0,
        ///        ...
        ///          request{N-1} => handler{N}.Invoke(request{N-1},
        ///             request{N} => next(request{N})))
        /// </code>
        private static LambdaExpression CreatePipelineExecutionExpression<TRequest, TResponse>(
            this IList<IHandler<TRequest, TResponse>> handlers,
            Func<TRequest, TResponse> next)
        {
            var lambdaExpression = InitialLambdaExpression(next, handlers.Count);

            // request{i} => handler.Invoke(request{i}, previous)
            for (var index = handlers.Count - 1; index >= 0; index--)
            {
                var handler = handlers[index];
                var handleMethodInfo = handler.ImplementedHandleMethod();
                var requestParameter = Expression.Parameter(typeof(TRequest), $"request{index}");

                var body = Expression.Call(
                    Expression.Constant(handler),
                    handleMethodInfo,
                    requestParameter,
                    lambdaExpression
                );

                lambdaExpression = Expression.Lambda(body, requestParameter);
            }

            return lambdaExpression;
        }

        /// <summary>
        /// Selects the method on the type which was implemented from the <see cref="IHandler{TRequest,TResponse}"/> interface;
        /// </summary>
        /// <param name="handler"></param>
        /// <typeparam name="TRequest"></typeparam>
        /// <typeparam name="TResponse"></typeparam>
        /// <returns></returns>
        private static MethodInfo ImplementedHandleMethod<TRequest, TResponse>(
            this IHandler<TRequest, TResponse> handler)
        {
            var handlerTypeInfo = handler.GetType().GetTypeInfo();
            var handlerInterfaceType = typeof(IHandler<TRequest, TResponse>);

            return handlerTypeInfo.GetInterfaceMap(handlerInterfaceType).TargetMethods.Single();
        }

        /// <code>
        /// request{N} => next(request{N})
        /// </code>
        private static LambdaExpression InitialLambdaExpression<TRequest, TResponse>(
            Func<TRequest, TResponse> next,
            int handlersCount)
        {
            var requestNParameter = Expression.Parameter(typeof(TRequest), $"request{handlersCount}");
            var invokeExpression = Expression.Invoke(Expression.Constant(next), requestNParameter);

            return Expression.Lambda(invokeExpression, requestNParameter);
        }
    }
}