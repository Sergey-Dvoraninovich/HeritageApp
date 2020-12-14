using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HeritageWebApplication.Areas.Admin.ViewModels;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HeritageWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyController : Controller
    {
        private ApplicationDbContext db;
        private IDataManager _manager;
        private IWebHostEnvironment _environment;
        private IImageService _imageService;
        private IHubContext<ChatHub> _hubContext;

        public CompanyController(ApplicationDbContext context, IDataManager dataManager, 
                                  IImageService imageService, IWebHostEnvironment environment,
                                  IHubContext<ChatHub> hubContext)
        {
            db = context;
            _manager = dataManager;
            _environment = environment;
            _imageService = imageService;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            List<RenovationCompany> renovations = _manager.RenovatoinCompanyRepository.GetAll();
            return View(renovations);
        }

        public IActionResult Create() => View();
        
        [HttpPost]
        public async Task<IActionResult> Create(RenovationViewModel model)
        {
            if (ModelState.IsValid)
            {
                RenovationCompany renovation = new RenovationCompany();
                renovation.Name = model.Name;
                renovation.Desc = model.Desc;
                _manager.RenovatoinCompanyRepository.Save(renovation);
                return Redirect("/admin/company");
                
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            RenovationCompany renovation = _manager.RenovatoinCompanyRepository.Get(id);
            if (renovation == null)
            {
                return NotFound();
            }

            RenovationViewModel model = new RenovationViewModel()
            {
                Name = renovation.Name,
                Desc = renovation.Desc,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RenovationViewModel model)
        {
            if (ModelState.IsValid)
            {
                RenovationCompany renovation = _manager.RenovatoinCompanyRepository.Get(model.Id);
                if(renovation!=null)
                {
                    renovation.Id = model.Id;
                    renovation.Name = model.Name;
                    renovation.Desc = model.Desc;
                    
                    _manager.RenovatoinCompanyRepository.Update(renovation);
                    return Redirect("/admin/company");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            RenovationCompany renovation = _manager.RenovatoinCompanyRepository.Get(id);
            if (renovation != null)
            {
                _manager.RenovatoinCompanyRepository.Delete(id);
            }
            return Redirect("/admin/company");
        }
    }
}