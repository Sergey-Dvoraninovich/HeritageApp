using System.Collections.Generic;
using System.Linq;
using HeritageWebApplication.Models;
using Microsoft.AspNetCore.Identity;

namespace HeritageWebApplication.Repository
{
    public class CommentRepository : IRepository<Comment>
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CommentRepository(ApplicationDbContext context, UserManager<User> userManager)  
        {  
            this._context = context;
            this._userManager = userManager;
        }  
        
        public List<Comment> GetAll()
        {
            List <Comment> _comments = new List<Comment>();
            _comments = _context.Comments.ToList();
            return _comments; 
        }

        public Comment Get(int id)  
        {  
            var _comment = _context.Comments.FirstOrDefault(t => t.Id == id);
            return _comment;
        }
    
        public void Save(Comment comment)
        {
            var heritageObject = _context.HeritageObjects.FirstOrDefault(c=> c.Id==comment.HeritageObject.Id);
            comment.HeritageObject = heritageObject;
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void Update(Comment comment)
        {
            var heritageObject = _context.HeritageObjects.FirstOrDefault(c=> c.Id==comment.HeritageObject.Id);
            comment.HeritageObject = heritageObject;
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }
    
        public void Delete(int id)
        {
            var comment = _context.Comments.FirstOrDefault(t => t.Id == id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            _context.SaveChanges();
        }
    }
}