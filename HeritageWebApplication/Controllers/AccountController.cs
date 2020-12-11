using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HeritageWebApplication.Models;
using HeritageWebApplication.Services;
using HeritageWebApplication.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HeritageWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        RoleManager<UserRole> _roleManager;
        private ILogger<Startup> _logger;
        
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
                                 RoleManager<UserRole> userRole, ILogger<Startup> logger, IEmailService emailService)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = userRole;
            this._logger = logger;
            this._emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Redirect()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Redirect(int id)
        {
            return Redirect("/admin/roles");
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Name };
                var result = await _userManager.CreateAsync(user, model.Password);
                List<string> roles = new List<string>();
                roles.Append("user");
                await _userManager.AddToRolesAsync(user, roles);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, code = code },
                        protocol: HttpContext.Request.Scheme);
                    ValidationSetting();
                    await _emailService.SendEmailAsync(model.Email, "Confirm your account",
                        $"Confirm your registration by clicking the link: <a href='{callbackUrl}'>link</a>");

                    return RedirectToAction("Redirect");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if(result.Succeeded)
                return RedirectToAction("Index", "Home");
            else
                return View("Error");
        }
 
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {

                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        
        static void ValidationSetting()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate,
                    X509Chain chain, SslPolicyErrors sslPolicyErrors) 
                {
                    return true;
                };
        }
        
    }
}