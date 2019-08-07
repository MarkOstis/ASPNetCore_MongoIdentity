using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ASPNetCore_MongoIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ASPNetCore_MongoIdentity.Pages
{
    public class UserHomePageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public UserHomePageModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }


        public async Task<IActionResult> OnGet(InputUserData inputUser)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(inputUser.UserName);

            if (user != null)
            {
                var claims = new List<Claim>
                    {
                        new Claim("user", user.UserName),
                        new Claim("role", "Member")
                    };

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role"));
                bool result3 = _signInManager.IsSignedIn(claimsPrincipal);

                if (result3 == true)
                {
                    return Page();
                }
                else
                {
                    return Redirect("/RegistrationPage");
                }
            }
            //ErrorFlag = true;
            return Page();
        }
    }
}