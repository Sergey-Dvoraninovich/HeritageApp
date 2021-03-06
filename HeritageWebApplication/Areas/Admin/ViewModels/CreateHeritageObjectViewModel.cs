﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HeritageWebApplication.Models;
using Microsoft.AspNetCore.Http;

namespace HeritageWebApplication.Areas.Admin.ViewModels
{
    public class CreateHeritageObjectViewModel
    {
        public int Id { get; set; }
        
        public string Type { get; set; }
        public string ShortDesc { get; set; }
        public string Desc { get; set; }
        public string Condition { get; set; }

        [Display(Name = "Heritage object image")]
        [AllowedExtensions.AllowedExtensionsAttribute(new [] { ".jpg", ".jpeg", ".png" })]
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; }

        public Building Building { get; set; }  
        public int BuildingId { get; set; }
        
        public List<Comment> Comments { get; set; }
    }
}