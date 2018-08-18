using System;
using System.Text;
using Xunit;

namespace ResponsibilityChain.Tests
{
    public class ReturnDefaultHandlerTest
    {
        [Fact]
        public void IntegerOutputExpected_ReturnsZero()
        {
            // arrange
            var handler = ReturnDefaultHandler<string, int>.Instance;

            // act
            var response = handler.Handle("some input", next: null);

            // assert
            Assert.Equal(0, response);
        }

        [Fact]
        public void StringOutputExpected_ReturnsNull()
        {
            // arrange
            var handler = ReturnDefaultHandler<string, string>.Instance;

            // act
            var response = handler.Handle("some input", next: null);

            // assert
            Assert.Null(response);
        }

        [Fact]
        public void DateTimeOutputExpected_ReturnsDateTimeMin()
        {
            // arrange
            var handler = ReturnDefaultHandler<string, DateTime>.Instance;

            // act
            var response = handler.Handle("some input", next: null);

            // assert
            Assert.Equal(DateTime.MinValue, response);
        }

        [Fact]
        public void ReferenceTypeObjectOutputExpected_ReturnsNull()
        {
            // arrange
            var handler = ReturnDefaultHandler<string, StringBuilder>.Instance;

            // act
            var response = handler.Handle("some input", next: null);

            // assert
            Assert.Null(response);
        }
    }
}