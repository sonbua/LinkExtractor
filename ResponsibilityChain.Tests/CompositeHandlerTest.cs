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
        public void GivenTextWithMinuteUnit_ReturnsCorrectNumberOfMinutes(string workLog, int expected)
        {
            // arrange
            IWorkLogParser parser = new WorkLogParser();

            // act
            var actual = parser.Handle(workLog, null);

            // assert
            Assert.Equal(expected, actual);
        }

        internal class WorkLogParser : CompositeHandler<string, int>, IWorkLogParser
        {
            public WorkLogParser()
            {
                AddHandler(new TechnicalLeaderParser());
            }
        }

        internal class TechnicalLeaderParser : CompositeHandler<string, int>, IWorkLogParser
        {
            public TechnicalLeaderParser()
            {
                AddHandler(new HourParser());
                AddHandler(new MinuteParser());
            }

            public override int Handle(string request, Func<string, int> next)
            {
                return request.Split(' ').Select(piece => base.Handle(piece, next)).Sum();
            }
        }

        internal class HourParser : IWorkLogParser
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

        internal class MinuteParser : IWorkLogParser
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

        internal interface IWorkLogParser : IHandler<string, int>
        {
        }
    }
}