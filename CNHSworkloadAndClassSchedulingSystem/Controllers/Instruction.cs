using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class Instruction : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}