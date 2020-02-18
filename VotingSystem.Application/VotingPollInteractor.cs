namespace VotingSystem.Application
{
    public class VotingPollInteractor
    {
        private readonly IVotingPollFactory _factory;
        private readonly IVotingSystemPersistance _persistance;

        public VotingPollInteractor(
            IVotingPollFactory factory,
            IVotingSystemPersistance persistance)
        {
            _factory = factory;
            _persistance = persistance;
        }

        public void CreateVotingPoll(VotingPollFactory.Request request)
        {
            var poll = _factory.Create(request);

            _persistance.SaveVotingPoll(poll);
        }
    }
}
