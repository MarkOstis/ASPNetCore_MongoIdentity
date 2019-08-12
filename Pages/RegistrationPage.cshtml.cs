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
using System.Security.Claims;
using ASPNetCore_MongoIdentity.Services;

namespace ASPNetCore_MongoIdentity.Pages
{
    public class RegistrationPageModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public bool ErrorFlag { get; set; }

        public List<ApplicationUser> DisplayUsers { get; set; }

        public RegistrationPageModel(UserManager<ApplicationUser> userManager,
                                     SignInManager<ApplicationUser> signInManager,
                                     IConfiguration configuration,
                                     RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        public void OnGet()
        {
            DisplayUsers = _userManager.Users.ToList<ApplicationUser>();
        }

        public async Task<IActionResult> OnPostRegisterNewUser(RegisterUser regUser)
        {
            var user = new ApplicationUser { Name = regUser.UserName,
                                             UserName = regUser.UserName,
                                             Email = regUser.UserName
            };

            bool isRoleExists = await _roleManager.RoleExistsAsync("Admin");
            if (isRoleExists == false)
            {
                var role = new ApplicationRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

            }

            var result = await _userManager.CreateAsync(user, regUser.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");

                var claims = new List<Claim>
                {
                    new Claim("user", user.UserName),
                    new Claim("role", "Admin")
                };
                result = await _userManager.AddClaimsAsync(user, claims);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);

                    //                var result2 = await _signInManager.PasswordSignInAsync(user.Email, inputUser.Password, true, lockoutOnFailure: true);
                    //if (result2.Succeeded)
                    //{
                    //    return LocalRedirect("/UserHomePage");
                    //}
                    //                var token = AuthenticationHelper.GenerateJwtToken(inputUser.Email, user, _configuration);
                    //               var tokenProvider = new AuthenticatorTokenProvider<ApplicationUser>();
                    //                var token = tokenProvider.
                    //                var rootData = new SignUpResponse(token, user.UserName, user.Email);
                    return Redirect("/UserHomePage");
                }
            }

            ErrorFlag = true;

            DisplayUsers = _userManager.Users.ToList();

            return Page();

        }

        public PartialViewResult OnGetUserListPartial()
        {
            int counter = SimpleCounterService.GetCounter();
            DisplayUsers = _userManager.Users.ToList<ApplicationUser>();
            return Partial("_UserListPartial", this);
        }
    }
}