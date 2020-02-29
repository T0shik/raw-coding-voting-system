using Moq;
using VotingSystem.Application;
using VotingSystem.Models;
using Xunit;

namespace VotingSystem.Applicaiton.Tests
{
    public class VotingIntoractorTests
    {
        private readonly Mock<IVotingSystemPersistance> _mockPersistance = new Mock<IVotingSystemPersistance>();
        private readonly VotingIntoractor _intoractor;
        private readonly Vote _vote = new Vote { UserId = "user", CounterId = 1 };

        public VotingIntoractorTests()
        {
            _intoractor = new VotingIntoractor(_mockPersistance.Object);
        }

        [Fact]
        public void Vote_PersistsVoteWhenUserHasntVoted()
        {
            _mockPersistance.Setup(x => x.VoteExists(_vote)).Returns(false);

            _intoractor.Vote(_vote);

            _mockPersistance.Verify(x => x.SaveVote(_vote));
        }


        [Fact]
        public void Vote_DoesntPersistVoteWhenUserAlreadyVoted()
        {
            _mockPersistance.Setup(x => x.VoteExists(_vote)).Returns(true);

            _intoractor.Vote(_vote);

            _mockPersistance.Verify(x => x.SaveVote(_vote), Times.Never);
        }
    }
}
