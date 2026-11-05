using Microsoft.AspNetCore.Mvc;

namespace CNHSworkloadAndClassSchedulingSystem.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddTeacher()
        {
            return View();
        }
        public IActionResult EditTeacher()
        {
            return View();
        }
    }
}
