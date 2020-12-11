using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HeritageWebApplication.Areas.Admin.ViewModels;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;


namespace HeritageWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db;
        private IDataManager _manager;
        private readonly UserManager<User> _userManager;
        private IHubContext<ChatHub> _hubContext;

        public HomeController(ApplicationDbContext context, IDataManager dataManager, UserManager<User> userManager,
                              IHubContext<ChatHub> hubContext)
        {
            db = context;
            _manager = dataManager;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        
        public IActionResult Index()
        {
            List<Building> buildings = _manager.BuildingRepository.GetAll();
            return View(buildings);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(string product)
        {
            //await _hubContext.Clients.All.SendAsync("Notify", $"Добавлено: {DateTime.Now.ToShortTimeString()}");
            return RedirectToAction("Index");
        }
        
        public async Task<IActionResult> EditUserData()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.UserMail = user.Email;
            ChangeDataViewModel model = new ChangeDataViewModel {Id = user.Id, Mail = user.Email, Name = user.UserName };
            return View(model);
        }
 
        [HttpPost]
        public async Task<IActionResult> EditUserData(ChangeDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByIdAsync(model.Id.ToString());
                if(user!=null)
                {
                    user.UserName = model.Name;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }
        
        public IActionResult Container(int id)
        {
            List<HeritageObject> heritageObjects = _manager.HeritageObjectRepository.GetAll();
            List<HeritageObject> result = new List<HeritageObject>();
            foreach (var heritageObject in heritageObjects)
            {
                if (heritageObject.BuildingId == id)
                    result.Add(heritageObject);
            }
            return View(result);
        }

        public IActionResult Redirect(int id)
        {
            return Redirect("/HeritageComment/Index/" + id.ToString());
        }
    }
}