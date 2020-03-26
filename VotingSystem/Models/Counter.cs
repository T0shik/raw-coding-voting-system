using System.Collections.Generic;

namespace VotingSystem.Models
{
    public class Counter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }
        public ICollection<Vote> Votes { get; set; }
    }
}
