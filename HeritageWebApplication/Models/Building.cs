using System.Collections.Generic;

namespace HeritageWebApplication.Models
{
    public class Building
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Image { get; set; }
        
        public string Location { get; set; }
        public string ShortDesc { get; set; }
        public string Desc { get; set; }
        
        public List<HeritageObject> HeritageObjects { get; set; }
    }
}