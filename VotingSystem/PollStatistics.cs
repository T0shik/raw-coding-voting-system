using System.Collections.Generic;

namespace VotingSystem
{
    public class PollStatistics
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<CounterStatistics> Counters { get; set; }
    }
}
