using System.Collections.Generic;
using System.Linq;
using VotingSystem.Models;
using Xunit;
using static Xunit.Assert;

namespace VotingSystem.Tests
{
    public class CounterManagerTests
    {
        public const int CounterId = 1;
        public const string CounterName = "Counter Name";
        public Counter _counter = new Counter { Id = CounterId, Name = CounterName, Count = 5 };

        [Fact]
        public void GetStatistics_IncludesCounterId()
        {
            var statistics = new CounterManager().GetStatistics(new[] { _counter }).First();
            Equal(CounterId, statistics.Id);
        }

        [Fact]
        public void GetStatistics_IncludesCounterName()
        {
            var statistics = new CounterManager().GetStatistics(new[] { _counter }).First();
            Equal(CounterName, statistics.Name);
        }

        [Fact]
        public void GetStatistics_IncludesCounterCount()
        {
            var statistics = new CounterManager().GetStatistics(new[] { _counter }).First();
            Equal(5, statistics.Count);
        }

        [Theory]
        [InlineData(5, 10, 50)]
        [InlineData(1, 3, 33.33)]
        [InlineData(2, 3, 66.67)]
        [InlineData(2, 8, 25)]
        [InlineData(0, 0, 0)]
        public void GetStatistics_ShowsPercentageUpToTwoDecimalsBasedOnTotalCount(int count, int total, double expected)
        {
            _counter.Count = count;
            var counter = new Counter { Count = total - count };
            var statistics = new CounterManager().GetStatistics(new[] { _counter, counter }).First();
            Equal(expected, statistics.Percent);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(33.33)]
        public void ResolveExcess_DoesntAddExcesswhenAllCountersAreEqual(double percent)
        {
            var counter1 = new CounterStatistics { Percent = percent };
            var counter2 = new CounterStatistics { Percent = percent };
            var counter3 = new CounterStatistics { Percent = percent };
            var counters = new List<CounterStatistics> { counter1, counter2, counter3 };

            new CounterManager().ResolveExcess(counters);

            Equal(percent, counter1.Percent);
            Equal(percent, counter2.Percent);
            Equal(percent, counter3.Percent);
        }

        [Theory]
        [InlineData(66.66, 66.67, 33.33)]
        [InlineData(66.65, 66.67, 33.33)]
        [InlineData(66.66, 66.68, 33.32)]
        public void ResolveExcess_AddsExcessToHighestCounter(double initial, double expected, double lowest)
        {
            var counter1 = new CounterStatistics { Percent = initial };
            var counter2 = new CounterStatistics { Percent = lowest };
            var counters = new List<CounterStatistics> { counter1, counter2 };

            new CounterManager().ResolveExcess(counters);

            Equal(expected, counter1.Percent);
            Equal(lowest, counter2.Percent);

            var counter3 = new CounterStatistics { Percent = initial };
            var counter4 = new CounterStatistics { Percent = lowest };
            counters = new List<CounterStatistics> { counter4, counter3 };

            new CounterManager().ResolveExcess(counters);

            Equal(expected, counter3.Percent);
            Equal(lowest, counter4.Percent);
        }

        [Theory]
        [InlineData(11.11, 11.12, 44.44)]
        [InlineData(11.10, 11.12, 44.44)]
        public void ResolveExcess_AddsExcessToLowestCounterWhenMoreThanOneHighestCounters(double initial, double expected, double highest)
        {
            var counter1 = new CounterStatistics { Percent = highest };
            var counter2 = new CounterStatistics { Percent = highest };
            var counter3 = new CounterStatistics { Percent = initial };
            var counters = new List<CounterStatistics> { counter1, counter2, counter3 };

            new CounterManager().ResolveExcess(counters);

            Equal(highest, counter1.Percent);
            Equal(highest, counter2.Percent);
            Equal(expected, counter3.Percent);
        }


        [Fact]
        public void ResolveExcess_DoesntAddExcessIfTotalPercentIs100()
        {
            var counter1 = new CounterStatistics { Percent = 80 };
            var counter2 = new CounterStatistics { Percent = 20 };
            var counters = new List<CounterStatistics> { counter1, counter2 };

            new CounterManager().ResolveExcess(counters);

            Equal(80, counter1.Percent);
            Equal(20, counter2.Percent);
        }

    }
}
