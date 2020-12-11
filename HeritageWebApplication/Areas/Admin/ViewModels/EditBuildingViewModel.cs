using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HeritageWebApplication.Areas.Admin.ViewModels
{
    public class EditBuildingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        [AllowedExtensions.AllowedExtensionsAttribute(new [] { ".jpg", ".jpeg", ".png" })]
        public IFormFile Image { get; set; }
        
        public string ImagePath { get; set; }
        public string ShortDesc { get; set; }
        public string Location { get; set; }
        public string Desc { get; set; }
    }
}