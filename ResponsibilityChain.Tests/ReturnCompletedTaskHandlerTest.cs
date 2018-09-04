﻿using System;
using System.Threading.Tasks;
using Xunit;

namespace ResponsibilityChain.Tests
{
    public class ReturnCompletedTaskHandlerTest
    {
        private class CompositeFooAsyncHandler : Handler<int, Task>
        {
            public CompositeFooAsyncHandler()
            {
                AddHandler(new BarHandler());
                AddHandler(ReturnCompletedTaskHandler<int>.Instance);
                AddHandler(ThrowNotSupportedHandler<int, Task>.Instance);
            }

            private class BarHandler : IHandler<int, Task>
            {
                public async Task Handle(int request, Func<int, Task> next)
                {
                    await Task.Delay(100);

                    await next(request);
                }
            }
        }

        [Fact]
        public async Task
            GivenThisShortCircuitHandlerPlacedInTheMiddleOfTheChain_ReturnsCompletedTaskWithoutThrowingException()
        {
            // arrange
            var handler = new CompositeFooAsyncHandler();

            // act
            await handler.Handle(111, null);

            // assert
        }
    }
}