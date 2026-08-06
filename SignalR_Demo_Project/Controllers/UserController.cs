using Microsoft.AspNetCore.Mvc;

namespace SignalR_Demo_Project.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
