using HtmlAgilityPack;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using VotingSystem.Database;
using VotingSystem.Integration.Tests.Infrastructure;
using VotingSystem.Models;
using VotingSystem.Ui;
using Xunit;
using BaseVotingTests = VotingSystem.Integration.Tests.VotingTests;

namespace VotingSystem.Integration.Tests.Ui
{
    public class VotingTests : IClassFixture<WebApplicationFactory<VotingSystem.Ui.Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public VotingTests(WebApplicationFactory<VotingSystem.Ui.Startup> factory)
        {
            _factory = factory;
        }

        public void ActionDatabase(Action<AppDbContext> action)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                action(ctx);
            }
        }

        [Fact]
        public async Task OnGet()
        {
            var client = _factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddAuthentication("Test")
                            .AddScheme<AuthenticationSchemeOptions, AuthMock>("Test", _ => { });

                        services.AddAntiforgery(setup =>
                        {
                            setup.Cookie.Name = "test_csrf_cookie";
                            setup.FormFieldName = "test_csrf_field";
                        });
                    });
                })
                .CreateClient();
            ActionDatabase(ctx =>
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
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
            using (var scope = _factory.Services.CreateScope())
            {
                var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
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
            }
            var pollPage = await client.GetAsync("/Poll/1");
            var pollHtml = await pollPage.Content.ReadAsStringAsync();

            var cookieToken = AntiForgeryUtils.ExtractCookieToken(pollPage.Headers);
            var formToken = AntiForgeryUtils.ExtractFormToken(pollHtml, "test_csrf_field");

            var request = new HttpRequestMessage(HttpMethod.Post, "/Poll/1");
            request.Content = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("counterId", "1"),
                new KeyValuePair<string, string>("test_csrf_field", formToken)
            });
            request.Headers.Add("Cookie", $"test_csrf_cookie={cookieToken}");
            var response = await client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            ActionDatabase(ctx =>
            {
                BaseVotingTests.AssertVotedForCounter(ctx, "test@test.com", 1);
            });
        }
    }

    public class AuthMock : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public AuthMock(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Email, "test@test.com") };
            var identity = new ClaimsIdentity(claims, "Test Voting System");
            var principal = new ClaimsPrincipal(new[] { identity });
            var ticket = new AuthenticationTicket(principal, "Test");
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
