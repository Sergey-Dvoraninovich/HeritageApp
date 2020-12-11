using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HeritageWebApplication.Areas.Admin.ViewModels;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.Services;
using HeritageWebApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HeritageWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HeritageObjectController : Controller
    {
        private ApplicationDbContext db;
        private IDataManager _manager;
        private IImageService _imageService;
        private IWebHostEnvironment _environment;


        public HeritageObjectController(ApplicationDbContext context, IDataManager dataManager,
                                        IImageService imageService, IWebHostEnvironment environment)
        {
            db = context;
            _manager = dataManager;
            _imageService = imageService;
            _environment = environment;
        }

        public IActionResult Index()
        {
            List<HeritageObject> heritageObjects = _manager.HeritageObjectRepository.GetAll();
            return View(heritageObjects);
        }

        public IActionResult Create()
        {
            ViewBag.Buildings = _manager.BuildingRepository.GetAll();
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateHeritageObjectViewModel model)
        {
            ViewBag.Buildings = _manager.BuildingRepository.GetAll();
            if (ModelState.IsValid)
            {
                string fileName = null;
                if (model.Image != null)
                {
                    fileName = await UploadFile(model.Image);
                }
                HeritageObject heritageObject = new HeritageObject
                {
                    Type = model.Type, 
                    Image = fileName, 
                    ShortDesc = model.ShortDesc, Desc = model.Desc,
                    Condition = model.Condition,
                    //BuildingId = model.Id,
                    Building = _manager.BuildingRepository.Get(model.BuildingId)
                };
                _manager.HeritageObjectRepository.Save(heritageObject);
                //return Redirect("/HeritageObject/Index");
                //return Redirect("/admin/building");
                return Redirect("/admin/heritageobject");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Buildings = _manager.BuildingRepository.GetAll();
            HeritageObject heritageObject = _manager.HeritageObjectRepository.Get(id);
            if (heritageObject == null)
            {
                return NotFound();
            }

            EditHeritageObjectViewModel model = new EditHeritageObjectViewModel
            {
                Id = heritageObject.Id,
                Type = heritageObject.Type,
                Condition = heritageObject.Condition,
                ShortDesc = heritageObject.ShortDesc,
                ImagePath = heritageObject.Image,
                Desc = heritageObject.Desc,
                BuildingId = heritageObject.BuildingId
            }; 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditHeritageObjectViewModel model)
        {
            ViewBag.Buildings = _manager.BuildingRepository.GetAll();
            if (ModelState.IsValid)
            {
                HeritageObject heritageObject = _manager.HeritageObjectRepository.Get(model.Id);
                if (heritageObject != null)
                {
                    if (model.Image != null)
                    {
                        if (heritageObject.Image != null)
                            DeleteFile(heritageObject.Image);
                        heritageObject.Image = await UploadFile(model.Image);
                    }

                    heritageObject.Type = model.Type;
                    heritageObject.Condition = model.Condition;
                    heritageObject.ShortDesc = model.ShortDesc;
                    heritageObject.Desc = model.Desc;
                    heritageObject.BuildingId = model.BuildingId;
                    heritageObject.Building = _manager.BuildingRepository.Get(model.BuildingId);
                    _manager.HeritageObjectRepository.Update(heritageObject);
                    //return Redirect("/HeritageObject/Index");
                    return Redirect("/admin/heritageobject");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        { 
            HeritageObject heritageObject = _manager.HeritageObjectRepository.Get(id);
            if (heritageObject != null)
            {
                _manager.HeritageObjectRepository.Delete(id);
            }
            return Redirect("/admin/heritageobject");
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