using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebApplication.Areas.Admin.ViewModels;
using HeritageWebApplication.Models;
using HeritageWebApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeritageWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        RoleManager<UserRole> _roleManager;
        UserManager<User> _userManager;
        public RolesController(RoleManager<UserRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        
        //public IActionResult Index() => View(_roleManager.Roles.ToList());

        public IActionResult Index() => View(_userManager.Users.ToList());
        
        /*public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                UserRole user = new UserRole();
                user.Name = name;
                IdentityResult result = await _roleManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }
         
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            UserRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }*/
        
        public async Task<IActionResult> Edit(int id)
        {
            User user = await _userManager.FindByIdAsync(id.ToString());
            if(user!=null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = user.Id,
                    UserEmail = user.Email,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }
 
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return Redirect("/admin/roles");
            }

            return NotFound();
        }
    }
}