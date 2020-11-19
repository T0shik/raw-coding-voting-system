using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using VotingSystem.Integration.Tests.Infrastructure;
using VotingSystem.Integration.Tests.Ui;
using VotingSystem.Models;

namespace VotingSystem.Integration.Tests.Fixtures
{
    public class VotingFixture : WebApplicationFactory<VotingSystem.Ui.Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, AuthMock>("Test", _ => { });

                services.AddAntiforgery(setup =>
                {
                    setup.Cookie.Name = "test_csrf_cookie";
                    setup.FormFieldName = "test_csrf_field";
                });

                DbContextUtils.ActionDatabase(services.BuildServiceProvider(), ctx =>
                {
                    ctx.VotingPolls.Add(new VotingPoll
                    {
                        Title = "title",
                        Description = "desc",
                        Counters = new List<Counter> {
                        new Counter { Name = "One" },
                        new Counter { Name = "Two" }
                    }
                    });
                    ctx.SaveChanges();
                });
            });
        }
    }
}
