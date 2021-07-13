using InYourFridge.Data;
using InYourFridge.Helper;
using InYourFridge.Repository;
using InYourFridge.ResponseModels;
using InYourFridge.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IAccountRepository accountRepository;
        private readonly Mailer mailer;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IAccountRepository accountRepository,
            Mailer mailer)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.accountRepository = accountRepository;
            this.mailer = mailer;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult UserRegistration()
        {
            ViewBag.Code = null;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserRegistration(ProfileViewModel model)
        {
            if (ModelState.IsValid)
            {

                var userExist = await userManager.FindByEmailAsync(model.Email);
                if (userExist != null)
                {
                    ViewBag.Message = "User already exist";
                    ViewBag.Code = 300;
                    return View();
                }
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                var result = await userManager.CreateAsync(user, model.Password);
                var role = await roleManager.FindByNameAsync("user");

                if (result.Succeeded && role != null)
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                    model.UserId = user.Id;
                    GenericResponseModel response = await accountRepository.RegisterUserAsync(model);

                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);
                    mailer.SendMail(confirmationLink, model.Email, "IYF User Registration", model.FirstName);
                    ViewBag.Message = response.StatusMessage;
                    ViewBag.Code = response.StatusCode;
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ViewBag.Message = "User creation failed";
                    ViewBag.Code = 301;
                    return View();
                }
            }
            ViewBag.Message = "Invalid Input";
            ViewBag.Code = 302;
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //check if user exist
            var user = await userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return View("NotFound");
            }
            //confirm email
            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }
            return View("Error");
        }
    }
}
