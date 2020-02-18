using System;
using System.Linq;
using VotingSystem.Models;
using Xunit;
using static Xunit.Assert;

namespace VotingSystem.Tests
{
    public class VotingPollTests
    {
        [Fact]
        public void ZeroCountersWhenCreated()
        {
            var poll = new VotingPoll();

            Empty(poll.Counters);
        }
    }

    public class VotingPollFactoryTests
    {
        private readonly VotingPollFactory _factory = new VotingPollFactory();
        private readonly VotingPollFactory.Request _request = new VotingPollFactory.Request
        {
            Title = "title",
            Description = "description",
            Names = new[] { "name1", "name2" },
        };

        [Fact]
        public void Create_ThrowsWhenLessThanTwoCounterNames()
        {
            _request.Names = new[] { "name" };
            Throws<ArgumentException>(() => _factory.Create(_request));
            _request.Names = new string[] { };
            Throws<ArgumentException>(() => _factory.Create(_request));
        }

        [Fact]
        public void Create_AddsCounterToThePollForEachName()
        {
            var poll = _factory.Create(_request);

            foreach (var name in _request.Names)
            {
                Contains(name, poll.Counters.Select(x => x.Name));
            }
        }

        [Fact]
        public void Create_AddsTitleToThePoll()
        {
            var poll = _factory.Create(_request);

            Equal(_request.Title, poll.Title);
        }

        [Fact]
        public void Create_AddsDescriptionToThePoll()
        {
            var poll = _factory.Create(_request);

            Equal(_request.Description, poll.Description);
        }
    }
}
