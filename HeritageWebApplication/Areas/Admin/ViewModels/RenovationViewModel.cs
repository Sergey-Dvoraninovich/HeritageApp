using System.Collections.Generic;
using HeritageWebApplication.Models;

namespace HeritageWebApplication.Areas.Admin.ViewModels
{
    public class RenovationViewModel
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Desc { get; set; }
        
        public int value { get; set; }
        
        public List<Building> Buildings { get; set; }
    }
}