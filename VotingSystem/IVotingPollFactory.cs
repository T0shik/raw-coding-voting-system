using VotingSystem.Models;

namespace VotingSystem
{
    public interface IVotingPollFactory
    {
        VotingPoll Create(VotingPollFactory.Request request);
    }
}
