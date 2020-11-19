using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using VotingSystem.Integration.Tests.Fixtures;
using VotingSystem.Integration.Tests.Infrastructure;
using Xunit;
using BaseVotingTests = VotingSystem.Integration.Tests.VotingTests;

namespace VotingSystem.Integration.Tests.Ui
{
    public class VotingTests : IClassFixture<VotingFixture>
    {
        private readonly VotingFixture _factory;

        public VotingTests(VotingFixture factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task OnGet()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Test");
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
            DbContextUtils.ActionDatabase(_factory.Services, ctx =>
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
