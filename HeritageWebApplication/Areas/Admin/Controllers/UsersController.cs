using System.Linq;
using System.Threading.Tasks;
using HeritageWebApplication.Areas.Admin.ViewModels;
using HeritageWebApplication.Models;
using HeritageWebApplication.Services;
using HeritageWebApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeritageWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        UserManager<User> _userManager;
        private IInfoService _infoService;
 
        public UsersController(UserManager<User> userManager, IInfoService infoService)
        {
            _userManager = userManager;
            _infoService = infoService;
        }

        public async Task<IActionResult> Index()
        {
            User cur_user = await _userManager.GetUserAsync(HttpContext.User);
            ViewBag.userMail = cur_user.Email;
            ViewBag.time = _infoService.StartTime().ToString();
            return View(_userManager.Users.ToList());
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
            }
            return Redirect("/admin/users");
        }
        
        public async Task<IActionResult> ChangePassword(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ChangePasswordViewModel model = new ChangePasswordViewModel { Id = user.Id, Email = user.Email};
            return View(model);
        }
 
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id.ToString());
                if (user != null)
                {
                    var _passwordValidator = 
                        HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                    var _passwordHasher =
                        HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;
     
                    IdentityResult result = 
                        await _passwordValidator.ValidateAsync(_userManager, user, model.NewPassword);
                    if(result.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.NewPassword);
                        await _userManager.UpdateAsync(user);
                        return Redirect("/admin/users");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
                }
            }
            return View(model);
        }
    }
}