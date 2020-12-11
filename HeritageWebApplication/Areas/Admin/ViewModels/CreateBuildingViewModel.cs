using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace HeritageWebApplication.Areas.Admin.ViewModels
{
    public class CreateBuildingViewModel
    {
        public string Name { get; set; }
        
        [Display(Name = "Heritage object image")]
        [AllowedExtensions.AllowedExtensionsAttribute(new [] { ".jpg", ".jpeg", ".png" })]
        public IFormFile Image { get; set; }
        public string ShortDesc { get; set; }
        public string Location { get; set; }
        public string Desc { get; set; }
    }
}