using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class Teachers : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
