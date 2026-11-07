using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class SubjectController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddSubject()
        {
            return View();
        }
        public IActionResult EditSubject()
        {
            return View();
        }
    }
}
