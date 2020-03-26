using System;
using System.Collections.Generic;
using System.Linq;
using VotingSystem.Models;

namespace VotingSystem
{
    public class CounterManager : ICounterManager
    {
        public List<CounterStatistics> GetStatistics(ICollection<Counter> counters)
        {
            var totalCount = counters.Sum(x => x.Count);

            return counters.Select(x => new CounterStatistics
            {
                Id = x.Id,
                Name = x.Name,
                Count = x.Count,
                Percent = totalCount > 0 ? RoundUp(x.Count * 100.0 / totalCount) : 0
            }).ToList();
        }

        public void ResolveExcess(List<CounterStatistics> counters)
        {
            var totalPercent = counters.Sum(x => x.Percent);
            if (totalPercent == 100) return;

            var excess = 100 - totalPercent;

            var highestPercent = counters.Max(x => x.Percent);
            var highestCounters = counters.Where(x => x.Percent == highestPercent).ToList();

            if (highestCounters.Count == 1)
            {
                highestCounters.First().Percent += excess;
            }
            else if (highestCounters.Count < counters.Count)
            {
                var lowestPercent = counters.Min(x => x.Percent);
                var lowestCounter = counters.First(x => x.Percent == lowestPercent);
                lowestCounter.Percent = RoundUp(lowestCounter.Percent + excess);
            }
        }

        private static double RoundUp(double num) => Math.Round(num, 2);
    }
}
