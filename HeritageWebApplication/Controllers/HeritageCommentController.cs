using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.Services;
using HeritageWebApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HeritageWebApplication.Controllers
{
    public class HeritageCommentController : Controller
    {
        private ApplicationDbContext db;
        private IDataManager _manager;
        private IImageService _imageService;
        private IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;


        public HeritageCommentController(ApplicationDbContext context, IDataManager dataManager,
                                        IImageService imageService, IWebHostEnvironment environment,
                                        UserManager<User> userManager)
        {
            db = context;
            _manager = dataManager;
            _imageService = imageService;
            _environment = environment;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            ViewBag.HeritageObjectId = id;
            ViewBag.UserId = _userManager.GetUserId(User);
            ViewBag.HeritageObject = _manager.HeritageObjectRepository.Get(id);
            List<Comment> comments = _manager.CommentRepository.GetAll();
            List<Comment> result = new List<Comment>();
            foreach (var comment in comments)
            {
                if (comment.HeritageObjectId == id)
                    result.Add(comment);
            }
            return View(result);
        }
        
        public IActionResult CreateComment(int id)
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateComment(int id, CommentViewModel model)
        {

            if (ModelState.IsValid)
            {
                Comment comment = new Comment
                {
                    Text = model.Text,
                    Time = DateTime.UtcNow,
                    IsEdited = false,
                    HeritageObjectId = id,
                    HeritageObject = _manager.HeritageObjectRepository.Get(id),
                    UserId = Int32.Parse(_userManager.GetUserId(User)),
                };
                _manager.CommentRepository.Save(comment);
                return Redirect("/HeritageComment/Index/" + id.ToString());
            }
            return View(model);
        }

        public async Task<IActionResult> EditComment(int id)
        {
            Comment comment = _manager.CommentRepository.Get(id);
            HeritageObject heritageObject = _manager.HeritageObjectRepository.Get(comment.HeritageObjectId);
            if (comment == null)
            {
                return NotFound();
            }

            CommentViewModel model = new CommentViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                //Time = comment.Time,
                //IsEdited = comment.IsEdited,
                HeritageObjectId = comment.HeritageObjectId,
                HeritageObject = heritageObject,
                UserId = comment.UserId
            }; 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditComment(int id, CommentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Comment comment = _manager.CommentRepository.Get(model.Id);
                if (comment != null)
                {
                    comment.Text = model.Text;
                    comment.Time = DateTime.UtcNow;
                    comment.IsEdited = true;
                    comment.HeritageObject = _manager.HeritageObjectRepository.Get(comment.HeritageObjectId);
                    _manager.CommentRepository.Update(comment);
                    return Redirect("/HeritageComment/Index/" + comment.HeritageObjectId.ToString());
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteComment(int id)
        {
            Comment comment = _manager.CommentRepository.Get(id);
            if (comment != null)
            {
                int heritageId = comment.HeritageObjectId;
                _manager.CommentRepository.Delete(id);
                return Redirect("/HeritageComment/Index/" + heritageId.ToString());
            }
            return Redirect("");
        }

    }
}