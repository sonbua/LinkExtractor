using System;
using System.Threading.Tasks;

namespace ResponsibilityChain
{
    /// <summary>
    /// <para>Serves as a marker for all handlers (processing elements) in the chain, e.g. for dependency registrations, etc.</para>
    /// <para>For most cases, the generic version <see cref="IHandler{TRequest,TResponse}"/> should be the one to be implemented, and not this one.</para>
    /// </summary>
    public interface IHandler
    {
    }

    /// <summary>
    /// <para>Represents a handler (processing element) in the chain. This could be used in several fashion</para>
    /// <para>1. Synchronous request/response model.</para>
    /// <para>2. Asynchronous request/response model by setting the response type <typeparamref name="TResponse"/> to be <see cref="Task"/> or <see cref="Task{TResult}"/>.</para>
    /// <para>3. Asynchronous, OWIN-like model by setting the request type <typeparamref name="TRequest"/> to be a context (including request and response objects), and the response type <typeparamref name="TResponse"/> to be a <see cref="Task"/>.</para>
    /// </summary>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResponse">The type of the response.</typeparam>
    public interface IHandler<TRequest, TResponse> : IHandler
    {
        /// <summary>
        /// Either processes the request then returns result to its caller or passes on the request to the next handler in the chain for further processing.
        /// </summary>
        /// <param name="request">The request object.</param>
        /// <param name="next">The next handler in the chain.</param>
        /// <returns></returns>
        TResponse Handle(TRequest request, Func<TRequest, TResponse> next);
    }
}