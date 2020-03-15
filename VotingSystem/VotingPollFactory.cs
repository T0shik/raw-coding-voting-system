using System;
using System.Linq;
using VotingSystem.Models;

namespace VotingSystem
{
    public class VotingPollFactory : IVotingPollFactory
    {
        public class Request
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string[] Names { get; set; }
        }

        public VotingPoll Create(Request request)
        {
            if (request.Names.Length < 2) throw new ArgumentException();

            return new VotingPoll
            {
                Title = request.Title,
                Description = request.Description,
                Counters = request.Names.Select(name => new Counter { Name = name }).ToList()
            };
        }
    }
}
