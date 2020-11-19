using Microsoft.Extensions.DependencyInjection;
using System;
using VotingSystem.Database;

namespace VotingSystem.Integration.Tests.Infrastructure
{
    public class DbContextUtils
    {
        public static void ActionDatabase(IServiceProvider provider, Action<AppDbContext> action)
        {
            using (var scope = provider.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                action(ctx);
            }
        }
    }
}
