using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace HeritageWebApplication.Models
{
    public class User : IdentityUser<int>
    {
        //public string UserName { get; set; }
        public string Image { get; set; }
        public List<Comment> Comments { get; set; }
    }
}