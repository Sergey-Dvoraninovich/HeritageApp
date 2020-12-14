using System.Collections.Generic;
using System.Linq;
using HeritageWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace HeritageWebApplication.Repository
{
    public class RenovationRepository : IRepository<RenovationCompany>
    {
        private readonly ApplicationDbContext _context;

        public RenovationRepository(ApplicationDbContext context)  
        {  
            this._context = context;
        }  
        
        public List<RenovationCompany> GetAll()
        {
            var renovationCompanies = _context.RenovationCompanies.Include(r=>r.Buildings).ToList();
            return renovationCompanies; 
        }

        public RenovationCompany Get(int id)  
        {  
            var renovationCompany = _context.RenovationCompanies.Include(r => r.Buildings).FirstOrDefault(t => t.Id == id);
            return renovationCompany;
        }
    
        public void Save(RenovationCompany renovationCompany)
        {
            if (renovationCompany.Buildings != null)
            {
                List<Building> buildings = renovationCompany.Buildings.ToList();
                renovationCompany.Buildings.Clear();
                //List<Building> buildings = new List<Building>();
                foreach (Building building in buildings)
                {
                    renovationCompany.Buildings.Add(_context.Buildings.FirstOrDefault(b => b.Id == building.Id));
                }
                //renovationCompany.Buildings = buildings;
            }
            _context.RenovationCompanies.Add(renovationCompany);
            _context.SaveChanges();
        }
        
        public void Update(RenovationCompany renovationCompany)
        {
            if (renovationCompany.Buildings != null)
            {
                List<Building> buildings = renovationCompany.Buildings.ToList();
                renovationCompany.Buildings.Clear();
                foreach (Building building in buildings)
                {
                    renovationCompany.Buildings.Add(_context.Buildings.FirstOrDefault(b => b.Id == building.Id));
                }
            }
            _context.RenovationCompanies.Update(renovationCompany);
            _context.SaveChanges();
        }
    
        public void Delete(int id)
        {
            var renovationCompany = _context.RenovationCompanies.Include(r => r.Buildings).FirstOrDefault(t => t.Id == id);
            if (renovationCompany != null)
            {
                _context.RenovationCompanies.Remove(renovationCompany);
            }
            _context.SaveChanges();
        }
    }
}