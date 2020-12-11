using System.Collections.Generic;
using HeritageWebApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace HeritageWebApplication.Areas.Admin.ViewModels
{
    public class ChangeRoleViewModel {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public List<UserRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
    
        public ChangeRoleViewModel()
        {
            AllRoles = new List<UserRole>();
            UserRoles = new List<string>();
        }
    }
}