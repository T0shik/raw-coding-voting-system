using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VotingSystem.Models;

namespace VotingSystem.Ui.Pages
{
    public class IndexModel : PageModel
    {
        public VotingPoll Poll { get; set; }
        [BindProperty]
        public VotingPollFactory.Request Form { get; set; }

        public void OnGet()
        {
            //var request = new VotingPollFactory.Request
            //{
            //       Title = "title",
            //       Description = "desc",
            //       Names = new [] { "one", "two" }
            //};
            //Poll = pollFactory.Create(request);
        }
        
        public void OnPost([FromServices] IVotingPollFactory pollFactory)
        {
            Poll = pollFactory.Create(Form);
        }
    }
}
