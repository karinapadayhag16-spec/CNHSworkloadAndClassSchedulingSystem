using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class Documentation : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}