using System.Collections.Generic;
using System.Linq;
using HeritageWebApplication.Models;
using Microsoft.EntityFrameworkCore;


namespace HeritageWebApplication.Repository
{
    public class BuildingRepository : IRepository<Building>
    {
        private readonly ApplicationDbContext _context;

        public BuildingRepository(ApplicationDbContext context)  
        {  
            this._context = context;
        }  
        
        public List<Building> GetAll()
        {
            List <Building> _buildings = new List<Building>();
            _buildings = _context.Buildings.Include(b => b.RenovationCompanies).ToList();
            return _buildings; 
        }

        public Building Get(int id)  
        {  
            var _building = _context.Buildings.Include(b => b.RenovationCompanies).FirstOrDefault(t => t.Id == id);
            return _building;
        }

        public void Update(Building building)
        {
            if (building.RenovationCompanies != null)
            {
                List<RenovationCompany> buildings = building.RenovationCompanies.ToList();
                building.RenovationCompanies.Clear();
                foreach (RenovationCompany renovation in buildings)
                {
                    building.RenovationCompanies.Add(_context.RenovationCompanies.FirstOrDefault(r => r.Id == renovation.Id));
                }
            }
            _context.Buildings.Update(building);
            _context.SaveChanges();
        }

        public void Save(Building building)
        {
            if (building.RenovationCompanies != null)
            {
                List<RenovationCompany> buildings = building.RenovationCompanies.ToList();
                building.RenovationCompanies.Clear();
                foreach (RenovationCompany renovation in buildings)
                {
                    building.RenovationCompanies.Add(_context.RenovationCompanies.FirstOrDefault(r => r.Id == renovation.Id));
                }
            }
            _context.Add(building);
            _context.SaveChanges();
        }
    
        public void Delete(int id)
        {
            var building = _context.Buildings.FirstOrDefault(t => t.Id == id);
            if (building != null)
            {
                _context.Buildings.Remove(building);
            }
            _context.SaveChanges();
        }
        
    }
}