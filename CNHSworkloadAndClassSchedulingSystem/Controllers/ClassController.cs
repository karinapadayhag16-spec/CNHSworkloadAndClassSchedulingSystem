using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class Classes : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
