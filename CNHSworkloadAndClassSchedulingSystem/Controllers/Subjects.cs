using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class Subjects : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
