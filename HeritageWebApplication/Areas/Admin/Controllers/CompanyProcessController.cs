using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebApplication.Areas.Admin.ViewModels;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HeritageWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CompanyProcessController : Controller
    {
        private ApplicationDbContext db;
        private IDataManager _manager;
        private IWebHostEnvironment _environment;
        private IImageService _imageService;
        private IHubContext<ChatHub> _hubContext;

        public CompanyProcessController(ApplicationDbContext context, IDataManager dataManager, 
            IImageService imageService, IWebHostEnvironment environment,
            IHubContext<ChatHub> hubContext)
        {
            db = context;
            _manager = dataManager;
            _environment = environment;
            _imageService = imageService;
            _hubContext = hubContext;
        }
        
        public async Task<IActionResult> Show(int id)
        {
            
            RenovationCompany renovation = _manager.RenovatoinCompanyRepository.Get(id);
            if (renovation == null)
            {
                return NotFound();
            }

            List<Building> buildings = renovation.Buildings;
            ViewBag.Buildings = buildings;
            return View(renovation);
        }

        public async Task<IActionResult> Add(int id)
        {
            RenovationCompany renovationCompany = _manager.RenovatoinCompanyRepository.Get(id);
            List<Building> buildings = _manager.BuildingRepository.GetAll();
            
            List<int> data = new List<int>();
            List<Building> cur_buildings = renovationCompany.Buildings;
            foreach (Building building in cur_buildings)
            {
                data.Add(building.Id);
            }

            foreach (int local_id in data)
            {
                Building remove_buildnig = new Building();
                foreach (Building building in buildings)
                {
                    if (building.Id == local_id)
                    {
                        remove_buildnig = building;
                    }
                }
                buildings.Remove(remove_buildnig);
            }

            ViewBag.Buildings = buildings;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddBuildingViewModel model)
        {
            if (ModelState.IsValid)
            {
                RenovationCompany renovationCompany = _manager.RenovatoinCompanyRepository.Get(model.Id);
                Building building = _manager.BuildingRepository.Get(model.BuildingId);
                renovationCompany.Buildings.Add(building);
                _manager.RenovatoinCompanyRepository.Update(renovationCompany);
                return Redirect("~/admin/Companyprocess/show?id=" + model.Id);
            }
            return View(model);
        }
        
        public async Task<IActionResult> Remove(int id)
        {
            RenovationCompany renovationCompany = _manager.RenovatoinCompanyRepository.Get(id);
            ViewBag.Buildings = renovationCompany.Buildings;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Remove(AddBuildingViewModel model)
        {
            if (ModelState.IsValid)
            {
                RenovationCompany renovationCompany = _manager.RenovatoinCompanyRepository.Get(model.Id);
                Building building = _manager.BuildingRepository.Get(model.BuildingId);
                Building building_delete = new Building();
                foreach (Building loc_building in renovationCompany.Buildings)
                {
                    if (loc_building.Id == building.Id)
                        building_delete = loc_building;
                }
                renovationCompany.Buildings.Remove(building_delete);
                _manager.RenovatoinCompanyRepository.Update(renovationCompany);
                return Redirect("~/admin/Companyprocess/show?id=" + model.Id);
            }
            return View(model);
        }
    }
}