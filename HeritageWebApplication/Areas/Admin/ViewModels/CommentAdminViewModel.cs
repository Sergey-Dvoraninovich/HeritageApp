using System;
using HeritageWebApplication.Models;

namespace HeritageWebApplication.Areas.Admin.ViewModels
{
    public class CommentAdminViewModel
    {
        public int Id { get; set; }
        
        public string Text { get; set; }
        public bool IsEdited { get; set; }
        public DateTime Time { get; set; }
        
        public HeritageObject HeritageObject { get; set; }  
        public int HeritageObjectId { get; set; }
        
        public User User { get; set; }  
        public int UserId { get; set; }
    }
}