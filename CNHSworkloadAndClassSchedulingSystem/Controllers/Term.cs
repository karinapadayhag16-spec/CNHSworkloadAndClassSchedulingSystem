using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class Term : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
