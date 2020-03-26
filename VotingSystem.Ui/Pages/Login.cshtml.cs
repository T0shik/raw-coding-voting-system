using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace VotingSystem.Ui.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string email)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email)
            };

            var identity = new ClaimsIdentity(claims, "Voting System");

            var principal = new ClaimsPrincipal(new[] { identity });

            await HttpContext.SignInAsync(principal);

            return RedirectToPage("/Index");
        }
    }
}
