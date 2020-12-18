using System;
using HeritageWebApplication.Models;

namespace HeritageWebApplication.ViewModels
{
    public class CommentViewModel
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