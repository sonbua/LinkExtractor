using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace ResponsibilityChain.Tests
{
    public class CompositeHandlerTest
    {
        [Theory]
        [InlineData("20m", 20)]
        [InlineData("1h", 60)]
        [InlineData("2h 10m", 130)]
        public void GivenTextWithMinuteUnit_ReturnsCorrectNumberOfMinutes(string worklog, int expected)
        {
            // arrange
            IWorklogParser parser = new WorklogParser();

            // act
            var actual = parser.Handle(worklog, null);

            // assert
            Assert.Equal(expected, actual);
        }

        internal class WorklogParser : CompositeHandler<string, int>, IWorklogParser
        {
            public WorklogParser()
            {
                AddHandler(new TechnicalLeaderParser());
            }
        }

        internal class TechnicalLeaderParser : CompositeHandler<string, int>, IWorklogParser
        {
            public TechnicalLeaderParser()
            {
                AddHandler(new HourParser());
                AddHandler(new MinuteParser());
            }

            public override int Handle(string request, Func<string, int> next)
            {
                return request.Split(' ').Select(piece => base.Handle(piece, null)).Sum();
            }
        }

        internal class HourParser : IWorklogParser
        {
            private readonly Regex _pattern = new Regex("^(\\d+)h$");

            public int Handle(string request, Func<string, int> next)
            {
                if (!_pattern.IsMatch(request))
                {
                    return next.Invoke(request);
                }

                var match = _pattern.Match(request);
                var hourAsText = match.Groups[1].Value;

                return (int) Math.Round(double.Parse(hourAsText) * 60);
            }
        }

        internal class MinuteParser : IWorklogParser
        {
            private readonly Regex _pattern = new Regex("^(\\d+)m$");

            public int Handle(string request, Func<string, int> next)
            {
                if (!_pattern.IsMatch(request))
                {
                    return next.Invoke(request);
                }

                var match = _pattern.Match(request);
                var minuteAsText = match.Groups[1].Value;

                return int.Parse(minuteAsText);
            }
        }

        internal interface IWorklogParser : IHandler<string, int>
        {
        }
    }
}