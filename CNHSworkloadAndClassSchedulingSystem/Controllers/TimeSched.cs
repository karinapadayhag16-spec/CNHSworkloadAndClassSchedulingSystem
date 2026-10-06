using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class TimeSched : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
