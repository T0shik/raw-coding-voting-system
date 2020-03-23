using VotingSystem.Models;

namespace VotingSystem
{
    public interface IVotingSystemPersistance
    {
        void SaveVotingPoll(VotingPoll votingPoll);
        void SaveVote(Vote vote);
        bool VoteExists(Vote vote);
        VotingPoll GetPoll(int pollId);
    }
}
