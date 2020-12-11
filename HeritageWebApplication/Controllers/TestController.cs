using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HeritageWebApplication.Controllers
{
    public class TestController : Controller
    {
        IHubContext<ChatHub> hubContext;
        public TestController(IHubContext<ChatHub> hubContext)
        {
            this.hubContext = hubContext;
        }
         
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(string product)
        {
            await hubContext.Clients.All.SendAsync("Notify", $"Добавлено: {product} - {DateTime.Now.ToShortTimeString()}");
            return RedirectToAction("Index");
        }
        
    }
}