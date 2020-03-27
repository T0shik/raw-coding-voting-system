using Moq;
using VotingSystem.Application;
using VotingSystem.Models;
using Xunit;

namespace VotingSystem.Application.Tests
{
    public class VotingInteractorTests
    {
        private readonly Mock<IVotingSystemPersistance> _mockPersistance = new Mock<IVotingSystemPersistance>();
        private readonly VotingInteractor _interactor;
        private readonly Vote _vote = new Vote { UserId = "user", CounterId = 1 };

        public VotingInteractorTests()
        {
            _interactor = new VotingInteractor(_mockPersistance.Object);
        }

        [Fact]
        public void Vote_PersistsVoteWhenUserHasntVoted()
        {
            _mockPersistance.Setup(x => x.VoteExists(_vote)).Returns(false);

            _interactor.Vote(_vote);

            _mockPersistance.Verify(x => x.SaveVote(_vote));
        }


        [Fact]
        public void Vote_DoesntPersistVoteWhenUserAlreadyVoted()
        {
            _mockPersistance.Setup(x => x.VoteExists(_vote)).Returns(true);

            _interactor.Vote(_vote);

            _mockPersistance.Verify(x => x.SaveVote(_vote), Times.Never);
        }
    }
}
