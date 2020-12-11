using System.Collections.Generic;

namespace HeritageWebApplication.Models
{
    public class HeritageObject
    {
        public int Id { get; set; }
        
        public string Type { get; set; }
        public string ShortDesc { get; set; }
        public string Desc { get; set; }
        public string Condition { get; set; }
        public string Image { get; set; }
        
        public Building Building { get; set; }  
        public int BuildingId { get; set; }
        
        public List<Comment> Comments { get; set; }
    }
}