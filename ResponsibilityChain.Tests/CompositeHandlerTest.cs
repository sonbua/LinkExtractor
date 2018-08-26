using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using EnsureThat;
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

        [Theory]
        [InlineData("30m 10m")]
        [InlineData("1h 2h")]
        public void GivenUnitDuplication_ThrowsException(string workLog)
        {
            // arrange
            IWorkLogParser parser = new WorkLogParser();

            // act
            Action testDelegate = () => parser.Handle(workLog, null);

            // assert
            Assert.Throws<ArgumentException>(testDelegate);
        }

        [Theory]
        [InlineData("30m 1h")]
        public void GivenUnitsInAscendingOrder_ThrowsException(string workLog)
        {
            // arrange
            IWorkLogParser parser = new WorkLogParser();

            // act
            Action testDelegate = () => parser.Handle(workLog, null);

            // assert
            Assert.Throws<ArgumentException>(testDelegate);
        }

        private class WorkLogParser : CompositeHandler<string, int>, IWorkLogParser
        {
            public WorkLogParser()
            {
                AddHandler(new WorkLogValidator());
                AddHandler(new TechnicalLeaderParser());
            }

            private class WorkLogValidator : CompositeHandler<string, int>, IWorkLogParser
            {
                public WorkLogValidator()
                {
                    AddHandler(new WorkLogMustNotBeNullOrEmptyRule());
                    AddHandler(new ThereShouldBeNoUnitDuplicationRule());
                    AddHandler(new UnitsMustBeInDescendingOrderRule());
                }

                private class WorkLogMustNotBeNullOrEmptyRule : IWorkLogParser
                {
                    public int Handle(string request, Func<string, int> next)
                    {
                        EnsureArg.IsNotNullOrEmpty(request, nameof(request));

                        return next.Invoke(request);
                    }
                }

                private class ThereShouldBeNoUnitDuplicationRule : IWorkLogParser
                {
                    public int Handle(string request, Func<string, int> next)
                    {
                        var duplicatedUnitGrouping =
                            request.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Last())
                                .GroupBy(x => x)
                                .FirstOrDefault(g => g.Count() > 1);

                        if (duplicatedUnitGrouping != null)
                        {
                            throw new ArgumentException("Duplicate unit: " + duplicatedUnitGrouping.Key);
                        }

                        return next.Invoke(request);
                    }
                }

                private class UnitsMustBeInDescendingOrderRule : IWorkLogParser
                {
                    private static readonly Dictionary<char, int> UnitOrderMap;

                    static UnitsMustBeInDescendingOrderRule()
                    {
                        UnitOrderMap = new Dictionary<char, int>
                        {
                            {'h', 2},
                            {'m', 1},
                        };
                    }

                    public int Handle(string request, Func<string, int> next)
                    {
                        var units = request.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                            .Select(x => x.Last());
                        var unitOrders = units.Select(unit => UnitOrderMap[unit]).ToList();

                        for (var i = 0; i < unitOrders.Count - 1; i++)
                        {
                            if (unitOrders[i] < unitOrders[i + 1])
                            {
                                throw new ArgumentException("Unit must be in descending order: " + unitOrders[i + 1]);
                            }
                        }

                        return next.Invoke(request);
                    }
                }
            }

            private class TechnicalLeaderParser : CompositeHandler<string, int>, IWorkLogParser
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

                private class HourParser : IWorkLogParser
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

                private class MinuteParser : IWorkLogParser
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
            }
        }

        private interface IWorkLogParser : IHandler<string, int>
        {
        }
    }
}