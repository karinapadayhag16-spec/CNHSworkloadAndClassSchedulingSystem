using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class TimeSchedController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddSchedule()
        {
            return View();
        }
      public IActionResult EditSchedule()
        {
            return View();
        }
            
    }
}
