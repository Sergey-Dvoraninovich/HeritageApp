using System.Collections.Generic;
using System.Linq;
using HeritageWebApplication.Models;

namespace HeritageWebApplication.Repository
{
    public class HeritageObjectRepository : IRepository<HeritageObject>
    {
        private readonly ApplicationDbContext _context;

        public HeritageObjectRepository(ApplicationDbContext context)  
        {  
            this._context = context;
        }  
        
        public List<HeritageObject> GetAll()
        {
            List <HeritageObject> _heritages = new List<HeritageObject>();
            _heritages = _context.HeritageObjects.ToList();
            return _heritages; 
        }

        public HeritageObject Get(int id)  
        {  
            var _heritage = _context.HeritageObjects.FirstOrDefault(t => t.Id == id);
            return _heritage;
        }
    
        public void Save(HeritageObject heritage)
        {
            var building = _context.Buildings.FirstOrDefault(b=> b.Id==heritage.Building.Id);
            heritage.Building = building;
            _context.HeritageObjects.Add(heritage);
            _context.SaveChanges();
        }
        
        public void Update(HeritageObject heritageObject)
        {
            var building = _context.Buildings.FirstOrDefault(b=> b.Id==heritageObject.Building.Id);
            heritageObject.Building = building;
            _context.HeritageObjects.Update(heritageObject);
            _context.SaveChanges();
        }
    
        public void Delete(int id)
        {
            var heritage = _context.HeritageObjects.FirstOrDefault(t => t.Id == id);
            if (heritage != null)
            {
                _context.HeritageObjects.Remove(heritage);
            }
            _context.SaveChanges();
        }
    }
}