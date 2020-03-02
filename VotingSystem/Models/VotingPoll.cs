using System.Collections.Generic;

namespace VotingSystem.Models
{
    public class VotingPoll
    {
        public VotingPoll()
        {
            Counters = new List<Counter>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public List<Counter> Counters { get; set; }
    }
}
