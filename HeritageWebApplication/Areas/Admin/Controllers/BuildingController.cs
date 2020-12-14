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
    public class BuildingController : Controller
    {
        private ApplicationDbContext db;
        private IDataManager _manager;
        private IWebHostEnvironment _environment;
        private IImageService _imageService;
        private IHubContext<ChatHub> _hubContext;

        public BuildingController(ApplicationDbContext context, IDataManager dataManager, 
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
            List<Building> buildings = _manager.BuildingRepository.GetAll();
            return View(buildings);
        }

        public IActionResult Create() => View();
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateBuildingViewModel model)
        {
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (model.Image != null)
                {
                    fileName = await UploadFile(model.Image);
                }
                Building building = new Building();
                building.Name = model.Name;
                building.Image = fileName;
                building.ShortDesc = model.ShortDesc;
                building.Location = model.Location;
                building.Desc = model.Desc;
                building.RenovationCompanies = new List<RenovationCompany>();
                _manager.BuildingRepository.Save(building);
                return Redirect("/admin/building");
                
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Building building = _manager.BuildingRepository.Get(id);
            if (building == null)
            {
                return NotFound();
            }

            EditBuildingViewModel model = new EditBuildingViewModel
            {
                Name = building.Name,
                Desc = building.Desc,
                ImagePath = building.Image,
                Location = building.Location,
                ShortDesc = building.ShortDesc
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditBuildingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Building building = _manager.BuildingRepository.Get(model.Id);
                if(building!=null)
                {
                    if (model.Image != null)
                    {
                        if (building.Image != null)
                            DeleteFile(building.Image);
                        building.Image = await UploadFile(model.Image);
                    }
                    
                    building.Id = model.Id;
                    building.Name = model.Name;
                    building.ShortDesc = model.ShortDesc;
                    building.Location = model.Location;
                    building.Desc = model.Desc;
                    
                    _manager.BuildingRepository.Update(building);
                    return Redirect("/admin/building");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            Building building = _manager.BuildingRepository.Get(id);
            if (building != null)
            {
                _manager.BuildingRepository.Delete(id);
            }
            return Redirect("/admin/building");
        }
        
        private async Task<string> UploadFile(IFormFile file)
        {
            string uploadDir = Path.Combine(_environment.WebRootPath, "img/heritageObjects");

            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }
            
            string fileName = $"{Guid.NewGuid().ToString()}_{file.FileName}";
            string filePath = Path.Combine(uploadDir, fileName);
            
            await _imageService.SaveImage(file, filePath);
            return fileName;
        }

        private void DeleteFile(string fileName)
        {
            string uploadDir = Path.Combine(_environment.WebRootPath, "img/albums");
            string filePath = Path.Combine(uploadDir, fileName);
            
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}