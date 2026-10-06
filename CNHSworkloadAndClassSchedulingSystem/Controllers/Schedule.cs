using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class Schedule : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
