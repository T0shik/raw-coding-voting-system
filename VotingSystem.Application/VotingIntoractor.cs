using VotingSystem.Application;
using VotingSystem.Models;

namespace VotingSystem.Applicaiton
{
    public class VotingIntoractor
    {
        private readonly IVotingSystemPersistance _persistance;

        public VotingIntoractor(IVotingSystemPersistance persistance)
        {
            _persistance = persistance;
        }

        public void Vote(Vote vote)
        {
            if (!_persistance.VoteExists(vote))
            {
                _persistance.SaveVote(vote);
            }
        }
    }
}
