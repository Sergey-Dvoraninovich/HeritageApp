using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HeritageWebApplication.Models;
using HeritageWebApplication.Repository;
using HeritageWebApplication.Services;
using HeritageWebApplication.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HeritageWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CommentController : Controller
    {
        private ApplicationDbContext db;
        private IDataManager _manager;
        private IImageService _imageService;
        private IWebHostEnvironment _environment;
        private readonly UserManager<User> _userManager;
        private IHubContext<ChatHub> _hubContext;


        public CommentController(ApplicationDbContext context, IDataManager dataManager,
                                        IImageService imageService, IWebHostEnvironment environment,
                                        UserManager<User> userManager, IHubContext<ChatHub> hubContext)
        {
            db = context;
            _manager = dataManager;
            _imageService = imageService;
            _environment = environment;
            _userManager = userManager;
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            List<Comment> comments = _manager.CommentRepository.GetAll();
            return View(comments);
        }

        public IActionResult Create()
        {
            ViewBag.HeritageObjects = _manager.HeritageObjectRepository.GetAll();
            ViewBag.Users = _userManager.Users.Where(u=> true);
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CommentAdminViewModel model)
        {
            ViewBag.HeritageObjects = _manager.HeritageObjectRepository.GetAll();
            ViewBag.Users = _userManager.Users.Where(u=> true);
            if (ModelState.IsValid)
            {
                Comment comment = new Comment
                {
                    Text = model.Text,
                    Time = DateTime.Now.ToString() + "  ",
                    IsEdited = false,
                    HeritageObjectId = model.HeritageObjectId,
                    HeritageObject = _manager.HeritageObjectRepository.Get(model.HeritageObjectId),
                    UserId = model.UserId,
                };
                _manager.CommentRepository.Save(comment);
                return Redirect("/admin/comment");
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.HeritageObjects = _manager.HeritageObjectRepository.GetAll();
            ViewBag.Users = _userManager.Users.Where(u=> true);
            Comment comment = _manager.CommentRepository.Get(id);
            if (comment == null)
            {
                return NotFound();
            }

            CommentAdminViewModel model = new CommentAdminViewModel
            {
                Id = comment.Id,
                Text = comment.Text,
                HeritageObjectId = comment.HeritageObjectId,
                UserId = comment.UserId
            }; 
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentAdminViewModel model)
        {
            ViewBag.HeritageObjects = _manager.HeritageObjectRepository.GetAll();
            ViewBag.Users = _userManager.Users.Where(u=> true);
            if (ModelState.IsValid)
            {
                Comment comment = _manager.CommentRepository.Get(model.Id);
                if (comment != null)
                {
                    comment.Text = model.Text;
                    comment.Time = DateTime.Now.ToString() + "  ";
                    comment.IsEdited = true;
                    comment.HeritageObjectId = model.HeritageObjectId;
                    comment.HeritageObject = _manager.HeritageObjectRepository.Get(model.HeritageObjectId);
                    comment.UserId = model.UserId;
                    _manager.CommentRepository.Update(comment);
                    await _hubContext.Clients.User(comment.UserId.ToString()).SendAsync("Notify", "Ваш комментарий был изменён");
                    return Redirect("/admin/comment/index");
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        { 
            Comment comment = _manager.CommentRepository.Get(id);
            if (comment != null)
            {
                _manager.CommentRepository.Delete(id);
                await _hubContext.Clients.User(comment.UserId.ToString()).SendAsync("Notify", "Ваш комментарий был удалён");
            }
            return Redirect("/admin/comment/index");
        }

    }
}