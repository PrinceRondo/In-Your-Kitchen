using InYourFridge.Data;
using InYourFridge.Helper;
using InYourFridge.Models;
using InYourFridge.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace InYourFridge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly AppDbContext dbContext;
        private readonly Utility utility;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, AppDbContext dbContext, Utility utility,
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.utility = utility;
            this.roleManager = roleManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.Code = null;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(LoginViewModel viewModel, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await userManager.FindByEmailAsync(viewModel.Email);
                    if (user != null && !user.EmailConfirmed && (await userManager.CheckPasswordAsync(user, viewModel.Password)))
                    {
                        ViewBag.Message = "Email not confirmed yet";
                        ViewBag.Code = 300;
                        return View(viewModel);
                    }
                    var result = await signInManager.PasswordSignInAsync(viewModel.Email, viewModel.Password, viewModel.RememberMe, true);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            if (await userManager.IsInRoleAsync(user, "admin"))
                                return RedirectToAction("Index", "Admin");
                            else if(await userManager.IsInRoleAsync(user, "user"))
                                return RedirectToAction("Index", "User");
                        }
                    }
                    ViewBag.Message = "Login failed";
                    ViewBag.Code = 301;
                    return View();
                }
                ViewBag.Message = "Invalid entry";
                ViewBag.Code = 302;
                return View();
            }
            catch (Exception ex)
            {
                //save error to db
                ErrorLog errorLog = new ErrorLog
                {
                    ErrorDate = DateTime.Now,
                    ErrorMessage = ex.Message,
                    ErrorSource = ex.Source,
                    ErrorStackTrace = ex.StackTrace
                };
                await dbContext.ErrorLogs.AddAsync(errorLog);
                await dbContext.SaveChangesAsync();
                //save error to file
                utility.SaveErrorMessage(ex);
                ViewBag.Message = "Error occured, kindly contact the system admin";
                ViewBag.Code = 500;
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
