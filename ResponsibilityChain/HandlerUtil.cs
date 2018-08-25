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
            var nextAsExpression = NextAsExpression(next);

            return handlers.CreatePipelineExecutionExpression(nextAsExpression).Compile();
        }

        /// Returns
        /// <code>
        /// requestN => next(requestN)
        /// </code>
        private static Expression<Func<TRequest, TResponse>> NextAsExpression<TRequest, TResponse>(
            Func<TRequest, TResponse> next)
        {
            if (next == null)
            {
                return requestN => ThrowNotSupportedHandler<TRequest, TResponse>.Instance.Handle(requestN, null);
            }

            return requestN => next(requestN);
        }

        /// Returns
        /// <code>
        /// rootRequest
        ///    => handler0.Invoke(rootRequest,
        ///       request0 => handler1.Invoke(request0,
        ///          ...
        ///          request{N-1} => handler{N}.Invoke(request{N-1}, next)))
        /// </code>
        private static Expression<Func<TRequest, TResponse>> CreatePipelineExecutionExpression<TRequest, TResponse>(
            this IList<IHandler<TRequest, TResponse>> handlers,
            Expression<Func<TRequest, TResponse>> next)
        {
            var lambdaExpression = next;

            // request{i} => handler.Invoke(request{i}, previous)
            for (var i = handlers.Count - 1; i >= 0; i--)
            {
                var handler = handlers[i];
                var handleMethodInfo = handler.ImplementedHandleMethod();
                var requestParameter = Expression.Parameter(typeof(TRequest), $"request{i}");

                var body = Expression.Call(
                    Expression.Constant(handler),
                    handleMethodInfo,
                    requestParameter,
                    lambdaExpression
                );

                lambdaExpression = Expression.Lambda<Func<TRequest, TResponse>>(body, requestParameter);
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
    }
}