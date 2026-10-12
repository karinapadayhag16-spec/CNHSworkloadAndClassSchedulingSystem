using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class SchoolYear : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
