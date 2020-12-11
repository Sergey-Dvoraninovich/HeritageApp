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
            _buildings = _context.Buildings.ToList();
            return _buildings; 
        }  
        
        public Building Get(int id)  
        {  
            var _building = _context.Buildings.FirstOrDefault(t => t.Id == id);
            return _building;
        }

        public void Update(Building building)
        {
            _context.Buildings.Update(building);
            _context.SaveChanges();
        }

        public void Save(Building building)
        {
            _context.Buildings.Add(building);
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