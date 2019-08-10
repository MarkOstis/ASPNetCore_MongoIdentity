using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCore_MongoIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ASPNetCore_MongoIdentity.Pages
{

    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<ApplicationRole> _roleManager;


        public bool InvalidLoginData { get; set; }

        public LoginModel(UserManager<ApplicationUser> userManager,
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

        }

        public async Task<IActionResult> OnPostLogIn(LoginUser loginUser)
        {
            ApplicationUser user = await _userManager.FindByNameAsync(loginUser.UserName);

            if (user != null)
            {
                var result2 = await _signInManager.PasswordSignInAsync(user.Email, loginUser.Password, loginUser.RememberMe, lockoutOnFailure: true);
                if (result2.Succeeded)
                {
                    return Redirect("/UserHomePage");
                }
            }

            InvalidLoginData = true;
            return Page();
        }

    }
}