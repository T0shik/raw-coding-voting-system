using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VotingSystem.Application;
using VotingSystem.Models;

namespace VotingSystem.Ui.Pages
{
    public class PollModel : PageModel
    {
        public PollStatistics Statistics { get; set; }

        public void OnGet(int id, [FromServices] StatisticsInteractor interactor)
        {
            Statistics = interactor.GetStatistics(id);
        }

        public IActionResult OnPost(int counterId, [FromServices] VotingInteractor interactor)
        {
            var email = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;

            interactor.Vote(new Vote
            {
                UserId = email,
                CounterId = counterId
            });

            return Redirect(Request.Path.Value);
        }
    }
}
