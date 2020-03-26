using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using VotingSystem.Database.Tests.Infrastructure;
using VotingSystem.Models;
using Xunit;
using static Xunit.Assert;

namespace VotingSystem.Database.Tests
{
    public class VotingSystemPersistanceTests
    {
        [Fact]
        public void PersistsVotingPoll()
        {
            var poll = new VotingPoll
            {
                Title = "title",
                Description = "desc",
                Counters = new List<Counter> {
                    new Counter { Name = "One" },
                    new Counter { Name = "Two" }
                }
            };

            using (var ctx = DbContextFactory.Create(nameof(PersistsVotingPoll)))
            {
                IVotingSystemPersistance persistance = new VotingSystemPersistance(ctx);
                persistance.SaveVotingPoll(poll);
            }

            using (var ctx = DbContextFactory.Create(nameof(PersistsVotingPoll)))
            {
                var savedPoll = ctx.VotingPolls
                    .Include(x => x.Counters)
                    .Single();

                Equal(poll.Title, savedPoll.Title);
                Equal(poll.Description, savedPoll.Description);
                Equal(poll.Counters.Count(), savedPoll.Counters.Count());

                foreach(var name in poll.Counters.Select(x => x.Name))
                {
                    Contains(name, savedPoll.Counters.Select(x => x.Name));
                }
            }
        }

        [Fact]
        public void PersistsVote()
        {
            var vote = new Vote { UserId = "user", CounterId = 1 };
            using (var ctx = DbContextFactory.Create(nameof(PersistsVote)))
            {
                var persistance = new VotingSystemPersistance(ctx);
                persistance.SaveVote(vote);
            }

            using (var ctx = DbContextFactory.Create(nameof(PersistsVote)))
            {
                var savedVote = ctx.Votes.Single();
                Equal(vote.UserId, savedVote.UserId);
                Equal(vote.CounterId, savedVote.CounterId);
            }
        }

        [Fact]
        public void VoteExists_ReturnsFalseWhenNoVote()
        {
            var vote = new Vote { UserId = "user", CounterId = 1 };

            using (var ctx = DbContextFactory.Create(nameof(VoteExists_ReturnsFalseWhenNoVote)))
            {
                var persistance = new VotingSystemPersistance(ctx);
                False(persistance.VoteExists(vote));
            }
        }

        [Fact]
        public void VoteExists_ReturnsTrueWhenVoteExists()
        {
            var vote = new Vote { UserId = "user", CounterId = 1 };

            using (var ctx = DbContextFactory.Create(nameof(VoteExists_ReturnsTrueWhenVoteExists)))
            {
                ctx.Votes.Add(vote);
                ctx.SaveChanges();
            }

            using (var ctx = DbContextFactory.Create(nameof(VoteExists_ReturnsTrueWhenVoteExists)))
            {
                var persistance = new VotingSystemPersistance(ctx);
                True(persistance.VoteExists(vote));
            }
        }

        
        [Fact]
        public void GetPoll_ReturnsSavedPollWithCounters_AndVotesAsCount()
        {
            var poll = new VotingPoll
            {
                Title = "title",
                Description = "desc",
                Counters = new List<Counter> {
                    new Counter { Name = "One" },
                    new Counter { Name = "Two" }
                }
            };

            using (var ctx = DbContextFactory.Create(nameof(GetPoll_ReturnsSavedPollWithCounters_AndVotesAsCount)))
            {
                ctx.VotingPolls.Add(poll);
                ctx.Votes.Add(new Vote { UserId = "a", CounterId = 1 });
                ctx.Votes.Add(new Vote { UserId = "b", CounterId = 1 });
                ctx.Votes.Add(new Vote { UserId = "c", CounterId = 2 });
                ctx.SaveChanges();
            }

            using (var ctx = DbContextFactory.Create(nameof(GetPoll_ReturnsSavedPollWithCounters_AndVotesAsCount)))
            {
                var savedPoll = new VotingSystemPersistance(ctx).GetPoll(1);

                Equal(poll.Title, savedPoll.Title);
                Equal(poll.Description, savedPoll.Description);
                Equal(poll.Counters.Count(), savedPoll.Counters.Count());

                var counter1 = savedPoll.Counters[0];
                Equal(1, counter1.Id);
                Equal("One", counter1.Name);
                Equal(2, counter1.Count);

                var counter2 = savedPoll.Counters[1];
                Equal(2, counter2.Id);
                Equal("Two", counter2.Name);
                Equal(1, counter2.Count);
            }
        }
    }
}
