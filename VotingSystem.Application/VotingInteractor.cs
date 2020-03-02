using VotingSystem.Application;
using VotingSystem.Models;

namespace VotingSystem.Applicaiton
{
    public class VotingInteractor
    {
        private readonly IVotingSystemPersistance _persistance;

        public VotingInteractor(IVotingSystemPersistance persistance)
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
