using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCore_MongoIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ASPNetCore_MongoIdentity.Pages
{
    public class RegistrationPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public bool ErrorFlag { get; set; }

        public List<ApplicationUser> DisplayUsers { get; set; }

        public RegistrationPageModel(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async void OnGetAsync()
        {
//            DisplayUsers.Add(await _userManager.FindByIdAsync("0"));
        }

        public async Task<IActionResult> OnPostRegisterNewUser(InputUserData inputUser)
        {
            var user = new ApplicationUser { Name = inputUser.UserName,
                                             UserName = inputUser.UserName,
                                             Email = inputUser.UserName
            };
            var result = await _userManager.CreateAsync(user, inputUser.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);

//                var result2 = await _signInManager.PasswordSignInAsync(user.Email, inputUser.Password, true, lockoutOnFailure: true);
                //if (result2.Succeeded)
                //{
                //    return LocalRedirect("/UserHomePage");
                //}
                //                var token = AuthenticationHelper.GenerateJwtToken(inputUser.Email, user, _configuration);
                var tokenProvider = new AuthenticatorTokenProvider<ApplicationUser>();
                //                var token = tokenProvider.
//                var rootData = new SignUpResponse(token, user.UserName, user.Email);
                return Page();
            }

            return Page();

        }

        public async Task<IActionResult> OnPostLogIn(InputUserData inputUser)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(inputUser.UserName);

            if (user != null)
            {
                var result2 = await _signInManager.PasswordSignInAsync(user.Email, inputUser.Password, false, lockoutOnFailure: true);
                if (result2.Succeeded)
                {
                    return LocalRedirect("/UserHomePage");
                }
            }
            ErrorFlag = true;
            return Page();
        }

    }
}