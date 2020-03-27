using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VotingSystem.Application;
using VotingSystem.Database;
using VotingSystem.Database.Tests.Infrastructure;
using VotingSystem.Models;
using Xunit;

namespace VotingSystem.Integration.Tests
{
    public class VotingTests
    {
        private AppDbContext _ctx;
        private IVotingSystemPersistance _persistance;
        private readonly VotingInteractor _votingInteractor;
        private readonly VotingPollInteractor _pollInteractor;

        public VotingTests()
        {
            _ctx = DbContextFactory.Create(Guid.NewGuid().ToString());
            _persistance = new VotingSystemPersistance(_ctx);
            _votingInteractor = new VotingInteractor(_persistance);
            _pollInteractor = new VotingPollInteractor(new VotingPollFactory(), _persistance);
        }

        private VotingPoll NewPoll() => new VotingPoll
        {
            Title = "title",
            Description = "desc",
            Counters = new List<Counter> {
                    new Counter { Name = "One" },
                    new Counter { Name = "Two" }
                }
        };

        [Fact]
        public void SavesVoteToDatabaseWhenVotingPollExists()
        {
            _ctx.VotingPolls.Add(NewPoll());
            _ctx.SaveChanges();

            _votingInteractor.Vote(new Vote { UserId = "user", CounterId = 1 });

            var vote = _ctx.Votes.Single();
            Assert.Equal("user", vote.UserId);
            Assert.Equal(1, vote.CounterId);
            
            var counter = _ctx.Counters.Include(x => x.Votes).First(x => x.Id == 1);
            Assert.Single(counter.Votes);
        }

        [Fact]
        public void SavesVoteToDatabaseAfterPollCreatedWithInteractor()
        {
            _pollInteractor.CreateVotingPoll(new VotingPollFactory.Request
            {
                Title = "Title",
                Description = "Description",
                Names = new[] { "One", "Two" }
            });

            _votingInteractor.Vote(new Vote { UserId = "user", CounterId = 1 });

            var vote = _ctx.Votes.Single();
            Assert.Equal("user", vote.UserId);
            Assert.Equal(1, vote.CounterId);

            var counter = _ctx.Counters.Include(x => x.Votes).First(x => x.Id == 1);
            Assert.Single(counter.Votes);
        }
    }
}
