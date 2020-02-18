using System.Collections.Generic;
using System.Linq;

namespace VotingSystem.Models
{
    public class VotingPoll
    {
        public VotingPoll()
        {
            Counters = Enumerable.Empty<Counter>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public IEnumerable<Counter> Counters { get; set; }
    }
}
