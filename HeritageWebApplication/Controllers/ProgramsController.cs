using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HeritageWebApplication.Controllers
{
    public class ProgramsController : Controller
    {
        private IDataManager _manager;


        public ProgramsController(IDataManager dataManager)
        {
            _manager = dataManager;
        }

        public async Task<IActionResult> Index()
        {
            List<RenovationCompany> renovationCompanies = _manager.RenovatoinCompanyRepository.GetAll().ToList();
            return View(renovationCompanies);
        }
        
        public async Task<IActionResult> Detail(int id)
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
    }
}