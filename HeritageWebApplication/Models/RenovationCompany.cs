using System.Collections.Generic;

namespace HeritageWebApplication.Models
{
    public class RenovationCompany
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Desc { get; set; }
        
        public int value { get; set; }
        
        public List<Building> Buildings { get; set; }
    }
}